namespace AdPlatformLocations.Lib.Tests;

public class ValidatorNormalizerFixture
{
    public ValidatorNormalizer ValidatorNormalizer { get; } = new();
}

public class ValidatorNormalizerTests : IClassFixture<ValidatorNormalizerFixture>
{
    private readonly ValidatorNormalizer _validatorNormalizer;

    public ValidatorNormalizerTests(ValidatorNormalizerFixture fixture)
    {
        _validatorNormalizer = fixture.ValidatorNormalizer;
    }

    [InlineData(null, null)]
    [InlineData("", null)]
    [InlineData(" ", null)]
    [InlineData("\n", null)]
    [InlineData("/\n", null)]
    [InlineData("a", null)]
    [InlineData("a/", null)]
    [InlineData("/a/", "/a")]
    [InlineData("/a", "/a")]
    [InlineData(" /a ", "/a")]
    [InlineData("\n/a\r", "/a")]
    [InlineData("\n/abc\r", "/abc")]
    [InlineData("\n/abc\r/", "/abc")]
    [InlineData("\n/abc\r /", "/abc")]
    [InlineData(" \n/abc\r /", "/abc")]
    [Theory]
    public void WorksForLocationPath(string? input, string? expected)
    {
        var result = _validatorNormalizer.ValidateAndNormalizeLocationPath(input);
        Assert.Equal(expected, result);
    }

    [InlineData(null, null)]
    [InlineData("", null)]
    [InlineData(" ", null)]
    [InlineData("\n", null)]
    [InlineData("a", "a")]
    [InlineData(" a", "a")]
    [InlineData("a ", "a")]
    [InlineData(" a ", "a")]
    [InlineData(" abc ", "abc")]
    [Theory]
    public void WorksForPlatformName(string? input, string? expected)
    {
        var result = _validatorNormalizer.ValidateAndNormalizePlatformName(input);
        Assert.Equal(expected, result);
    }
}