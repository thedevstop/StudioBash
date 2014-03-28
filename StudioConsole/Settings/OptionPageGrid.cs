using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;

namespace TheDevStop.StudioConsole.Settings
{
    [ClassInterface(ClassInterfaceType.AutoDual)]
    [CLSCompliant(false), ComVisible(true)]
    public class OptionPageGrid : DialogPage
    {
        public override object AutomationObject
        {
            get
            {
                return SCSettings.Instance;
            }
        }

        public override void LoadSettingsFromStorage()
        {
            base.LoadSettingsFromStorage();
            SCSettings.Instance.SaveSettings();
        }

        public override void SaveSettingsToStorage()
        {
            base.SaveSettingsToStorage();
            SCSettings.Instance.LoadSettings();
        }

        public override void ResetSettings()
        {
            SCSettings.Instance.ConsolePath = string.Empty;
            SCSettings.Instance.SaveSettings();
        }
    }
}