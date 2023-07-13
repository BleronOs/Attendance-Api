namespace AttendanceTracker.Api.Configuration;

public class TokenConfiguration
{
    public string Secret { get; set; }

    private int? _expirationInDays = 6;

    /// <summary>
    /// This value will be retrieved from the appsettings.json, however if there's no data the value will be set to 6
    /// </summary>
    public int? ExpirationInDays
    {
        get { return _expirationInDays; }
        set
        {
            if (value != null)
            {
                _expirationInDays = value;
            }
        }
    }
}