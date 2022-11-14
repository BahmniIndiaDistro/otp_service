using System.Net.Http;
using System.Net.Http.Headers;

namespace In.ProjectEKA.OtpService.Clients
{
	using System.Threading.Tasks;
	using Common;
	using Common.Logger;
	using Microsoft.Extensions.Configuration;

	public class SmsClient : ISmsClient
    {
        private readonly IConfiguration configuration;
        private readonly HttpClient httpClient;

        public SmsClient(IConfiguration configuration, HttpClient httpClient)
        {
	        this.configuration = configuration;
	        this.httpClient = httpClient;
        }

        public async Task<Response> Send(string phoneNumber, string message, string templateID)
        {
            Log.Information("sms client");
	        var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api.d7networks.com/messages/v1/send");

	        request.Headers.TryAddWithoutValidation("Accept", "application/json");
	        request.Headers.TryAddWithoutValidation("Authorization", "Bearer ");

	        request.Content =
		        new StringContent(
			        "{\n\"messages\": [{\"channel\": \"sms\", \"recipients\": [\"+919865689295\"], \"content\": \"Greetings from D7 API\", \"msg_type\": \"text\", \"originator\": \"Bahmni\"}]");
	        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

	        var response = await httpClient.SendAsync(request);
	        Log.Information("SMS" + response);
	        return new Response(ResponseType.Success, "Notification sent");


	        // var notification = HttpUtility.UrlEncode(message);
	        // using var webClient = new WebClient();
	        // var response = await webClient.UploadValuesTaskAsync("https://api.textlocal.in/send/",
	        //     new NameValueCollection
	        //     {
	        //         {"apikey" , configuration.GetConnectionString("TextLocaleApiKey")},
	        //         {"numbers" , phoneNumber},
	        //         {"message" , notification},
	        //         {"sender name" , "HCMNCG"},
	        //     });
	        // var json = JObject.Parse(Encoding.UTF8.GetString(response));
	        // Log.Information((string)json["status"] == "success" ? "Success in sending notification" :
	        //     (string)json["errors"][0]["message"]);
	        // return (string)json["status"] == "success" ?  new Response(ResponseType.Success,"Notification sent") 
	        //         :new Response(ResponseType.Success, (string)json["errors"][0]["message"]);
        }
    }
}
