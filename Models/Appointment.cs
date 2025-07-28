using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BloodBridge.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.UtcNow; // Default to current UTC time

        [Required(ErrorMessage = "Location is required.")]
        [MaxLength(255)]
        public string Location { get; set; } = string.Empty;

        // ✅ Use Hospital instead of Donor
        [Required]
        [ForeignKey("Hospital")]
        public string? HospitalId { get; set; }

        public Hospital? Hospital { get; set; } // Navigation property

        public string ConfirmationMessage { get; set; } = string.Empty;

        // ✅ Static Predefined Blood Group List
        public static IReadOnlyList<SelectListItem> BloodGroupList { get; } = new List<SelectListItem>
        {
            new("A+", "A+"), new("A-", "A-"), new("B+", "B+"), new("B-", "B-"),
            new("O+", "O+"), new("O-", "O-"), new("AB+", "AB+"), new("AB-", "AB-")
        };

        // ✅ Static Predefined Hospital List
        public static IReadOnlyList<SelectListItem> HospitalList { get; } = new List<SelectListItem>
        {
            new SelectListItem { Text = "Korle Bu Teaching Hospital", Value = "KorleBuTeachingHospital" },
            new SelectListItem { Text = "37 Military Hospital", Value = "MilitaryHospital37" },
            new SelectListItem { Text = "Komfo Anokye Teaching Hospital", Value = "KomfoAnokyeTeachingHospital" },
            new SelectListItem { Text = "Tamale Teaching Hospital", Value = "TamaleTeachingHospital" },
            new SelectListItem { Text = "Cape Coast Teaching Hospital", Value = "CapeCoastTeachingHospital" },
            new SelectListItem { Text = "Ho Teaching Hospital", Value = "HoTeachingHospital" },
            new SelectListItem { Text = "Sunyani Regional Hospital", Value = "SunyaniRegionalHospital" },
            new SelectListItem { Text = "Koforidua Regional Hospital", Value = "KoforiduaRegionalHospital" },
            new SelectListItem { Text = "Effia Nkwanta Regional Hospital", Value = "EffiaNkwantaRegionalHospital" },
            new SelectListItem { Text = "Wa Regional Hospital", Value = "WaRegionalHospital" },
            new SelectListItem { Text = "Bolgatanga Regional Hospital", Value = "BolgatangaRegionalHospital" }
        };
    }
}