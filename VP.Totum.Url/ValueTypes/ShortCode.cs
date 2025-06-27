namespace VP.Totum.Url.ValueTypes;

public readonly struct ShortCode : IEquatable<ShortCode>, IComparable<ShortCode>
{
	private const Int32 Length = 6;
	private static readonly Char[] Alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
	private static readonly HashSet<Char> AllowedChars = new HashSet<Char>(Alphabets);

	public String Value { get; }

	private ShortCode(String value)
	{
		Value = value;
	}

	public static ShortCode New() => InternalGenerate();

	public static ShortCode New(String value)
	{
		if (String.IsNullOrWhiteSpace(value) || value.Length != 6)
			throw new ArgumentException($"Short code should not be null/empty and must be exactly 6 characters long. Length of value is {value.Length}", nameof(value));
		if (value.Any(c => !AllowedChars.Contains(c)))
			throw new ArgumentException("Short code can only contain alphanumeric characters (A-Z, a-z, 0-9).", nameof(value));

		return new ShortCode(value);
	}

	public Boolean Equals(ShortCode other) => Value.Equals(other.Value, StringComparison.Ordinal);
	public Int32 CompareTo(ShortCode other) => String.Compare(Value, other.Value, StringComparison.Ordinal);
	public override String ToString() => Value;
	public override Boolean Equals(Object? obj) => obj is ShortCode other && Equals(other);
	public override Int32 GetHashCode() => Value.GetHashCode(StringComparison.Ordinal);

	public static implicit operator String(ShortCode shortCode) => shortCode.Value;
	public static implicit operator ShortCode(String value) => New(value);

	public static Boolean operator ==(ShortCode left, ShortCode right) => left.Equals(right);
	public static Boolean operator !=(ShortCode left, ShortCode right) => !left.Equals(right);

	public static Boolean operator <(ShortCode left, ShortCode right) => left.CompareTo(right) < 0;
	public static Boolean operator >(ShortCode left, ShortCode right) => left.CompareTo(right) > 0;

	public static Boolean operator <=(ShortCode left, ShortCode right) => left.CompareTo(right) < 1;
	public static Boolean operator >=(ShortCode left, ShortCode right) => left.CompareTo(right) > -1;

	#region Private methods
	private static ShortCode InternalGenerate()
	{
		var chars = new Char[Length];
		var bytes = new Byte[Length];
		using (var rng = RandomNumberGenerator.Create())
		{
			rng.GetBytes(bytes);
		}

		for (var i = 0; i < Length; i++)
		{
			// Map the byte to an index in the Alphabets array
			chars[i] = Alphabets[bytes[i] % Alphabets.Length];
		}

		return new String(chars);
	}
	#endregion Private methods
}
