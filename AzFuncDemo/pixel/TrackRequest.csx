public class TrackRequest
{
    public TrackingSettings Settings { get; set; }
    public string Url { get; set; }
}

public class TrackingSettings
{
    public string TrackingId { get; set; }
}
