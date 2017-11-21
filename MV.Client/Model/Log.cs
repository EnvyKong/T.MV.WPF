[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace MV.Client.Model
{
    public static class Log
    {
        public static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}
