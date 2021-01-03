#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using RadLibrary.Configuration.Scheme;

#endregion

namespace RadLibrary.Configuration.Managers.IniManager
{
    public class IniManager : IManager<IniSection>
    {
        private readonly Encoding _encoding;
        private readonly string _fileName;
        private readonly List<IniSection> _sections;

        public IniManager(string fileName, bool preLoad = true, Encoding encoding = null)
        {
            _fileName = fileName;
            _encoding = encoding ?? Encoding.UTF8;

            _sections = new List<IniSection>();

            if (preLoad)
                // ReSharper disable once VirtualMemberCallInConstructor
                Load();
        }

        /// <inheritdoc />
        public IEnumerable<IniSection> Sections => _sections;

        /// <inheritdoc />
        public virtual IniSection this[string key]
        {
            get => GetSection(key);
            set => SetSection(key, value);
        }

        /// <inheritdoc />
        public virtual IniSection GetSection(string section)
        {
            return Sections.First(x => x.Key.Equals(section, StringComparison.OrdinalIgnoreCase));
        }

        /// <inheritdoc />
        public virtual void SetSection(string key, IniSection section)
        {
            section.Key = key;

            var predicate = Sections.FirstOrDefault(x => x.Key == key);

            if (predicate != null)
            {
                predicate.SetValue(section.Value);
                return;
            }

            _sections.Add(section);
        }

        /// <inheritdoc />
        public virtual void Load()
        {
            _sections.Clear();

            using var stream = File.Open(_fileName, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Inheritable);
            using var reader = new StreamReader(stream, _encoding);

            var sb = new StringBuilder();

            while (!reader.EndOfStream)
            {
                var s = reader.ReadLine();

                if (s == "\n" || s == "\r\n")
                    continue;

                if (string.IsNullOrWhiteSpace(s) || s == "\n" || s == "\r\n")
                    continue;

                if (s.StartsWith("#", StringComparison.Ordinal))
                {
                    sb.Append(s.Replace("\n# ", "\n"));
                    continue;
                }

                var split = s.Split(new[] {'='}, 2, StringSplitOptions.RemoveEmptyEntries);

                var key = split[0].Trim();
                var value = split[1].Trim();
                var comment = sb.Length == 0 ? null : sb.ToString();
                var valueSeparated = false;

                if (value.StartsWith("\"", StringComparison.Ordinal) && value.EndsWith("\"", StringComparison.Ordinal))
                {
                    value = value.Trim('"');
                    valueSeparated = true;
                }

                if (_sections.Any(x => x.Key == key))
                    throw new ArgumentException($"Key {key} duplicated");

                _sections.Add(new IniSection(key, value, comment, valueSeparated));
            }

            reader.Dispose();
            stream.Dispose();
        }

        /// <inheritdoc />
        public virtual void Save()
        {
            using var stream = File.Open(_fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Inheritable);
            using var writer = new StreamWriter(stream, _encoding);

            foreach (var section in _sections)
            {
                if (section.Comment != null)
                {
                    writer.Write(section.Comment.Replace("\n", "\n# "));

                    writer.Write("\n");
                }

                var value = section.ValueQuoted ? $"\"{section.Value}\"" : section.Value;

                writer.Write($"{section.Key} = {value}");

                writer.Write("\n\n");
            }
        }

        /// <inheritdoc />
        public void EnsureScheme(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (type.GetConstructors().All(x => x.GetParameters().Length != 0))
                throw new ArgumentException("The scheme class doesn't have parameterless constructor", nameof(type));

            var instance = Activator.CreateInstance(type);

            foreach (var member in type.GetMembers())
            {
                var attr = member.GetCustomAttribute<SchemeSectionAttribute>();

                if (attr == null)
                    continue;

                var section = _sections.FirstOrDefault(x => x.Key == attr.Key);

                Type paramType = null;
                object defaultValue = null;

                // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
                switch (member.MemberType)
                {
                    case MemberTypes.Field:
                        var field = (FieldInfo) member;
                        paramType = field.FieldType;
                        defaultValue = field.GetValue(instance);
                        break;
                    case MemberTypes.Property:
                        var property = (PropertyInfo) member;
                        paramType = property.PropertyType;
                        defaultValue = property.GetValue(instance);
                        break;
                }

                if (section != null)
                {
                    if (paramType == null)
                        continue;

                    try
                    {
                        _ = Convert.ChangeType(section.Value, paramType);
                    }
                    catch
                    {
                        section.SetValue(defaultValue ?? paramType.GetDefault());
                    }
                }
                else
                {
                    var key = attr.Key ?? member.Name.FirstCharacterToLower();

                    var val = (string) Convert.ChangeType(defaultValue ?? paramType.GetDefault(), TypeCode.String);

                    _sections.Add(new IniSection(key, val, attr.Comment, false));
                }
            }
        }
    }
}