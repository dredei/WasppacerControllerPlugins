#region Using

using System;
using System.Windows.Forms;
using WACPlugIn;

#endregion

namespace WasppacerHider
{
    public partial class FrmSettings : Form
    {
        private readonly ISettings _settings;
        private readonly IPlugin _plugin;

        public FrmSettings( IPlugin plugin, ISettings settings )
        {
            this._settings = settings;
            this._plugin = plugin;
            this.InitializeComponent();
            this.LoadSettings();
        }

        private void LoadSettings()
        {
            bool chkd = false;
            object chkdObj = this._settings[ this._plugin, "AlwaysHideWasppacer" ];
            if ( chkdObj != null )
            {
                chkd = bool.Parse( chkdObj.ToString() );
            }
            this.cbAlwaysHideWasppacer.Checked = chkd;
        }

        private void SaveSettings()
        {
            this._settings[ this._plugin, "AlwaysHideWasppacer" ] = this.cbAlwaysHideWasppacer.Checked;
        }

        private void btnOk_Click( object sender, EventArgs e )
        {
            this.SaveSettings();
            this.Close();
        }
    }
}