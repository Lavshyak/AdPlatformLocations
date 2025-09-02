namespace AdPlatformLocations.Lib;

public class AdPlatformsAndLocationsFromStreamParser
{
    private readonly ValidatorNormalizer _validatorNormalizer;

    public AdPlatformsAndLocationsFromStreamParser(ValidatorNormalizer validatorNormalizer)
    {
        _validatorNormalizer = validatorNormalizer;
    }

    // Использует перечисление и yeld return, так было проще, да и по памяти оптимизированнее
    public IEnumerable<PlatformNameAndLocationPaths> ParseEnumerable(TextReader textReader)
    {
        while (true) // пришлось
        {
            var line = textReader.ReadLine();
            if (line == null)
                break;

            line = line.Trim();
            if (line.Length == 0)
            {
                continue;
            }

            var colonFirstIndex = line.IndexOf(':');
            var colonLastIndex = line.LastIndexOf(':');

            if (colonFirstIndex != colonLastIndex) // 2+ ':'
            {
                throw new InvalidFileContentException();
            }

            if (colonFirstIndex == 0)
            {
                throw new InvalidFileContentException(); // нет названия платформы
            }

            if (colonLastIndex == line.Length - 1) // нет локаций
            {
                continue;
            }

            var splittedPlatformNameAndLocationPaths = line.Split(":");

            if (splittedPlatformNameAndLocationPaths.Length != 2) // на всякий случай
            {
                throw new InvalidFileContentException();
            }

            var platformNameSource = splittedPlatformNameAndLocationPaths[0];
            var platformName = _validatorNormalizer.ValidateAndNormalizePlatformName(platformNameSource);
            if (platformName == null)
            {
                throw new InvalidFileContentException();
            }

            var locationPaths = splittedPlatformNameAndLocationPaths[1].Split(',').Select(locationPathSource =>
            {
                var normalized = _validatorNormalizer.ValidateAndNormalizeLocationPath(locationPathSource);
                if (normalized == null)
                    throw new InvalidFileContentException();
                return normalized;
            }).ToArray(); // проще тестировать, когда здесь ToArray
            
            var result = new PlatformNameAndLocationPaths(platformName, locationPaths);
            yield return result;
        }
    }
}