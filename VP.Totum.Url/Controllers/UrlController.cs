namespace VP.Totum.Url.Controllers;

[ApiController]
[Route("[controller]")]
public class UrlController(TotumDbContext dbContext) : ControllerBase
{
	private readonly TotumDbContext _dbContext = dbContext;

	[HttpGet("{code}")]
	public async Task <ActionResult> GetOriginalUrl(String code)
	{
		if (String.IsNullOrWhiteSpace(code))
			return BadRequest("Short code cannot be null or empty.");

		try
		{
			var shortCode = (ShortCode)code;
			var shortenedUrl = await GetByShortCodeAsync(shortCode);
			if (shortenedUrl == null)
				return NotFound("Url not available anymore");

			return Ok(shortenedUrl.OriginalUrl);
		}
		catch (ArgumentException ex)
		{
			return BadRequest(ex.Message);
		}
	}

	[HttpPost("shorten")]
	public async Task<ObjectResult> ShortenUrl([FromBody] String originalUrl)
	{
		if (String.IsNullOrWhiteSpace(originalUrl))
			return BadRequest("URL cannot be null or empty.");

		var shortCode = await GetUniqueCode();
		var shortenedUrl = new ShortenedUrl
		{
			ShortCode = shortCode,
			OriginalUrl = originalUrl,
			CreatedAt = DateTime.UtcNow,
			IsActive = true
		};
		await _dbContext.ShortenedUrls.AddAsync(shortenedUrl);
		await _dbContext.SaveChangesAsync();

		return StatusCode(StatusCodes.Status201Created, shortenedUrl.ShortCode);
	}

	#region Private methods
	private async Task<Boolean> IsUnique(ShortCode shortCode)
	{
		var shortCodeExists = await _dbContext.ShortenedUrls.AnyAsync(u => u.ShortCode == shortCode.Value);
		return !shortCodeExists;
	}

	private async Task<ShortenedUrl?> GetByShortCodeAsync(ShortCode shortCode)
	{
		var shortenedUrl = await _dbContext.ShortenedUrls
			.AsNoTracking()
			.FirstOrDefaultAsync(u => u.ShortCode == shortCode.Value && u.IsActive);
		return shortenedUrl;
	}

	private async Task<ShortCode> GetUniqueCode()
	{
		var shortCode = ShortCode.New();
		while(!await IsUnique(shortCode))
		{
			shortCode = ShortCode.New();
		}

		return shortCode;
	}
	#endregion Private methods
}
