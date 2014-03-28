using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace TheDevStop.StudioConsole.Settings
{
    public class ExecutableNameEditor : FileNameEditor
    {
        protected override void InitializeDialog(OpenFileDialog openFileDialog)
        {
            base.InitializeDialog(openFileDialog);
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Executable|*.exe|All|*.*";
        }
    }
}