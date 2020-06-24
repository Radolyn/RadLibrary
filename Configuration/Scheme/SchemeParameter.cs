using System;

namespace RadLibrary.Configuration.Scheme
{
        public class SchemeParameter : Parameter
        {
            public Type Type;

            public SchemeParameter()
            {
                
            }
            
            public SchemeParameter(string key, object value, string comment, Type type)
            {
                Key = key;
                Value = value.ToString();
                Comment = comment;
                Type = type;
            }
        }
}