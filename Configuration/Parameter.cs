namespace RadLibrary.Configuration
{
    public class Parameter
    {
        /// <summary>
        ///     The value
        /// </summary>
        public string Value { get; internal set; }

        /// <summary>
        ///     The comment
        /// </summary>
        public string Comment { get; internal set; }

        /// <summary>
        ///     Initializes class with specified value & comment
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="comment">The comment</param>
        public Parameter(string value, string comment)
        {
            Value = value;
            Comment = comment;
        }
    }
}