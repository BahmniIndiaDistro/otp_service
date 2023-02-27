using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace In.ProjectEKA.OtpService.Clients
{
	using System.Threading.Tasks;
	using Common;
	using Common.Logger;
	using System.Net.Http;
	using System.Net.Mime;
	using System.Text;
	using System.Text.Json;

	public class SmsClient : ISmsClient
    {
	    private readonly HttpClient httpClient;
        private readonly D7SmsServiceProperties d7SmsServiceProperties;

        public SmsClient(D7SmsServiceProperties d7SmsServiceProperties)
        {
	        httpClient = new HttpClient();
	        this.d7SmsServiceProperties = d7SmsServiceProperties;
        }

        public async Task<Response> Send(string phoneNumber, string message, string templateId)
        {
	        try
            {
	            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.d7networks.com/messages/v1/send");

	            request.Headers.Add("Accept", "application/json");
	            request.Headers.Add("Authorization", "Bearer " + d7SmsServiceProperties.Token);
	            var messages = new List<D7Message>();
	            messages.Add(new D7Message(d7SmsServiceProperties.Channel, new List<string>() {phoneNumber}, message,
		            "text", d7SmsServiceProperties.Originator));
	            var json = JsonConvert.SerializeObject(new {messages});
	            request.Content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
	            

	            HttpResponseMessage response = await httpClient.SendAsync(request);
	            
	            var contents = await response.Content.ReadAsStringAsync();
	            Log.Information(contents);
	            if (response.StatusCode == (HttpStatusCode) 200)
		            return new Response(ResponseType.Success, "Notification sent");
	            if (response.StatusCode == (HttpStatusCode) 402)
		            return new Response(ResponseType.Error402, "Error in sending notification");
	            Log.Error(response.StatusCode,response.Content);
            }
	        catch (Exception exception)
	        {
		        Log.Error(exception, exception.StackTrace);
		        return new Response(ResponseType.InternalServerError, "Unable to send message.");
	        }
	        return new Response(ResponseType.Error, "Error in sending notification");
        }
    }
}
