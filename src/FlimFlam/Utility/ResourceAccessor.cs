using System;
using System.Configuration;
using System.Resources;

namespace Plisky.FlimFlam { 

    internal class ResourceAccessor : IAppHelpAbstraction {

        #region IAppHelpAbstraction Members

        public bool ConsumeURL(string url) {
            throw new NotImplementedException();
        }

        public string GetAppBaseUrl() {
            string baseUrlFromConfig = ConfigurationManager.AppSettings["basehelpurl"];
            if (baseUrlFromConfig != null) {
                return baseUrlFromConfig;
            }
            throw new ConfigurationErrorsException("The application has not been configured correctly");
        }

        public string GetAppUrl(string parameter) {
            string baseUrlFromConfig = ConfigurationManager.AppSettings["basehelpurl_1p"];
            if (baseUrlFromConfig != null) {
                return baseUrlFromConfig;
            }
            throw new ConfigurationErrorsException("The application has not been configured correctly");
        }

        public string GetRawResourceString(string identifier) {
            ResourceManager rm = new ResourceManager("OldFlimFlam.Resources.MexResources", this.GetType().Assembly);
            return rm.GetString(identifier);
        }

        #endregion IAppHelpAbstraction Members
    }
}