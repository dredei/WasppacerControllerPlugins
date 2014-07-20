#region Using

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using WACPlugIn;

#endregion

namespace PowerControll
{
    public class PlugIn : IPlugin
    {
        private IWaController _waController;

        /// <summary>
        /// Отключает компьютер (через минуту после вызова)
        /// </summary>
        /// <returns></returns>
        private async Task Shutdown()
        {
            await TaskEx.Run( () => { Process.Start( "shutdown", "-s -t 60" ); } );
        }

        /// <summary>
        /// Переводит компьютер в спящий режим
        /// </summary>
        /// <returns></returns>
        private async Task Hibernate()
        {
            await TaskEx.Run( () => { Application.SetSuspendState( PowerState.Hibernate, true, false ); } );
        }

        private async Task<object> CreateMenuItem()
        {
            return await TaskEx.Run( () => new Dictionary<string, string>
            {
                { "Выключить компьютер после завершения Wasppacer", "ShutdownAfterCloseWasppacer" },
                { "Перейти в спящий режим после завершения Wasppacer", "HibernateAfterCloseWasppacer" }
            } );
        }

        private async Task MenuItemClicked( string functionName )
        {
            switch ( functionName )
            {
                case "ShutdownAfterCloseWasppacer":
                    await this._waController.StopWasppacerAsync( true );
                    await this.Shutdown();
                    break;

                case "HibernateAfterCloseWasppacer":
                    await this._waController.StopWasppacerAsync( true );
                    await this.Hibernate();
                    break;
            }
        }

        #region Members

        public WaspEnvent[] Activate( params object[] paramsArr )
        {
            if ( paramsArr.Length > 1 && paramsArr[ 1 ] != null )
            {
                this._waController = (IWaController)paramsArr[ 1 ];
            }
            return new[] { WaspEnvent.CreateMenuItem, WaspEnvent.MenuItemClicked };
        }

        public async Task<object> Event( WaspEnvent eventName, params object[] paramsArr )
        {
            switch ( eventName )
            {
                case WaspEnvent.CreateMenuItem:
                    return await this.CreateMenuItem();

                case WaspEnvent.MenuItemClicked:
                    await this.MenuItemClicked( paramsArr[ 0 ].ToString() );
                    break;
            }
            return null;
        }

        #region InfoFields

        public void ShowSettingsForm()
        {
        }

        public string PluginName
        {
            get { return "PowerControll"; }
        }

        public string DisplayPluginName
        {
            get { return "PowerControll"; }
        }

        public string PluginDescription
        {
            get { return "Позволяет выключать/переводить в спящий режим компьютер после завершения Wasppacer."; }
        }

        public string Author
        {
            get { return "dredei"; }
        }

        public Version Version
        {
            get { return new Version( "1.0.1" ); }
        }

        #endregion

        #endregion
    }
}