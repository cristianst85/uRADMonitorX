namespace uRADMonitorX.Updater
{
    public interface IWebUpdater {

        IUpdateInfo Check(string updateUrl);

        byte[] Download(string downloadUrl);
    }
}
