using AdPlatformLocations.Lib;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AdPlatformLocations.WebApi.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AdPlatformsController : ControllerBase
{
    private readonly AdPlatformsLocationsRepository _adPlatformsLocationsRepository;
    private readonly ValidatorNormalizer _validatorNormalizer;
    
    public AdPlatformsController(AdPlatformsLocationsRepository adPlatformsLocationsRepository, ValidatorNormalizer validatorNormalizer)
    {
        _adPlatformsLocationsRepository = adPlatformsLocationsRepository;
        _validatorNormalizer = validatorNormalizer;
    }

    [ProducesResponseType<string>(StatusCodes.Status200OK)]
    [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
    [HttpGet]
    public Results<Ok<IEnumerable<string>>, BadRequest<string>> Get(string location)
    {
        var normalized = _validatorNormalizer.ValidateAndNormalizeLocationPath(location);
        if (normalized == null)
        {
            return TypedResults.BadRequest("invalid location");
        }

        var platformNames = _adPlatformsLocationsRepository.GetPlatformNames(location);
        return TypedResults.Ok(platformNames);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
    [HttpPost]
    public Results<Ok, BadRequest<string>> ReInit(IFormFile file,
        [FromServices] AdPlatformsAndLocationsFromStreamParser adPlatformsAndLocationsFromStreamParser)
    {
        try
        {
            using var stream = file.OpenReadStream();
            using var streamReader = new StreamReader(stream);
            var contents = adPlatformsAndLocationsFromStreamParser.ParseEnumerable(streamReader);
            _adPlatformsLocationsRepository.ReInit(contents);
            return TypedResults.Ok();
        }
        catch (InvalidFileContentException ex)
        {
            return TypedResults.BadRequest("Invalid file content");
        }
    }
}