
namespace BetterEducationToolbar
{
    internal static class LogHelper
    {
        private const string Prefix = "Better Education Toolbar: ";

        public static void Information(string message, params object[] args)
        {
            var msg = Prefix + string.Format(message, args);
            UnityEngine.Debug.Log(msg);
        }

        public static void Warning(string message, params object[] args)
        {
            var msg = Prefix + string.Format(message, args);
            UnityEngine.Debug.LogWarning(msg);
        }

        public static void Error(string message, params object[] args)
        {
            var msg = Prefix + string.Format(message, args);
            UnityEngine.Debug.LogError(msg);
        }
    }
}
