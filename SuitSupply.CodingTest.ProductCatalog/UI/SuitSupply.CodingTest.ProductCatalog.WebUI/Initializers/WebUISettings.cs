using log4net;
using SuitSupply.CodingTest.ProductCatalog.WebUI.Properties;
using System;
using System.Collections.Specialized;
using System.Configuration;

namespace SuitSupply.CodingTest.ProductCatalog.WebApi.Initializers
{
    public class WebUISettings
    {
        #region Fields
        private static readonly ILog _logger = LogManager.GetLogger(Resources.Log4NetName);
        private string _productNumberInHome = "";
        private string _productWebApiUrl = "";
        private string _log4NetName = "";
        private static WebUISettings _instance = null;
        private static readonly object _lockObj = new object();
        #endregion

        #region Properties
        protected static ILog Logger { get { return _logger; } }
        public string ProductNumberInHome { get { return _productNumberInHome; } }
        public string ProductWebApiUrl { get { return _productWebApiUrl; } }
        public string Log4NetName { get { return _log4NetName; } }
        /// <summary>
        /// Initialize singleton object to read config file
        /// </summary>
        public static WebUISettings Instance
        {
            get
            {
                lock (_lockObj)
                {
                    if (_instance == null)
                        _instance = new WebUISettings();
                    return _instance;
                }
            }
        }
        #endregion

        #region Constructors
        private WebUISettings()
        {
            LoadSettings();
        }
        #endregion

        private void LoadSettings()
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            SetSettingAs<string>(appSettings, Resources.ProductsInHome, ref _productNumberInHome);
            SetSettingAs<string>(appSettings, Resources.ProductWebApiUrl, ref _productWebApiUrl);
            SetSettingAs<string>(appSettings, Resources.Log4NetName, ref _log4NetName);
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