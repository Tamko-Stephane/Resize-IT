using System.Collections.Generic;
using System.Web.Mvc;

namespace SocialNetwork_Img_Resizer.Models
{
    public class UploadedIMGViewModel
    {
        [HiddenInput]
        public string IMG_Name { get; set; }
        [HiddenInput]
        public string IMG_Path { get; set; }
        public string IMG_ContentType { get; set; }
        [HiddenInput]
        public string IMG_Guid { get; set; }
        public List<Social_Network_VM> CompatibleNetworks { get; set; }
        public UploadedIMGViewModel()
        {
            CompatibleNetworks = new List<Social_Network_VM>();
        }
    }

    public class Social_Network_VM
    {
        [HiddenInput]
        public int Id { get; set; }
        [HiddenInput]
        public string Name { get; set; }
        public string IconFa { get; set; }
        public bool IsSelected { get; set; }
        public List<IMG_Profile_Spec_VM> IMG_Profile_Specs { get; set; }

    }

    public class IMG_Profile_Spec_VM
    {
        [HiddenInput]
        public int Id { get; set; }
        [HiddenInput]
        public int Height { get; set; }
        [HiddenInput]
        public int Width { get; set; }
        public bool IsSelected { get; set; }
        public string ImgCDNSrc { get; set; }

        [HiddenInput]
        public string ProfileName { get; set; }

    }
}