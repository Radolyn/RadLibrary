using System;

namespace RadLibrary.Configuration
{
    public interface IConfigurationManager
    {
        void Setup(string name);
        
        string GetString(string key);
        int GetInteger(string key);
        bool GetBool(string key);
        
        void SetString(string key, string value);
        void SetInteger(string key, int value);
        void SetBool(string key, bool value);

        void RemoveKey(string key);

        void Save();
    }
}