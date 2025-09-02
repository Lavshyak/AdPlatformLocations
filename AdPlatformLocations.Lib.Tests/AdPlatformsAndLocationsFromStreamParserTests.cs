namespace AdPlatformLocations.Lib.Tests;

public class AdPlatformsAndLocationsFromStreamParserTestsFixture
{
    public AdPlatformsAndLocationsFromStreamParser Parser { get; }

    public AdPlatformsAndLocationsFromStreamParserTestsFixture()
    {
        Parser = new AdPlatformsAndLocationsFromStreamParser(new ValidatorNormalizer());
    }
}

public class
    AdPlatformsAndLocationsFromStreamParserTests : IClassFixture<AdPlatformsAndLocationsFromStreamParserTestsFixture>
{
    private readonly AdPlatformsAndLocationsFromStreamParser _parser;

    public AdPlatformsAndLocationsFromStreamParserTests(AdPlatformsAndLocationsFromStreamParserTestsFixture fixture)
    {
        _parser = fixture.Parser;
    }

    [Fact]
    public void WorksOnNormalFile()
    {
        var content = @"
            Яндекс.Директ:/ru
            Ревдинский рабочий:/ru/svrd/revda,/ru/svrd/pervik
            Газета уральских москвичей:/ru/msk,/ru/permobl,/ru/chelobl
            
            Крутая реклама:/ru/svrd
                        ";
        using var stream = new StringReader(content);
        var actual = _parser.ParseEnumerable(stream).ToArray();

        Assert.Collection(actual,
            item =>
            {
                Assert.Equal("Яндекс.Директ", item.PlatformName);
                Assert.Equal(["/ru"], item.Paths);
            },
            item =>
            {
                Assert.Equal("Ревдинский рабочий", item.PlatformName);
                Assert.Equal(["/ru/svrd/revda", "/ru/svrd/pervik"], item.Paths);
            },
            item =>
            {
                Assert.Equal("Газета уральских москвичей", item.PlatformName);
                Assert.Equal(["/ru/msk", "/ru/permobl", "/ru/chelobl"], item.Paths);
            },
            item =>
            {
                Assert.Equal("Крутая реклама", item.PlatformName);
                Assert.Equal(["/ru/svrd"], item.Paths);
            }
        );
    }

    [Fact]
    public void ThrowsOnInvalidAdPlatformName()
    {
        var content = @"
            Яндекс.Директ:/ru
            :/ru/svrd/revda,/ru/svrd/pervik
                        ";
        using var stream = new StringReader(content);
        Assert.Throws<InvalidFileContentException>(() => _parser.ParseEnumerable(stream).ToArray());
    }
    
    [Fact]
    public void ThrowsOnInvalidLocations1()
    {
        var content = @"
            Яндекс.Директ:/ru
            Ревдинский рабочий:/ru/svrd/revda,
                        ";
        using var stream = new StringReader(content);
        Assert.Throws<InvalidFileContentException>(() => _parser.ParseEnumerable(stream).ToArray());
    }
    
    [Fact]
    public void ThrowsOnInvalidLocations2()
    {
        var content = @"
            Яндекс.Директ:/ru
            Ревдинский рабочий:/ru/svrd/revda, ,
                        ";
        using var stream = new StringReader(content);
        Assert.Throws<InvalidFileContentException>(() => _parser.ParseEnumerable(stream).ToArray());
    }
    
    [Fact]
    public void ThrowsOnInvalidLocations3()
    {
        var content = @"
            Яндекс.Директ:/ru
            Ревдинский рабочий:ru/svrd/revda
                        ";
        using var stream = new StringReader(content);
        Assert.Throws<InvalidFileContentException>(() => _parser.ParseEnumerable(stream).ToArray());
    }
    
    [Fact]
    public void EmptyOnEmptyFile()
    {
        var content = @"
            
                        ";
        using var stream = new StringReader(content);
        var actual = _parser.ParseEnumerable(stream).ToArray();
        Assert.Empty(actual);
    }
    
    [Fact]
    public void ThrowsOnRubbishFile()
    {
        var content = @"
            sthjk
                        ";
        using var stream = new StringReader(content);
        Assert.Throws<InvalidFileContentException>(() => _parser.ParseEnumerable(stream).ToArray());
    }
    
    [Fact]
    public void SkipsEmptyLocations()
    {
        var content = @"
            Яндекс.Директ:/ru
            Ревдинский рабочий:
            Газета уральских москвичей:/ru/msk,/ru/permobl,/ru/chelobl
            Крутая реклама:/ru/svrd
                        ";
        using var stream = new StringReader(content);
        var actual = _parser.ParseEnumerable(stream).ToArray();

        Assert.Collection(actual,
            item =>
            {
                Assert.Equal("Яндекс.Директ", item.PlatformName);
                Assert.Equal(["/ru"], item.Paths);
            },
            item =>
            {
                Assert.Equal("Газета уральских москвичей", item.PlatformName);
                Assert.Equal(["/ru/msk", "/ru/permobl", "/ru/chelobl"], item.Paths);
            },
            item =>
            {
                Assert.Equal("Крутая реклама", item.PlatformName);
                Assert.Equal(["/ru/svrd"], item.Paths);
            }
        );
    }
}