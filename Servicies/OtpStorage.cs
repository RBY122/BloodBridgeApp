namespace BloodBridge.Services;

using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Security.Cryptography;

public static class OtpStorage
{
    private static ConcurrentDictionary<string, (string Otp, DateTime Expiry)> otpStore = new ConcurrentDictionary<string, (string, DateTime)>();
    private static ConcurrentDictionary<string, DateTime> otpRequestTracker = new ConcurrentDictionary<string, DateTime>(); // Tracks request times

    private const int ExpiryMinutes = 5;
    private const int RequestCooldownMinutes = 1; // Prevent multiple requests in a short time

    // Store OTP securely with an expiration time
    public static bool StoreOtp(string phoneNumber, string otp)
    {
        var now = DateTime.UtcNow;

        // Prevent duplicate requests within cooldown
        if (otpRequestTracker.TryGetValue(phoneNumber, out var lastRequestTime) && now < lastRequestTime.AddMinutes(RequestCooldownMinutes))
        {
            return false; // Request blocked due to cooldown
        }

        otpStore[phoneNumber] = (otp, now.AddMinutes(ExpiryMinutes));
        otpRequestTracker.TryAdd(phoneNumber, now); // Track request time securely
        return true;
    }

    // Verify OTP and remove it upon successful authentication
    public static bool VerifyOtp(string phoneNumber, string enteredOtp)
    {
        if (otpStore.TryGetValue(phoneNumber, out var storedOtp))
        {
            if (storedOtp.Expiry > DateTime.UtcNow && storedOtp.Otp == enteredOtp)
            {
                otpStore.TryRemove(phoneNumber, out _);
                otpRequestTracker.TryRemove(phoneNumber, out _); // Clear request tracker
                return true;
            }
        }
        return false;
    }

    // Remove expired OTPs
    public static void CleanupExpiredOtps()
    {
        foreach (var key in otpStore.Keys.ToList())
        {
            if (otpStore.TryGetValue(key, out var storedOtp) && storedOtp.Expiry <= DateTime.UtcNow)
            {
                otpStore.TryRemove(key, out _);
                otpRequestTracker.TryRemove(key, out _); // Cleanup request tracker
            }
        }
    }

    // Block OTP requests within cooldown period
    public static bool IsOtpRequestBlocked(string phoneNumber, TimeSpan cooldown)
    {
        return otpRequestTracker.TryGetValue(phoneNumber, out var lastRequestTime) && DateTime.UtcNow < lastRequestTime.Add(cooldown);
    }

    // Remove OTP manually (for verification success)
    public static void RemoveOtp(string phoneNumber)
    {
        otpStore.TryRemove(phoneNumber, out _);
    }
}

// Separate OtpService class for OTP generation
public static class OtpService
{
    public static string GenerateSecureOtp()
    {
        int otpValue = RandomNumberGenerator.GetInt32(100000, 999999); // Secure 6-digit OTP
        return otpValue.ToString("D6"); // Ensures correct format
    }
}