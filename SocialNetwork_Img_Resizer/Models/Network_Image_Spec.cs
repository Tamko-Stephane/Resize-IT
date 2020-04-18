namespace SocialNetwork_Img_Resizer.Models
{
    public class Network_Image_Spec
    {
        public int Id { get; set; }
        public string ProfileName { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public virtual Social_Network Social_Network { get; set; }
    }
}