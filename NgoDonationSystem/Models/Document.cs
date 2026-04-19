using System;
using System.Collections.Generic;

namespace NgoDonationSystem.Models
{
    public class Document
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileType { get; set; }
        public DateTime UploadedDate { get; set; }
        
        public ICollection<Expense> Expenses { get; set; }
    }
}
