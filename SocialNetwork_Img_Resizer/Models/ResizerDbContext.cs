using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Threading.Tasks;

namespace SocialNetwork_Img_Resizer.Models
{
    public class ResizerDbContext : DbContext
    {
        public DbSet<Social_Network> Social_Networks { get; set; }
        public DbSet<Network_Image_Spec> Network_Image_Specs { get; set; }

        public ResizerDbContext() : base("db5375457768354c28a4ceaba101160fd9")
        {

        }

        public async Task<Dictionary<Social_Network, List<Network_Image_Spec>>> GetSocialNetworksCompatibleWithFile(FileInfo fileInfo)
        {
            if (fileInfo != null)
            {
                try
                {
                    var attr = fileInfo.Attributes;
                    Dictionary<Social_Network, List<Network_Image_Spec>> dictionnary = new Dictionary<Social_Network, List<Network_Image_Spec>>();
                    var networks = await this.Social_Networks.ToListAsync();
                    foreach (var n in networks)
                    {
                        dictionnary.Add(n, await GetSocialNetworkProfiles(n));
                    }
                    return dictionnary;
                }
                catch (Exception e)
                {

                    throw e;
                }

            }
            return null;
        }
        public async Task<List<Network_Image_Spec>> GetSocialNetworkProfiles(Social_Network social_Network)
        {
            if (social_Network != null)
            {
                var networks_specs = await this.Network_Image_Specs.ToListAsync();
                List<Network_Image_Spec> image_Specs = new List<Network_Image_Spec>();
                foreach (var spec in networks_specs)
                {
                    if (spec.Social_Network == social_Network)
                    {
                        image_Specs.Add(spec);
                    }
                }
                return image_Specs;
            }
            return null;
        }
        //public async Task<List<Social_Network>> GetSocialNetworksCompatibleWithFile(FileInfo fileInfo)
        //{
        //    if (fileInfo != null)
        //    {
        //        var attr = fileInfo.Attributes;
        //        return await this.Social_Networks.ToListAsync();
        //    }
        //    return null;
        //}
    }
}