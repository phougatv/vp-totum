namespace VP.Totum.Url.Models;

public sealed class ShortenedUrl
{
	public Int32 Id { get; set; }
	public String ShortCode { get; set; } = String.Empty;
	public String OriginalUrl { get; set; } = String.Empty;
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	public DateTime? ExpirationDate { get; set; }
	public Boolean IsActive { get; set; } = true;
}
