using BloodBridge.Data;
using BloodBridge.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BloodBridge.Models
{
    public class ChatPost
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public int? ReplyToId { get; set; }

        public List<string> Tags { get; set; } = new List<string>();

       public string BloodGroup { get; set; } = string.Empty;



        [NotMapped]
        public Dictionary<string, int> Reactions { get; set; } = new Dictionary<string, int>();
    }
}