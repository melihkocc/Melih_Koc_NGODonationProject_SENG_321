using System;
using System.ComponentModel.DataAnnotations;

namespace NgoDonationSystem.DTOs.Responses
{
    public class AuditLogResponse
    {
        public int Id { get; set; }

        [Display(Name = "Tablo Adı")]
        public string TableName { get; set; }

        [Display(Name = "İşlem")]
        public string Action { get; set; }

        [Display(Name = "Kullanıcı")]
        public string UserName { get; set; }

        [Display(Name = "Zaman Damgası")]
        public DateTime Timestamp { get; set; }

        [Display(Name = "Eski Değerler")]
        public string OldValues { get; set; }

        [Display(Name = "Yeni Değerler")]
        public string NewValues { get; set; }
    }
}
