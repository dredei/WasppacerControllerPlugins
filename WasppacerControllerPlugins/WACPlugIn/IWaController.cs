#region Using

using System.Threading.Tasks;

#endregion

namespace WACPlugIn
{
    public interface IWaController
    {
        /// <summary>
        /// Скачивает архив во временную папку с RGHost
        /// </summary>
        /// <param name="rgHostLink">Ссылка на RGHost</param>
        /// <returns>Возвращает путь к сохраненному файлу</returns>
        Task<string> DownloadRgHostFileAsync( string rgHostLink );

        /// <summary>
        /// Ожидание закрытия WAAgent
        /// </summary>
        Task ExpectCloseAgentsAsync();

        /// <summary>
        /// Распаковывает ZIP архив
        /// </summary>
        /// <param name="fileName">Путь к архиву</param>
        /// <returns></returns>
        Task<bool> ExtractArchiveAsync( string fileName );

        /// <summary>
        /// Получение информации
        /// </summary>
        WaspInfo GetInfo();

        /// <summary>
        /// Получает ссылку на тему с последней версией ПО
        /// </summary>
        /// <returns></returns>
        Task<string> GetLastVersionLinkAsync( bool release = true );

        /// <summary>
        /// Получает ссылку на RGHost
        /// </summary>
        /// <param name="threadLink">Ссылка на тему</param>
        /// <returns></returns>
        Task<string> GetRgHostLinkAsync( string threadLink );

        /// <summary>
        /// Переустанавливает Wasppacer
        /// </summary>
        /// <returns>Возвращает значение указывающее на успешность переустановки</returns>
        Task<bool> ReinstallWasppacerAsync( bool release = true );

        /// <summary>
        /// Перезапускает Wasppacer
        /// </summary>
        Task RestartWasppacerAsync();

        /// <summary>
        /// Запускает Wasppacer (если в настройках указан запуск в песочнице, то произойдет запуск в песочнице)
        /// </summary>
        Task StartWasppacerAsync();

        /// <summary>
        /// Закрывает Wasppacer
        /// </summary>
        /// <param name="expectCloseAgent">Ожидать завершения всех WAAgent</param>
        Task StopWasppacerAsync( bool expectCloseAgent = false );
    }
}