#load "TrackRequest.csx"
#r "Newtonsoft.Json"
#r "System.Web"

using Newtonsoft.Json;
using StackExchange.Redis;
using System.Configuration;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    // Get request body
    var body = await req.Content.ReadAsStringAsync();
    var request = JsonConvert.DeserializeObject<TrackRequest>(body);

    // Extract information from IP
    var ip = GetIpAddress(req);
    log.Info($"IP: {ip}");

    using (var client = new HttpClient())
    {
        client.BaseAddress = new Uri("http://ip-api.com/json/");
        var result = await client.GetAsync(ip);
        var json = await result.Content.ReadAsStringAsync();
        dynamic details = JsonConvert.DeserializeObject(json);

        log.Info($"As: {details?.@as}");
        log.Info($"Country: {details?.country}");
    }

    // Store track request in Redis
    var urlHash = SHA256(request.Url);

    var key = $"{request.Settings.TrackingId}:{urlHash}";
    var db = Connection.GetDatabase();
    var total = await db.StringIncrementAsync(urlHash);

    log.Info($"Total hits for {request.Settings.TrackingId} - {request.Url} = {total}");

    return req.CreateResponse(HttpStatusCode.OK, total);
}

private static string SHA256(string url)
{
    var hash = new StringBuilder();
    var crypt = new SHA256Managed();
    var crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(url), 0, Encoding.UTF8.GetByteCount(url));

    foreach (byte theByte in crypto)
    {
        hash.Append(theByte.ToString("x2"));
    }

    return hash.ToString();
}

private static string GetIpAddress(HttpRequestMessage request)
{
    if (request.Properties.ContainsKey("MS_HttpContext"))
    {
        return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
    }

    return string.Empty;
}

private static Lazy<ConnectionMultiplexer> lazyConnection =
    new Lazy<ConnectionMultiplexer>(() =>
    {
        var cnn = ConfigurationManager.AppSettings["RedisConnection"];
        return ConnectionMultiplexer.Connect(cnn);
    });

public static ConnectionMultiplexer Connection
{
    get
    {
        return lazyConnection.Value;
    }
}