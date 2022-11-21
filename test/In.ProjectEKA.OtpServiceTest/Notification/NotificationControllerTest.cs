using In.ProjectEKA.OtpService.Clients;

namespace In.ProjectEKA.OtpServiceTest.Notification
{
	using System.Threading.Tasks;
	using Builder;
	using FluentAssertions;
	using Microsoft.AspNetCore.Mvc;
	using Moq;
	using OtpService.Common;
	using OtpService.Notification;
	using Xunit;

	public class NotificationControllerTest
    {
        private readonly NotificationController notificationController;
        private readonly Mock<ISmsClient> smsClient = new Mock<ISmsClient>();
        private readonly Mock<INotificationService> notificationService = new Mock<INotificationService>();

        public NotificationControllerTest()
        {
            notificationController = new NotificationController(notificationService.Object,smsClient.Object);
        }
        
        [Fact]
        public async Task ShouldSuccessInNotificationSend()
        {
            var expectedResponse = new Response(ResponseType.Success, "Notification sent");
            notificationService.Setup(e => e.SendNotification(It.IsAny<Notification>())
            ).ReturnsAsync(expectedResponse);
            
            var response = await notificationController.SendNotification(TestBuilder.GenerateNotificationMessage());
            
            notificationService.Verify();
            response.Should()
                .NotBeNull()
                .And
                .Subject.As<OkObjectResult>()
                .Value
                .Should()
                .BeEquivalentTo(expectedResponse);
        }
        
        [Fact]
        public async Task ShouldSuccessInNotificationSms()
        {
	        var expectedResponse = new Response(ResponseType.Success, "Notification sent");
	        smsClient.Setup(e => e.Send("+919876543210","message",null)).ReturnsAsync(expectedResponse);
	        
	        var response = await notificationController.SendSMS("+919876543210","message");
	        
	        smsClient.Verify();
	        response.Should()
		        .NotBeNull()
		        .And
		        .Subject.As<OkObjectResult>()
		        .Value
		        .Should()
		        .BeEquivalentTo(expectedResponse);
        }
    }
}