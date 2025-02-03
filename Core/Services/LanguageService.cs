using System.Globalization;
using System.Reflection;
using System.Resources;

namespace Core.Services
{
    public class LanguageService
    {
        private static LanguageService instance;
        private ResourceManager _resourceManager;

        public static LanguageService GetInstance()
        {
            if (instance == null)
            {
                instance = new LanguageService();
            }

            return instance;
        }

        public LanguageService()
        {
            _resourceManager = new ResourceManager("Core.Resources.strings", Assembly.GetExecutingAssembly());
        }

        public string GetString(string name)
        {
            return _resourceManager.GetString(name);
        }

        public void ChangeLanguage(string culturalCode)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culturalCode);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(culturalCode);
        }
    }

}
