using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BloodBridge.Models
{
    public class Donation
    {
        [Key]
        public int Id { get; set; }

        // ✅ Donation Amount with Proper Validation
        [Required(ErrorMessage = "Donation amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        // ✅ Tracks Donation Date (Ensures UTC conversion)
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DonationDate { get; set; } = DateTime.UtcNow;

        // ✅ Maps DonorId to NationalIDNumber in Donor Model
        [Required]
        [ForeignKey("Donor")]
        public string DonorId { get; set; } = string.Empty;  

        public Donor? Donor { get; set; } // References NationalIDNumber

        // ✅ Transaction Status Handling (Using Enum)
        [Required]
        public DonationStatus Status { get; set; } = DonationStatus.Pending;

        // ✅ Transaction ID for External Payment Tracking
        [Required]
        public string TransactionId { get; set; } = Guid.NewGuid().ToString();

        // ✅ **New: Track Payment Method**
        [Required]
        public PaymentMethodType PaymentMethod { get; set; } = PaymentMethodType.MobileMoney;

        // ✅ **New: Enable Recurring Donations**
        public bool IsRecurringDonation { get; set; } = false;
    }

    // ✅ Enum to Define Allowed Payment Statuses
    public enum DonationStatus
    {
        Pending,
        Processing, // ✅ Added for better tracking
        Completed,
        Failed,
        Refunded // ✅ Added refund state
    }

    // ✅ Enum to Define Allowed Payment Methods
    public enum PaymentMethodType
    {
        Card,
        MobileMoney,
        BankTransfer
    }
}