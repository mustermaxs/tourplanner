namespace Client.Utils;

public static class StaticResService
{
    private static String IconBasePath = "Icons/";
    public static String GetIconPath(String IconName) => $"{IconBasePath}{Images.ResourceManager.GetString(IconName)}";
    public static String GetColor(String ColorKey) => Images.ResourceManager.GetString(ColorKey);
    public static String GetApiBaseUrl => Images.ResourceManager.GetString("api_base_url");
}