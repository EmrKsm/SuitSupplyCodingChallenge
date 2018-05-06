using log4net;
using System;
using System.Collections.Specialized;
using System.Configuration;

namespace SuitSupply.CodingTest.ProductCatalog.WebApi.Initializers
{
    public class WebApiSettings
    {
        #region Fields
        private static readonly ILog _logger = LogManager.GetLogger(Environment.MachineName);
        private string _productsDbConnectionString = "";
        private static WebApiSettings _instance = null;
        private static readonly object _lockObj = new object();
        #endregion

        #region Properties
        protected static ILog Logger { get { return _logger; } }
        public string ProductsDbConnectionString { get { return _productsDbConnectionString; } }
        /// <summary>
        /// Initialize singleton object to read config file
        /// </summary>
        public static WebApiSettings Instance
        {
            get
            {
                lock (_lockObj)
                {
                    if (_instance == null)
                        _instance = new WebApiSettings();
                    return _instance;
                }
            }
        }
        #endregion

        #region Constructors
        private WebApiSettings()
        {
            LoadSettings();
        }
        #endregion

        private void LoadSettings()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            SetSettingAs<string>(appSettings, SettingsKey.ProductDbConnectionString, ref _productsDbConnectionString);
        }

        #region Helpers

        private void SetSettingAs<T>(object source, string key, ref T value)
        {
            string settingValue = String.Empty;
            try
            {
                if (source is NameValueCollection collection)
                {
                    settingValue = collection[key];
                }
                else
                {
                    Logger.WarnFormat("Unknown settings source: {0}", new string[] { key });
                }
            }
            catch (Exception ex)
            {
                Logger.Error("SetSettingAs", ex);
            }
            finally
            {
                if (String.IsNullOrEmpty(settingValue.Trim()))
                {
                    Logger.WarnFormat("Setting value does not assing for this key: {0}", new string[] { key });
                }
                else
                {
                    SetSettingValueAs<T>(settingValue, ref value);
                }
            }
        }

        private void SetSettingValueAs<T>(string settingValue, ref T value)
        {
            Type valueType = typeof(T);
            object parsedValue = null;

            if (valueType == typeof(Int32))
            {
                parsedValue = Int32.Parse(settingValue);
            }
            else if (valueType == typeof(Int64))
            {
                parsedValue = Int64.Parse(settingValue);
            }
            else if (valueType == typeof(Decimal))
            {
                parsedValue = Decimal.Parse(settingValue);
            }
            else if (valueType == typeof(Double))
            {
                parsedValue = Double.Parse(settingValue);
            }
            else if (valueType == typeof(Boolean))
            {
                parsedValue = Boolean.Parse(settingValue);
            }
            else if (valueType == typeof(TimeSpan))
            {
                parsedValue = TimeSpan.Parse(settingValue);
            }

            if (parsedValue == null)
            {
                value = (T)Convert.ChangeType(settingValue, valueType);
            }
            else
            {
                value = (T)Convert.ChangeType(parsedValue, valueType);
            }
        }

        #endregion
    }
}