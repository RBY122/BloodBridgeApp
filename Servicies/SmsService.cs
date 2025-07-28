using System;
namespace BloodBridge.Services;
using System.Security.Cryptography;
using System.Collections.Concurrent;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

public class SmsService
{
    private const string accountSid = "YOUR_TWILIO_ACCOUNT_SID";
    private const string authToken = "YOUR_TWILIO_AUTH_TOKEN";
    private const string twilioPhoneNumber = "+1234567890"; // Twilio verified number

    private static ConcurrentDictionary<string, DateTime> otpRequestTracker = new ConcurrentDictionary<string, DateTime>();
    private const int RequestCooldownMinutes = 1; // Prevent excessive OTP requests

    // Generate a cryptographically secure OTP
    public static string GenerateSecureOtp()
    {
        int otpValue = RandomNumberGenerator.GetInt32(100000, 999999); // Secure 6-digit OTP
        return otpValue.ToString("D6"); // Ensures correct format
    }

    public static bool SendOtpSms(string userPhoneNumber)
    {
        var now = DateTime.UtcNow;

        // Prevent duplicate OTP requests within cooldown period
        if (otpRequestTracker.TryGetValue(userPhoneNumber, out var lastRequestTime) && now < lastRequestTime.AddMinutes(RequestCooldownMinutes))
        {
            Console.WriteLine($"OTP request blocked: Cooldown period active for {userPhoneNumber}");
            return false;
        }

        string otp = GenerateSecureOtp();
        otpRequestTracker[userPhoneNumber] = now; // Track request time

        try
        {
            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: $"Your BloodBridge OTP is: {otp}. This expires in 5 minutes.",
                from: new PhoneNumber(twilioPhoneNumber),
                to: new PhoneNumber(userPhoneNumber)
            );

            Console.WriteLine($"OTP Sent Successfully: {message.Sid} to {userPhoneNumber}");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send OTP to {userPhoneNumber}: {ex.Message}");
            return false;
        }
    }
}