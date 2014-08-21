#region Using

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WACPlugIn;

#endregion

namespace WasppacerHider
{
    public class PlugIn : IPlugin
    {
        #region WinAPI

        private delegate bool EnumThreadDelegate( IntPtr hWnd, IntPtr lParam );

        [DllImport( "user32.dll" )]
        private static extern bool EnumThreadWindows( int dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam );

        private const int SW_HIDE = 0;
        private const int SW_SHOWNORMAL = 1;
        private const int WS_EX_APPWINDOW = 0x40000;
        private const int GWL_EXSTYLE = -0x14;

        [DllImport( "user32.dll" )]
        private static extern bool ShowWindow( IntPtr hwnd, int nCmdShow );

        [DllImport( "user32.dll", CharSet = CharSet.Auto )]
        private static extern int GetWindowText( IntPtr hWnd, StringBuilder title, int size );

        [DllImport( "User32.dll" )]
        private static extern int SetWindowLong( IntPtr hWnd, int nIndex, int dwNewLong );

        [DllImport( "User32.dll" )]
        private static extern int GetWindowLong( IntPtr hWnd, int nIndex );

        [DllImport( "user32.dll" )]
        private static extern bool SetForegroundWindow( IntPtr hWnd );

        private static IEnumerable<IntPtr> EnumerateProcessWindowHandles( int processId )
        {
            var handles = new List<IntPtr>();

            foreach ( ProcessThread thread in Process.GetProcessById( processId ).Threads )
            {
                EnumThreadWindows( thread.Id,
                    ( hWnd, lParam ) =>
                    {
                        handles.Add( hWnd );
                        return true;
                    }, IntPtr.Zero );
            }

            return handles;
        }

        #endregion

        private ISettings _settings;
        private bool _showThenHide = false;
        private int _showThenHideCount = 0;

        #region Настройки

        private bool AlwaysHideWasppacerSett
        {
            get
            {
                bool chkd = false;
                object chkdObj = this._settings[ this, "AlwaysHideWasppacer" ];
                if ( chkdObj != null )
                {
                    bool.TryParse( chkdObj.ToString(), out chkd );
                }
                return chkd;
            }
        }

        private bool ShowWasppacerSett
        {
            get
            {
                bool chkd = true;
                object chkdObj = this._settings[ this, "ShowWasppacer" ];
                if ( chkdObj != null )
                {
                    bool.TryParse( chkdObj.ToString(), out chkd );
                }
                return chkd;
            }
            set { this._settings[ this, "ShowWasppacer" ] = value; }
        }

        private int ShowThenHideDelay
        {
            get
            {
                int res = 5000;
                object resObj = this._settings[ this, "ShowThenHideDelay" ];
                if ( resObj != null )
                {
                    int.TryParse( resObj.ToString(), out res );
                }
                return res;
            }
        }

        #endregion

        private async Task HideShowWasppacer( bool hide = false )
        {
            await TaskEx.Run( () =>
            {
                Process[] processes = Process.GetProcessesByName( "Wasppacer" );
                foreach ( Process process in processes )
                {
                    var forms = EnumerateProcessWindowHandles( process.Id ).ToList();
                    foreach ( var h in forms )
                    {
                        StringBuilder title = new StringBuilder( 256 );
                        GetWindowText( h, title, 256 );
                        if ( !title.ToString().Contains( "Wasppacer" ) )
                        {
                            continue;
                        }
                        if ( hide )
                        {
                            ShowWindow( h, SW_HIDE );
                        }
                        else
                        {
                            if ( title.ToString() == "Wasppacer" || title.ToString() == "[#] Wasppacer [#]" )
                            {
                                continue;
                            }
                            ShowWindow( h, SW_SHOWNORMAL );
                            SetWindowLong( h, GWL_EXSTYLE, GetWindowLong( h, GWL_EXSTYLE ) | WS_EX_APPWINDOW );
                            SetForegroundWindow( h );
                        }
                    }
                }
            } );
        }

        private async Task ShowWasppacer()
        {
            this.ShowWasppacerSett = true;
            await this.HideShowWasppacer();
        }

        private async Task HideWasppacer()
        {
            this.ShowWasppacerSett = false;
            await this.HideShowWasppacer( true );
        }

        #region Members

        public WaspEnvent[] Activate( params object[] paramsArr )
        {
            this._settings = paramsArr[ 0 ] as ISettings;
            return new[] { WaspEnvent.CreateMenuItem, WaspEnvent.MenuItemClicked, WaspEnvent.TimerTick };
        }

        public async Task<object> Event( WaspEnvent eventName, params object[] paramsArr )
        {
            switch ( eventName )
            {
                case WaspEnvent.CreateMenuItem:
                    return await this.CreateMenuItem();

                case WaspEnvent.MenuItemClicked:
                    await this.MenuItemClicked( paramsArr[ 0 ].ToString() );
                    return null;

                case WaspEnvent.TimerTick:
                    if ( this._showThenHide )
                    {
                        this._showThenHideCount += 250;
                        if ( this._showThenHideCount >= this.ShowThenHideDelay )
                        {
                            await this.HideWasppacer();
                        }
                    }
                    if ( !this.ShowWasppacerSett && this.AlwaysHideWasppacerSett )
                    {
                        await this.HideShowWasppacer( true );
                    }
                    break;
            }
            return null;
        }

        private async Task<object> CreateMenuItem()
        {
            return await TaskEx.Run( () => new Dictionary<string, string>
            {
                { "Скрыть Wasppacer", "HideWasppacer" },
                { "Показать Wasppacer", "ShowWasppacer" },
                { "Показать и скрыть", "ShowThenHide" }
            } );
        }

        private async Task MenuItemClicked( string functionName )
        {
            switch ( functionName )
            {
                case "HideWasppacer":
                    await this.HideWasppacer();
                    break;

                case "ShowWasppacer":
                    await this.ShowWasppacer();
                    break;

                case "ShowThenHide":
                    await this.ShowWasppacer();
                    this._showThenHide = true;
                    this._showThenHideCount = 0;
                    break;
            }
        }

        public void ShowSettingsForm()
        {
            var frm = new FrmSettings( this, this._settings );
            frm.ShowDialog();
        }

        #region Info fields

        public string PluginName
        {
            get { return "WasppacerHider"; }
        }

        public string DisplayPluginName
        {
            get { return "Wasppacer Hider"; }
        }

        public string PluginDescription
        {
            get { return "Скрывает Wasppacer"; }
        }

        public string Author
        {
            get { return "dredei"; }
        }

        public Version Version
        {
            get { return new Version( "1.0.2" ); }
        }

        #endregion

        #endregion
    }
}