using Pastel;

namespace App.Utils.ConsoleLogging
{
    public static class LogMe
    {
        private const string INFO = "INFO:";
        private static Color INFO_COLOR = Color.FromArgb(165, 229, 250);

        private const string WARNING = "WARNING:";
        private static Color WARNING_COLOR = Color.FromArgb(229, 217, 32);

        private const string ERROR = "ERROR:";
        private static Color ERROR_COLOR = Color.FromArgb(218, 50, 50);

        public static void Info(string message)
        {
            Output(INFO, INFO_COLOR, message);
        }

        public static void Warning(string message)
        {
            Output(WARNING, WARNING_COLOR, message);
        }

        public static void Error(string message)
        {
            Output(ERROR, ERROR_COLOR, message);
        }

        public static void Output(string lineHeader, Color lineHeaderColor, string message)
        {
            Console.WriteLine($"{lineHeader.Pastel(Color.Black).PastelBg(lineHeaderColor)} {message}");
        }
    }
}
