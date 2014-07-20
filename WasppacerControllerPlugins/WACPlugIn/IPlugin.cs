#region Using

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#endregion

namespace WACPlugIn
{
    public interface IPlugin
    {
        string PluginName { get; } // имя плагина
        string DisplayPluginName { get; } // имя плагина, которое отображается
        string PluginDescription { get; } // описание плагина
        string Author { get; } // имя автора плагина
        Version Version { get; } // версия

        /// <summary>
        /// Вызывается при активации плагина
        /// </summary>
        /// <param name="paramsArr">Массив параметров</param>
        WaspEnvent[] Activate( params object[] paramsArr );

        /// <summary>
        /// Вызывается во время выполнения события
        /// </summary>
        /// <param name="eventName">Имя события</param>
        /// <param name="paramsArr">Список параметров</param>
        /// <returns></returns>
        Task<object> Event( WaspEnvent eventName, params object[] paramsArr );

        /// <summary>
        /// Вызывается, когда пользователь нажал на кнопку настроек плагина
        /// </summary>
        void ShowSettingsForm();
    }

    public enum WaspEnvent
    {
        BeforeStartAllWasppacer,
        AfterStartAllWasppacer,
        BeforeStartWasppacer,
        AfterStartWasppacer,
        BeforeStopWasppacer,
        AfterStopWasppacer,
        BeforeReinstallWasppacer,
        AfterReinstallWasppacer,
        BeforeStartProgram,
        CreateMenuItem,
        MenuItemClicked,
        TimerTick
    }
}