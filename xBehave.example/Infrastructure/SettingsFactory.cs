
namespace xBehave.example.Infrastructure
{
    using System.Collections;
    using Castle.Components.DictionaryAdapter;
    using System.Configuration;

    public class SettingsFactory
    {
        public static ISettings Create()
        {
            var dictionary = new Hashtable();
            var factory = new DictionaryAdapterFactory();
            return factory.GetAdapter<ISettings>(ConfigurationManager.AppSettings);
        }
    }
}
