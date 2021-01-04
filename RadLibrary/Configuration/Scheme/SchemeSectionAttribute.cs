#region

using System;

#endregion

namespace RadLibrary.Configuration.Scheme
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class SchemeSectionAttribute : Attribute
    {
        public SchemeSectionAttribute()
        {
        }

        public SchemeSectionAttribute(string key)
        {
            Key = key;
        }

        public string Key { get; }

        public string Comment { get; set; }
    }
}