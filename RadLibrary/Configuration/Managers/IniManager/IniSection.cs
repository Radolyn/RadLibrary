#region

using System;

#endregion

namespace RadLibrary.Configuration.Managers.IniManager
{
    public class IniSection : IConfigurationSection
    {
        public IniSection(string key, string value, string comment, bool valueQuoted)
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
        public TU ValueAs<TU>() where TU : new()
        {
            return (TU) Convert.ChangeType(Value, typeof(TU));
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Value;
        }

        public static implicit operator IniSection(string val)
        {
            return new IniSection("", val, null, false);
        }

        public static implicit operator IniSection(bool val)
        {
            return val.ToString();
        }
    }
}