namespace AdPlatformLocations.Lib.Tests;

public class AdPlatformsLocationsRepositoryTests
{
    private readonly AdPlatformsLocationsRepository _repository = new();

    [Fact]
    public void Works1()
    {
        Assert.Equal([], _repository.GetPlatformNames("/aaa1"));
    }

    [Fact]
    public void Works2()
    {
        _repository.ReInit([new PlatformNameAndLocationPaths("A", ["/aaa2"])]);
        Assert.Equal([], _repository.GetPlatformNames("/aaa1"));
        Assert.Equal(["A"], _repository.GetPlatformNames("/aaa2"));
    }

    [Fact]
    public void WorksEmptyReInit()
    {
        _repository.ReInit([]);
        Assert.Equal([], _repository.GetPlatformNames("/aaa1"));
    }

    [Fact]
    public void WorksReInitMultipleTimes()
    {
        _repository.ReInit([new PlatformNameAndLocationPaths("A", ["/aaa2"])]);
        Assert.Equal([], _repository.GetPlatformNames("/aaa1"));
        Assert.Equal(["A"], _repository.GetPlatformNames("/aaa2"));

        _repository.ReInit([new PlatformNameAndLocationPaths("A", ["/aaa2"])]);
        Assert.Equal([], _repository.GetPlatformNames("/aaa1"));
        Assert.Equal(["A"], _repository.GetPlatformNames("/aaa2"));

        _repository.ReInit([]);
        Assert.Equal([], _repository.GetPlatformNames("/aaa1"));

        _repository.ReInit([new PlatformNameAndLocationPaths("A", ["/aaa2"])]);
        Assert.Equal([], _repository.GetPlatformNames("/aaa1"));
        Assert.Equal(["A"], _repository.GetPlatformNames("/aaa2"));
    }

    [Fact]
    public void Works3()
    {
        _repository.ReInit([
            new PlatformNameAndLocationPaths("A", ["/aaa2"]),
            new PlatformNameAndLocationPaths("B", ["/aaa2/bbb1"])
        ]);
        Assert.Equal([], _repository.GetPlatformNames("/aaa1"));
        Assert.Equal(["A"], _repository.GetPlatformNames("/aaa2"));
        Assert.Equal(["A", "B"], _repository.GetPlatformNames("/aaa2/bbb1").Order());
    }

    [Fact]
    public void Works4()
    {
        _repository.ReInit([
            new PlatformNameAndLocationPaths("A", ["/aaa2"]),
            new PlatformNameAndLocationPaths("A", ["/aaa2"]),
            new PlatformNameAndLocationPaths("B", ["/aaa2/bbb1"]),
            new PlatformNameAndLocationPaths("B", ["/aaa2/bbb1"])
        ]);
        Assert.Equal([], _repository.GetPlatformNames("/aaa1"));
        Assert.Equal(["A"], _repository.GetPlatformNames("/aaa2"));
        Assert.Equal(["A", "B"], _repository.GetPlatformNames("/aaa2/bbb1").Order());
    }

    [Fact]
    public void Works5()
    {
        _repository.ReInit([
            new PlatformNameAndLocationPaths("A", ["/aaa2"]),
            new PlatformNameAndLocationPaths("A1", ["/aaa2"]),
            new PlatformNameAndLocationPaths("B", ["/aaa2/bbb1"]),
        ]);
        Assert.Equal([], _repository.GetPlatformNames("/aaa1"));
        Assert.Equal(["A", "A1"], _repository.GetPlatformNames("/aaa2"));
        Assert.Equal(["A", "A1", "B"], _repository.GetPlatformNames("/aaa2/bbb1").Order());
    }

    [Fact]
    public void Works6()
    {
        _repository.ReInit([
            new PlatformNameAndLocationPaths("Яндекс.Директ", ["/ru"]),
            new PlatformNameAndLocationPaths("Ревдинский рабочий", ["/ru/svrd/revda", "/ru/svrd/pervik"]),
            new PlatformNameAndLocationPaths("Газета уральских москвичей", ["/ru/msk", "/ru/permobl", "/ru/chelobl"]),
            new PlatformNameAndLocationPaths("Крутая реклама", ["/ru/svrd"]),
        ]);
        Assert.Equal([], _repository.GetPlatformNames("/aaa1"));
        Assert.Equal(((string[]) ["Яндекс.Директ"]).Order(), _repository.GetPlatformNames("/ru").Order());
        Assert.Equal(((string[]) ["Ревдинский рабочий", "Яндекс.Директ", "Крутая реклама"]).Order(),
            _repository.GetPlatformNames("/ru/svrd/revda").Order());
        Assert.Equal(((string[]) ["Яндекс.Директ", "Крутая реклама"]).Order(),
            _repository.GetPlatformNames("/ru/svrd").Order());
    }
}