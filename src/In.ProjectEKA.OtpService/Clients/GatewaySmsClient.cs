using System.Net.Http.Headers;

namespace In.ProjectEKA.OtpService.Clients
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Runtime.Caching;
    using Common;
    using Common.Logger;
    using Otp;
	public class GatewaySmsClient : ISmsClient
    {
        private readonly SmsServiceProperties smsServiceProperties;
        private readonly HttpClient httpClient;
        private MemoryCache cache;

        public GatewaySmsClient(SmsServiceProperties smsServiceProperties)
        {
            this.smsServiceProperties = smsServiceProperties;
            httpClient = new HttpClient();
            cache = MemoryCache.Default;
        }

        public async Task<Response> Send(string phoneNumber, string message, string templateId)
        {
            Log.Information("gateway sms client");
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
            // var phoneNumberWithCountryCode = phoneNumber;
            // if (phoneNumber.Contains('-'))
            //     phoneNumberWithCountryCode = phoneNumber.Replace("+", string.Empty).Replace("-", String.Empty);
            //
            // try
            // {
            //     var uriBuilder = new UriBuilder(smsServiceProperties.SmsApi);
            //     uriBuilder.Port = -1;
            //     var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            //     query["username"] = smsServiceProperties.ClientId;
            //     query["pin"] = smsServiceProperties.ClientSecret;
            //     query["message"] = message;
            //     query["mnumber"] = phoneNumberWithCountryCode;
            //     query["dlt_template_id"] = templateId;
            //     query["signature"] = smsServiceProperties.Signature;
            //     query["dlt_entity_id"] = smsServiceProperties.EntityId;
            //
            //     uriBuilder.Query = query.ToString();
            //     
            //     var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.ToString());
            //
            //     var response = await client
            //         .SendAsync(request)
            //         .ConfigureAwait(false);
            //
            //     var contents = await response.Content.ReadAsStringAsync();
            //     Log.Information(contents);
            //     if (response.StatusCode == (HttpStatusCode) 200)
            //         return new Response(ResponseType.Success, "Notification sent");
            //     Log.Error(response.StatusCode,response.Content);
            // }
            // catch (Exception exception)
            // {
            //     Log.Error(exception, exception.StackTrace);
            //     return new Response(ResponseType.InternalServerError, "Unable to create otp message.");
            // }
            //
            // return new Response(ResponseType.Success, "Error in sending notification");
        }
    }
}