#region Using

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using WACPlugIn;

#endregion

namespace ClosingUnownedWasub
{
    public class PlugIn : IPlugin
    {
        private Timer _timer;

        private void Check()
        {
            var wasubs = Process.GetProcessesByName( "wasub" );
            foreach ( Process process in wasubs )
            {
                Process parent = process.Parent();
                if ( parent != Process.GetCurrentProcess() && parent == null )
                {
                    try
                    {
                        process.Kill();
                    }
                    catch ( Exception )
                    {
                        continue;
                    }
                }
            }
        }

        private void InitTimer()
        {
            if ( this._timer == null )
            {
                this._timer = new Timer { Interval = 30000 };
                this._timer.Elapsed += this._timer_Tick;
                this._timer.Start();
            }
        }

        private void _timer_Tick( object sender, EventArgs e )
        {
            this.Check();
        }

        #region Members

        public WaspEnvent[] Activate( params object[] paramsArr )
        {
            this.InitTimer();
            return null;
        }

        public async Task<object> Event( WaspEnvent eventName, params object[] paramsArr )
        {
            await TaskEx.Run( () => this.InitTimer() );
            return null;
        }

        public void ShowSettingsForm()
        {
        }

        #region InfoFields

        public string PluginName
        {
            get { return "ClosingUnownedWasub"; }
        }

        public string DisplayPluginName
        {
            get { return "ClosingUnownedWasub"; }
        }

        public string PluginDescription
        {
            get { return "Закрывает \"бесхозные\" процессы wasub.exe"; }
        }

        public string Author
        {
            get { return "dredei"; }
        }

        public Version Version
        {
            get { return new Version( 1, 0, 1 ); }
        }

        #endregion

        #endregion
    }

    public static class ProcessExtensions
    {
        private static string FindIndexedProcessName( int pid )
        {
            try
            {
                var processName = Process.GetProcessById( pid ).ProcessName;
                var processesByName = Process.GetProcessesByName( processName );
                string processIndexdName = null;

                for ( var index = 0; index < processesByName.Length; index++ )
                {
                    processIndexdName = index == 0 ? processName : processName + "#" + index;
                    var processId = new PerformanceCounter( "Process", "ID Process", processIndexdName );
                    if ( (int)processId.NextValue() == pid )
                    {
                        return processIndexdName;
                    }
                }

                return processIndexdName;
            }
            catch ( Exception )
            {
                return null;
            }
        }

        private static Process FindPidFromIndexedProcessName( string indexedProcessName )
        {
            var parentId = new PerformanceCounter( "Process", "Creating Process ID", indexedProcessName );
            try
            {
                return Process.GetProcessById( (int)parentId.NextValue() );
            }
            catch ( Exception )
            {
                return null;
            }
        }

        public static Process Parent( this Process process )
        {
            string indexedProcessName = FindIndexedProcessName( process.Id );
            if ( string.IsNullOrEmpty( indexedProcessName ) )
            {
                return Process.GetCurrentProcess();
            }
            return FindPidFromIndexedProcessName( indexedProcessName );
        }
    }
}