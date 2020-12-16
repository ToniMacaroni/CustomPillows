using IPA.Logging;

namespace CustomPillows
{
    internal class CPLogger
    {
        private readonly Logger _logger;

        private CPLogger(Logger logger)
        {
            _logger = logger;
        }

        public void Log<T>(object obj) => _logger.Debug($"[{typeof(T).Name}] {obj}");

        public void LogInfo<T>(object obj) => _logger.Info($"[{typeof(T).Name}] {obj}");
    }
}
