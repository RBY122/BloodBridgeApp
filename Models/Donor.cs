using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BloodBridge.Models
{
    public class Donor : IdentityUser
    {
        [Required(ErrorMessage = "Full name is required.")]
        [MaxLength(100, ErrorMessage = "Full name cannot exceed 100 characters.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Blood group is required.")]
        [MaxLength(5, ErrorMessage = "Blood group should be a short code (e.g., A+, O-).")]
        public string BloodGroup { get; set; } = string.Empty;

        [Required(ErrorMessage = "Location is required.")]
        public string Location { get; set; } = LocationHelper.GetLocations().FirstOrDefault()?.Value ?? "Ashanti - Kumasi";

        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Donation frequency is required.")]
        public DonationFrequencyType DonationFrequency { get; set; } = DonationFrequencyType.Quarterly;

        [Required(ErrorMessage = "National ID is required.")]
        [MaxLength(20, ErrorMessage = "National ID cannot exceed 20 characters.")]
        public string NationalIDNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Emergency contact name is required.")]
        [MaxLength(100, ErrorMessage = "Emergency contact name cannot exceed 100 characters.")]
        public string EmergencyContactName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Emergency contact phone is required.")]
        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        [MaxLength(15, ErrorMessage = "Emergency contact phone cannot exceed 15 characters.")]
        public string EmergencyContactPhone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Availability status is required.")]
        public bool IsAvailableForDonation { get; set; } = true;

        public DateTime? NextAppointment { get; set; }
        public DateTime? NextDonationDate { get; set; } // ✅ Updated for better data consistency
        public DateTime? LastDonationDate { get; set; }
        public int TotalDonations { get; set; } = 0;
        public bool HasSignedConsentForm { get; set; } = false;

        public string ProfilePictureUrl { get; set; } = "/images/default-profile.png";

        // ✅ Database Relationships
        public List<Donation> DonationRecords { get; set; } = new();
        public List<Appointment> Appointments { get; set; } = new();
        public List<Notification> Notifications { get; set; } = new();

        // ✅ **Optimized Profile Completion Tracker**
        public int ProfileCompletionPercentage
        {
            get
            {
                int completedFields = 0;
                int totalFields = 10;

                if (!string.IsNullOrWhiteSpace(FullName)) completedFields++;
                if (!string.IsNullOrWhiteSpace(BloodGroup)) completedFields++;
                if (!string.IsNullOrWhiteSpace(Location)) completedFields++;
                if (DateOfBirth != default) completedFields++;
                if (!string.IsNullOrWhiteSpace(NationalIDNumber)) completedFields++;
                if (!string.IsNullOrWhiteSpace(EmergencyContactName)) completedFields++;
                if (!string.IsNullOrWhiteSpace(EmergencyContactPhone)) completedFields++;
                if (!string.IsNullOrWhiteSpace(ProfilePictureUrl) && ProfilePictureUrl != "/images/default-profile.png") completedFields++;
                if (HasSignedConsentForm) completedFields++;
                if (IsAvailableForDonation) completedFields++;

                return (completedFields * 100) / totalFields;
            }
        }
    }

    public enum DonationFrequencyType
    {
        Default = 0,
        Monthly,
        Quarterly,
        Annually
    }

    // ✅ **Optimized LocationHelper for Immutability**
    public static class LocationHelper
    {
        private static readonly IReadOnlyList<SelectListItem> _locations = new List<SelectListItem>
        {
            new SelectListItem { Value = "Ashanti - Kumasi", Text = "Ashanti - Kumasi" },
            new SelectListItem { Value = "Greater Accra - Accra", Text = "Greater Accra - Accra" },
            new SelectListItem { Value = "Northern - Tamale", Text = "Northern - Tamale" },
            new SelectListItem { Value = "Western - Sekondi-Takoradi", Text = "Western - Sekondi-Takoradi" },
            new SelectListItem { Value = "Eastern - Koforidua", Text = "Eastern - Koforidua" },
            new SelectListItem { Value = "Central - Cape Coast", Text = "Central - Cape Coast" },
            new SelectListItem { Value = "Volta - Ho", Text = "Volta - Ho" },
            new SelectListItem { Value = "Upper East - Bolgatanga", Text = "Upper East - Bolgatanga" },
            new SelectListItem { Value = "Upper West - Wa", Text = "Upper West - Wa" },
            new SelectListItem { Value = "Bono - Sunyani", Text = "Bono - Sunyani" },
            new SelectListItem { Value = "Bono East - Techiman", Text = "Bono East - Techiman" },
            new SelectListItem { Value = "Ahafo - Goaso", Text = "Ahafo - Goaso" },
            new SelectListItem { Value = "Savannah - Damongo", Text = "Savannah - Damongo" },
            new SelectListItem { Value = "North East - Nalerigu", Text = "North East - Nalerigu" },
            new SelectListItem { Value = "Western North - Sefwi Wiawso", Text = "Western North - Sefwi Wiawso" },
            new SelectListItem { Value = "Oti - Dambai", Text = "Oti - Dambai" }
        }.AsReadOnly(); // ✅ Prevents modification

        public static IReadOnlyList<SelectListItem> GetLocations() => _locations;
    }
}