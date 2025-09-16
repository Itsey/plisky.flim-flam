using System.Xml.Linq;

namespace Plisky.FlimFlam; 

internal class AppHelpProvider {
    private readonly IAppHelpAbstraction resources;

    internal AppHelpProvider(IAppHelpAbstraction ira) {
        //Bilge.Assert(ira != null, "The resource access can not be null");
        resources = ira;
    }

    protected string LastUsedIdentifier { get; set; }

    protected string LastUsedRawValue { get; set; }
    protected XDocument LoadedRawValue { get; set; }

    internal static string CreateHelpContent(string helpContent) {
        var xd = CreateHelp(false, helpContent, false, null);
        return xd.ToString();
    }

    internal static string CreateHelpContent(string helpDescription, string absoluteUrl) {
        var xd = CreateHelp(false, helpDescription, true, absoluteUrl);
        return xd.ToString();
    }

    internal static string CreateHelpContentWithContext(string helpDescription, string knownIdentifier) {
        var xd = CreateHelp(false, helpDescription, false, knownIdentifier);
        return xd.ToString();
    }

    internal static string CreateHelpReference(string knownIdentifier) {
        var xd = CreateHelp(true, knownIdentifier, false, null);
        return xd.ToString();
    }

    internal string GetHelpDescription(string identifier) {
        UpdateLastUsedForIdentifier(identifier);

        return RawToHelpDescription();
    }

    internal string GetHelpUrl(string identifier) {
        //Bilge.Assert(!string.IsNullOrEmpty(identifier), "The identifier cant be empty");
        //Bilge.Assert(resources != null, "the resources are not set up yet");

        UpdateLastUsedForIdentifier(identifier);

        return RawToHelpUrl();
    }

    private static XDocument CreateHelp(bool reference, string helpContent, bool urlIsAbsolute, string urlText) {
        var result = new XDocument();
        result.Add(new XElement("hlp"));

        if (reference) {
            result.Element("hlp").Add(new XAttribute("idr", helpContent));
        } else {
            if (helpContent != null) {
                result.Element("hlp").Add(new XElement("txt", helpContent));
            }

            result.Element("hlp").Add(new XElement("lnk"));

            if (!string.IsNullOrEmpty(urlText)) {
                if (urlIsAbsolute) {
                    result.Element("hlp").Element("lnk").Add(new XElement("abs", urlText));
                } else {
                    result.Element("hlp").Element("lnk").Add(new XElement("abs"), new XElement("ctx", urlText));
                }
            }
        }
        return result;
    }

    private void LoadRawData(string rawData) {
        if (!string.IsNullOrEmpty(rawData)) {
            if (rawData.StartsWith("<hlp")) {
                var xd = XDocument.Parse(rawData);
                LastUsedRawValue = rawData;
                LoadedRawValue = xd;

                // Its possible to redirect one help element to another, this code will load the replacement redirect
                // if this is the case.  It is recursive but it will replace the values that we have just set.
                if (xd.Element("hlp").Attribute("idr") != null) {
                    string redirectContent = resources.GetRawResourceString(xd.Element("hlp").Attribute("idr").Value);
                    if (redirectContent != null) {
                        LoadRawData(redirectContent);
                    }
                }
            } else {
                LastUsedRawValue = rawData;
                LoadedRawValue = null;
            }
        } else {
            LastUsedRawValue = null;
            LoadedRawValue = null;
        }
    }

    private string RawToHelpDescription() {
        if (LastUsedRawValue == null) { return null; }
        string result = null;

        if (LoadedRawValue == null) {
            result = LastUsedRawValue;
        } else {
            if (LoadedRawValue.Element("hlp").Element("txt") != null) {
                result = LoadedRawValue.Element("hlp").Element("txt").Value;
            }
        }

        return result;
    }

    /// <summary>
    /// The URL which is returned can be one of three different values.  If the Xml has an ABS tag in it then this is an absolute URL.  If
    /// it has a Link tag but no ctx tag then its a link to the default site.  If it has a ctx tag then this is a context parameter to pass
    /// to the url and the base url is sent with a context parameter.
    /// </summary>
    /// <returns>A URL for the help reference</returns>
    private string RawToHelpUrl() {
        if ((LoadedRawValue == null) || (LastUsedRawValue == null)) { return null; }

        var linkElement = LoadedRawValue.Element("hlp").Element("lnk");
        if ((linkElement.Element("abs") != null) && (linkElement.Element("abs").Value.Length > 0)) {
            return LoadedRawValue.Element("hlp").Element("lnk").Element("abs").Value;
        } else {
            if ((linkElement.Element("ctx") != null) && linkElement.Element("ctx").Value.Length > 0) {
                return resources.GetAppUrl(linkElement.Element("ctx").Value);
            }
        }
        return resources.GetAppBaseUrl();
    }

    private void UpdateLastUsedForIdentifier(string identifier) {
        if (identifier != LastUsedIdentifier) {
            //b.Verbose.Log("Not a cached value, replacing the selected resource string");
            string workingRaw = resources.GetRawResourceString(identifier);
            LoadRawData(workingRaw);
        }
    }
}