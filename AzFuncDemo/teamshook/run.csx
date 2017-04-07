#r "Newtonsoft.Json"

using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Threading;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    using (var client = new HttpClient())
    {
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Add("User-Agent", "AzureFunctions");

        const string title = "Philippine .NET Users Group posted a new event";
        const string message = "Tue 28 March 2017 from 18:00 to 21:00 PHT. PHINUG March 2017 MSDN Session.";

        const string uri = "https://outlook.office.com/webhook/3c879d89-1476-44b6-8b0f-6b928223ee7c@d1c1e846-fd3f-4a45-ba0f-161f3748735e/IncomingWebhook/cbb04a712c624f539db27b0d69728a29/8b56ba22-b572-458f-9128-e9499bec668d";

        var targetUrl = new List<string> { "https://www.eventbrite.co.uk/e/phinug-march-2017-msdn-session-tickets-33076377335" };
        var action = new Action { context = "https://schema.org", type = "ViewAction", name = "Click here to register!", target = targetUrl };
        var actions = new List<Action> { action };

        var msg = new TeamHook { title = title, text = message, themeColor = "EA4300", potentialAction = actions };
        var TeamMsg = new StringContent(JsonConvert.SerializeObject(msg));

        return await client.PostAsync(uri, TeamMsg);
    }
}

public class TeamHook
{
    public string title { get; set; }
    public string text { get; set; }
    public string themeColor { get; set; }
    public List<Action> potentialAction { get; set; }
}

public class Action
{
    [JsonProperty(PropertyName = "@content")]
    public string context { get; set; }
    [JsonProperty(PropertyName = "@type")]
    public string type { get; set; }
    public string name { get; set; }
    public List<string> target { get; set; }
}