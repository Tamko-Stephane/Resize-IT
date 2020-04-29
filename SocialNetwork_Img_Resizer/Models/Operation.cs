using System;

namespace SocialNetwork_Img_Resizer.Models
{
    public class Operation
    {
        public Guid Id { get; set; }
        public byte[] FileUploaded { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public float Height { get; set; }
        public float Width { get; set; }
        public DateTime Date { get; set; }

    }
}