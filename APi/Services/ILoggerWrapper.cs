namespace Tourplanner.Services.Logging
{
    using log4net;

    public interface ILoggerWrapper
    {
        void Debug(string msg);
        void Error(string msg);
        void Fatal(string msg);
        void Warn(string msg);
        void Info(string msg);
    }

    public static class LoggerFactory
    {
        public static ILoggerWrapper GetLogger()
        {
            return LoggerFactory.CreateLogger();
        }
        public static Log4NetWrapper CreateLogger(string configPath)
        {
            if (!File.Exists(configPath))
            {
                throw new ArgumentException("Config file for logger does not exist", nameof(configPath));
            }

            log4net.Config.XmlConfigurator.Configure(new FileInfo(configPath));
            var logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            return new Log4NetWrapper(logger);
        }
    }
}