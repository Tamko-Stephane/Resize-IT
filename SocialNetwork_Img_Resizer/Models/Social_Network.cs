using System.Collections.Generic;

namespace SocialNetwork_Img_Resizer.Models
{
    public class Social_Network
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public ICollection<Network_Image_Spec> MyProperty { get; set; }
    }
}