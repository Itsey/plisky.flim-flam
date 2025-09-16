namespace Plisky.FlimFlam;  

internal interface IAppHelpAbstraction {

    bool ConsumeURL(string url);

    string GetAppBaseUrl();

    string GetAppUrl(string parameter);

    string GetRawResourceString(string identifier);
}