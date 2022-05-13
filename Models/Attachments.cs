using System;

namespace testmvc.Models
{
    public class Attachments
    {

        public int DocumentId { get; set; }     
        
        public int CustomerId { get; set; }

        public int UploadedBy { get; set; }

        public string DocumentName { get; set; }

        public string DocumentType { get; set; }    

        public string UploadDate { get; set; }

        public byte[] Attachment { get; set; } 

        public string src { get; set; } 
    }
}
