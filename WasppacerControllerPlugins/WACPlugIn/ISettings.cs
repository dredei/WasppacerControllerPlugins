namespace WACPlugIn
{
    public interface ISettings
    {
        object this[ IPlugin plugin, string paramName ] { get; set; }
    }
}