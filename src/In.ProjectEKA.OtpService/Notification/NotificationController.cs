using In.ProjectEKA.OtpService.Clients;
using Serilog;

namespace In.ProjectEKA.OtpService.Notification
{
	using System.Threading.Tasks;
	using Common;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;

	[Route("/notification")]
	[ApiController]
	public class NotificationController : Controller
	{
		private readonly INotificationService notificationService;
		private readonly ISmsClient smsClient;

		public NotificationController(INotificationService notificationService, ISmsClient smsClient)
		{
			this.notificationService = notificationService;
			this.smsClient = smsClient;
		}

		[HttpPost]
		public async Task<ActionResult> SendNotification([FromBody] Notification notification)
		{
			return ResponseFrom(await notificationService.SendNotification(notification));
		}
		
		[HttpPost("/sendSMS")]
		public async Task<ActionResult> SendSMS([FromForm] string phoneNumber, [FromForm] string message)
		{
			Log.Information("say hello");
			return ResponseFrom(await smsClient.Send(phoneNumber,message,"1234567"));
		}
		private ActionResult ResponseFrom(Response notificationResponse)
		{
			return notificationResponse.ResponseType switch
			{
				ResponseType.Success => Ok(notificationResponse),
				ResponseType.InternalServerError => StatusCode(StatusCodes.Status500InternalServerError,
					notificationResponse),
				_ => NotFound(notificationResponse)
				};
		}
	}
}
