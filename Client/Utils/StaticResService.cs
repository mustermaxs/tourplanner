namespace Client.Utils;

public static class StaticResService
{
    private static String IconBasePath = "Icons/";
    public static String GetIconPath(String IconName) => $"{IconBasePath}{Images.ResourceManager.GetString(IconName)}";
}