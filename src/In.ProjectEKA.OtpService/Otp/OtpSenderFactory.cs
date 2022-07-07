namespace In.ProjectEKA.OtpService.Otp
{
	using System.Collections.Generic;
	using System.Linq;
    using Common.Logger;

	public class OtpSenderFactory
    {
        private readonly IEnumerable<string> whitelistedNumbers;
        private readonly OtpSender otpSender;
        private readonly FakeOtpSender fakeOtpSender;

        public OtpSenderFactory(OtpSender otpSender,
            FakeOtpSender fakeOtpSender,
            IEnumerable<string> whitelistedNumbers)
        {
            this.whitelistedNumbers = whitelistedNumbers ?? new List<string>();
            this.otpSender = otpSender;
            this.fakeOtpSender = fakeOtpSender;
        }

        public IOtpSender ServiceFor(string mobileNumber)
        {
            if (mobileNumber != null && whitelistedNumbers.Any(number => number.Contains(mobileNumber)))
            {
                Log.Information("FAKE OTP");
                return fakeOtpSender;
            }

            return otpSender;
        }
    }
}
