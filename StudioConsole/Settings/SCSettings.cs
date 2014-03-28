using Microsoft.Win32;
using System.ComponentModel;

namespace TheDevStop.StudioConsole.Settings
{
    public class SCSettings : RegistrySettings
    {
        public static readonly SCSettings Instance = new SCSettings();

        [Category("Settings")]
        [DisplayName("Console Path")]
        [Description("The path to the console emulator to embed.")]
        [Editor(typeof(ExecutableNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string ConsolePath { get; set; }

        public SCSettings()
        {
        }

        public override void SaveSettings()
        {
            this.SetUserRegistryValue("StudioConsole\\General", "ConsolePath", ConsolePath);
        }

        public override void LoadSettings()
        {
            this.ConsolePath = this.GetUserRegistryValue<string>("StudioConsole\\General", "ConsolePath", string.Empty);
        }
    }
}