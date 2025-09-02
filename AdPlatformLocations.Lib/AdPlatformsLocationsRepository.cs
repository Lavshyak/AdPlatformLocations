using System.Collections.Frozen;

namespace AdPlatformLocations.Lib;

public class AdPlatformsLocationsRepository
{
    /*
     * Будет синглтоном. синхронизации (про параллельность, многопоточность) не сделал специально, т.к. с текущими ТЗ и реализацией проблем быть не может.
     * Если что, то можно реализовать новый класс для Scoped, хранящий ссылку на экземпляр словаря, берущий ее отсюда.
     * Выбрал FrozenDictionary, т.к. время поиска примерно константное, городов в РФ около 1000,
     * про оптимизацию по памяти в ТЗ не указано, кол-во площадок неизвестно, но потребление памяти в целом должно быть адекватным.
     */

    /// <summary>
    /// location path -> platform names
    /// </summary>
    private FrozenDictionary<string, FrozenSet<string>> _root = FrozenDictionary<string, FrozenSet<string>>.Empty;

    public IEnumerable<string> GetPlatformNames(string locationPath)
    {
        if (_root.TryGetValue(locationPath, out var platformNames))
        {
            return platformNames;
        }
        else
        {
            return [];
        }
    }
    
    /// <summary>
    /// обновить
    /// </summary>
    /// <param name="fileLineContents"></param>
    public void ReInit(IEnumerable<PlatformNameAndLocationPaths> fileLineContents)
    {
        Dictionary<string, List<string>> pathToPlatformNamesDict = new();
        foreach (var fileLineContent in fileLineContents)
        {
            foreach (var path in fileLineContent.Paths)
            {
                if (!pathToPlatformNamesDict.TryGetValue(path, out var platformNames))
                {
                    platformNames = new List<string>();
                    pathToPlatformNamesDict.Add(path, platformNames);
                }

                platformNames.Add(fileLineContent.PlatformName);
            }
        }

        foreach (var kv in pathToPlatformNamesDict)
        {
            var pathComponents = kv.Key.Split('/', StringSplitOptions.RemoveEmptyEntries);
            // копирование названий платформ из глобальных путей в этот локальный путь
            for (int i = 1; i < pathComponents.Length; i++)
            {
                var key = "/"+string.Join("/", pathComponents.Take(i));
                if (pathToPlatformNamesDict.TryGetValue(key, out var platformNames))
                {
                    kv.Value.AddRange(platformNames);
                }
            }
        }

        // path, platform names
        FrozenDictionary<string, FrozenSet<string>> frozenDictionary = pathToPlatformNamesDict.Select(kv =>
            KeyValuePair.Create(kv.Key, kv.Value.ToFrozenSet())).ToFrozenDictionary();

        _root = frozenDictionary;
    }
}