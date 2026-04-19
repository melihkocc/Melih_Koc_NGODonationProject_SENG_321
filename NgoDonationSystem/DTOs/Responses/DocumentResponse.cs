using System;
using System.ComponentModel.DataAnnotations;

namespace NgoDonationSystem.DTOs.Responses
{
    public class DocumentResponse
    {
        public int Id { get; set; }

        [Display(Name = "Dosya Adı")]
        public string FileName { get; set; }

        [Display(Name = "Dosya Yolu")]
        public string FilePath { get; set; }

        [Display(Name = "Tür")]
        public string FileType { get; set; }

        [Display(Name = "Yükleme Tarihi")]
        public DateTime UploadedDate { get; set; }
    }
}
