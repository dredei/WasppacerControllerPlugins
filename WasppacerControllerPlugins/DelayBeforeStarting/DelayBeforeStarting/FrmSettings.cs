#region Using

using System;
using System.Windows.Forms;
using WACPlugIn;

#endregion

namespace DelayBeforeStarting
{
    public partial class FrmSettings : Form
    {
        private readonly IPlugin _plugin;
        private readonly ISettings _settings;

        public FrmSettings( IPlugin plugin, ISettings settings )
        {
            this._plugin = plugin;
            this._settings = settings;
            this.InitializeComponent();
            this.LoadSettings();
        }

        private void LoadSettings()
        {
            int interval = 0;
            object intervalObj = this._settings[ this._plugin, "DelayInterval" ];
            if ( intervalObj != null )
            {
                interval = int.Parse( intervalObj.ToString() );
            }
            this.nudDelay.Value = interval;
        }

        private void SaveSettings()
        {
            this._settings[ this._plugin, "DelayInterval" ] = (int)this.nudDelay.Value;
        }

        private void btnSave_Click( object sender, EventArgs e )
        {
            this.SaveSettings();
            this.Close();
        }
    }
}