#region

using System.Collections.Generic;

#endregion

namespace RadLibrary.Configuration
{
    public interface IConfigurationManager
    {
        event ConfigurationUpdated ConfigurationUpdated;

        bool HotReload { get; set; }

        void Setup(string name);

        Dictionary<string, Parameter> GetParameters();

        string GetString(string key);
        int GetInteger(string key);
        bool GetBool(string key);

        void SetString(string key, string value);
        void SetInteger(string key, int value);
        void SetBool(string key, bool value);

        void SetComment(string key, string comment);

        void RemoveKey(string key);

        void Save();
    }
}