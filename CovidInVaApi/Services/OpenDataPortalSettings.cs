namespace CovidInVaApi.Services;

public class OpenDataPortalSettings
{
    public OpenDataPortalSettings()
    {
        AppToken = string.Empty;
        Url = string.Empty;
    }

    public string AppToken { get; set; }

    public string Url { get; set; }
}
