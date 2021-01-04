#region

using System;
using JetBrains.Annotations;

#endregion

namespace RadLibrary.Configuration.Managers.IniManager
{
    public class IniSection : IConfigurationSection
    {
        public IniSection([NotNull] string key, string value, string comment, bool valueQuoted)
        {
            Key = key;
            Value = value;
            Comment = comment;
            ValueQuoted = valueQuoted;
        }

        /// <summary>
        ///     Defines if value needs to be quoted
        /// </summary>
        public bool ValueQuoted { get; }

        /// <inheritdoc />
        public virtual IConfigurationSection this[string key] => throw new NotSupportedException();

        /// <inheritdoc />
        public string Key { get; internal set; }

        /// <inheritdoc />
        public string Value { get; private set; }

        /// <inheritdoc />
        public string Comment { get; set; }

        /// <inheritdoc />
        public virtual void SetValue<T>(T value)
        {
            // todo: add support for lists of primitive data types

            if (value is IConfigurationSection)
                throw new NotSupportedException();

            Value = (string) Convert.ChangeType(value, TypeCode.String);
        }

        /// <inheritdoc />
        [CanBeNull]
        public TU ValueAs<TU>() where TU : new()
        {
            return (TU) Convert.ChangeType(Value, typeof(TU));
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Value;
        }

        [NotNull]
        public static implicit operator IniSection(string val)
        {
            return new("", val, null, false);
        }

        [NotNull]
        public static implicit operator IniSection(int val)
        {
            return new("", val.ToString(), null, false);
        }

        [NotNull]
        public static implicit operator IniSection(bool val)
        {
            return val.ToString();
        }
    }
}