#region Using

using System;
using System.Threading.Tasks;
using WACPlugIn;

#endregion

namespace DelayBeforeStarting
{
    public class PlugIn : IPlugin
    {
        private ISettings _settings;
        private int _count;

        private int Interval
        {
            get
            {
                int interval = 0;
                object intervalObj = this._settings[ this, "DelayInterval" ];
                if ( intervalObj != null )
                {
                    interval = int.Parse( intervalObj.ToString() );
                }
                return interval;
            }
        }

        private async Task Delay()
        {
            if ( this._count > 0 )
            {
                await TaskEx.Delay( this.Interval );
            }
            this._count++;
        }

        #region Members

        public void Activate( params object[] paramsArr )
        {
            this._settings = paramsArr[ 0 ] as ISettings;
        }

        public async Task<object> Event( WaspEnvent eventName, params object[] paramsArr )
        {
            switch ( eventName )
            {
                case WaspEnvent.BeforeStartWasppacer:
                    await this.Delay();
                    break;

                case WaspEnvent.AfterStartAllWasppacer:
                    this._count = 0;
                    break;
            }
            return null;
        }

        public void ShowSettingsForm()
        {
            var frm = new FrmSettings( this, this._settings );
            frm.ShowDialog();
        }

        #region InfoFields

        public string PluginName
        {
            get { return "DelayBeforeStarting"; }
        }

        public string DisplayPluginName
        {
            get { return "DelayBeforeStarting"; }
        }

        public string PluginDescription
        {
            get { return "Задержка между запуском копий Wasppacer"; }
        }

        public string Author
        {
            get { return "dredei"; }
        }

        public Version Version
        {
            get { return new Version( "1.0.0" ); }
        }

        #endregion

        #endregion
    }
}