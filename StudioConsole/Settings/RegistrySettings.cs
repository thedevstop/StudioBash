using Microsoft.Win32;

namespace TheDevStop.StudioConsole.Settings
{
    public abstract class RegistrySettings
    {
        protected RegistryKey UserRegistryRoot { get; set; }

        public abstract void SaveSettings();
        public abstract void LoadSettings();

        public void Initialize(RegistryKey userRegistryRoot)
        {
            this.UserRegistryRoot = userRegistryRoot;
            this.LoadSettings();
        }

        protected T GetUserRegistryValue<T>(string subKey, string name, T defaultValue)
        {
            try
            {
                using (Microsoft.Win32.RegistryKey registryKey = this.UserRegistryRoot.OpenSubKey(subKey))
                {
                    if (registryKey == null)
                        return defaultValue;

                    object value = registryKey.GetValue(name);
                    if (value == null)
                        return defaultValue;

                    bool flag;
                    if (typeof(T) == typeof(bool) &&
                        value is string &&
                        bool.TryParse((string)value, out flag))
                        value = flag;

                    if (value is T)
                        return (T)value;
                }
            }
            catch (System.Exception)
            {
            }

            return defaultValue;
        }

        protected void SetUserRegistryValue(string subKey, string name, object value)
        {
            try
            {
                using (Microsoft.Win32.RegistryKey registryKey = this.UserRegistryRoot.CreateSubKey(subKey))
                {
                    registryKey.SetValue(name, value);
                }
            }
            catch (System.Exception)
            {
            }
        }
    }
}
