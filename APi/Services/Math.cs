namespace Tourplanner.Services;

public static class MathUtils
{
    public static float MapRange(float value, float inMin, float inMax, float outMin, float outMax)
    {
        value = Math.Max(inMin, Math.Min(inMax, value));
        float normalizedValue = (value - inMin) / (inMax - inMin);
    
        return outMin + normalizedValue * (outMax - outMin);
    }
}