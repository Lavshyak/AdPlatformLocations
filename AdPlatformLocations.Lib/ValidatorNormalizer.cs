namespace AdPlatformLocations.Lib;

public class ValidatorNormalizer
{
    /// <returns>null - invalid; not null - normalized</returns>
    public string? ValidateAndNormalizeLocationPath(string? locationPath)
    {
        if (string.IsNullOrWhiteSpace(locationPath))
            return null;
        
        var normalized = locationPath.Trim().TrimEnd('/').TrimEnd();
        if (normalized.Length < 2)
            return null;
        if (!normalized.StartsWith('/'))
            return null;
        // TODO: проверка на разрешенные символы?
        return normalized;
    }

    /// <returns>null - invalid; not null - normalized</returns>
    public string? ValidateAndNormalizePlatformName(string? platformName)
    {
        if (string.IsNullOrWhiteSpace(platformName))
            return null;
        
        var normalized = platformName.Trim();
        // TODO: проверка на разрешенные символы?
        return normalized;
    }
}