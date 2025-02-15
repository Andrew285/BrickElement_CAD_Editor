using System.Globalization;
using System.Reflection;
using System.Resources;

namespace Core.Services
{
    public class LanguageService
    {
        private static LanguageService instance;
        private ResourceManager _resourceManager;

        public event EventHandler LanguageChanged;
        public Language Language { get; private set; }

        public static LanguageService GetInstance()
        {
            if (instance == null)
            {
                instance = new LanguageService();
            }
            
            return instance;
        }

        private LanguageService()
        {
            _resourceManager = new ResourceManager("Core.Resources.strings", Assembly.GetExecutingAssembly());
        }

        public string GetString(string name)
        {
            string value = _resourceManager.GetString(name);
            return value ?? $"[{name}]"; // Handle missing keys
        }

        public void ChangeLanguage(string culturalCode)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culturalCode);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(culturalCode);

            // Reload ResourceManager
            _resourceManager = new ResourceManager("Core.Resources.strings", Assembly.GetExecutingAssembly());

            // Notify UI or other services that language changed
            Console.WriteLine($"Changed Language to: {culturalCode}");
            LanguageChanged?.Invoke(this, EventArgs.Empty);
        }

        private static readonly Dictionary<Language, string> languageCodes = new()
        {
            { Language.ENGLISH, "en-US" },
            { Language.UKRAINIAN, "uk-UA" }
        };

        public void ChangeLanguage(Language language)
        {
            if (languageCodes.TryGetValue(language, out string culturalCode))
            {
                Language = language;
                ChangeLanguage(culturalCode);
            }
        }
    }

    public enum Language
    {
        ENGLISH,
        UKRAINIAN
    }
}
