using Chroma.PP5.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace Chroma.FuelCell.GatewayConnector
{

    public static class LanguageResources
    {
        const string ResourcePattern = "pack://application:,,,/Chroma.FuelCell.GatewayConnector;component/ControlResources.xaml";
        const string LanguagePattern = "pack://application:,,,/Chroma.FuelCell.GatewayConnector;component/Resources/{0}.xaml";
        const string ThemePattern = "pack://application:,,,/Chroma.FuelCell.GatewayConnector;component/Themes/{0}.xaml";
        private static readonly ResourceDictionary sharedResource;

        static LanguageResources()
        {
            sharedResource = new ResourceDictionary();

            //sharedResource.Source = new Uri("pack://application:,,,/Chroma.HardwareConfiguration;component/ControlResources_en-US.xaml");
        }

        private static string GetLanguageResource(string name)
        {
            string currentName = string.Empty;
            currentName = string.Format(LanguagePattern, name);

            return currentName;

        }
        private static void onMergedDictionaryChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
        {
            FrameworkElement el = source as FrameworkElement;
            if (el == null)
                return;
           

            Uri resourceLocator = new Uri(GetMergedDictionary(source), UriKind.Relative);
            sharedResource.Source = resourceLocator;



            bool designTime = System.ComponentModel.DesignerProperties.GetIsInDesignMode(source);
            if (!designTime)
            {
                string cultureName = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
                if (!string.IsNullOrEmpty(cultureName))
                {
                    string currentName = GetLanguageResource(cultureName);
                    Uri languageLocator = new Uri(currentName, UriKind.RelativeOrAbsolute);
                    sharedResource.MergedDictionaries[(int)ResourceIndex.Language].Source = languageLocator;
                }
            }


            //Uri resourceLocator = new Uri(GetMergedDictionary(source), UriKind.Relative);
            //sharedResource = (ResourceDictionary)Application.LoadComponent(resourceLocator);

            el.Resources.MergedDictionaries.Add(sharedResource);
        }

        public static readonly DependencyProperty MergedDictionaryProperty =
            DependencyProperty.RegisterAttached("MergedDictionary", typeof(String), typeof(LanguageResources), new FrameworkPropertyMetadata(null, onMergedDictionaryChanged));

        [AttachedPropertyBrowsableForType(typeof(FrameworkElement))]
        public static String GetMergedDictionary(DependencyObject source)
        {
            return (String)source.GetValue(MergedDictionaryProperty);
        }

        public static void SetMergedDictionary(DependencyObject source, String value)
        {
            source.SetValue(MergedDictionaryProperty, value);
        }

        public static string GetValue(string key)
        {
            return sharedResource[key] as string;
        }


        public static void SetCulture(string resource)
        {
            string currentName = GetLanguageResource(resource);
            sharedResource.MergedDictionaries[(int)ResourceIndex.Language].Source = new Uri(currentName);

        }
    }

}
