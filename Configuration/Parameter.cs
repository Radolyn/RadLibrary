namespace RadLibrary.Configuration
{
    public class Parameter
    {
        /// <summary>
        ///     The key
        /// </summary>
        public string Key { get; internal set; }
        
        /// <summary>
        ///     The value
        /// </summary>
        public string Value { get; internal set; }

        /// <summary>
        ///     The comment
        /// </summary>
        public string Comment { get; internal set; }

        /// <summary>
        /// Initializes class
        /// </summary>
        public Parameter()
        {
            
        }

        /// <summary>
        ///     Initializes class with specified value & comment
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">The value</param>
        /// <param name="comment">The comment</param>
        public Parameter(string key, string value, string comment)
        {
            Key = key;
            Value = value;
            Comment = comment;
        }
    }
}