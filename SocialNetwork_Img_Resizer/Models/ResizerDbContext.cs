using SocialNetwork_Img_Resizer.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Web;

namespace SocialNetwork_Img_Resizer.Models
{
    public class ResizerDbContext : DbContext
    {
        public DbSet<Social_Network> Social_Networks { get; set; }
        public DbSet<Network_Image_Spec> Network_Image_Specs { get; set; }
        public DbSet<Operation> Operations { get; set; }

        public ResizerDbContext() : base("MyDB")        //db5375457768354c28a4ceaba101160fd9
        {

        }

        public async Task<Guid> SaveFileInDB(HttpPostedFileBase file)
        {
            if (file != null)
            {
                //create new operation
                Operation operation = new Operation();

                var task = Task.Run(
                     () =>
                    {
                        operation.Id = Guid.NewGuid();
                        operation.ContentType = file.ContentType;
                        operation.FileName = file.FileName;
                        operation.FileUploaded = FileConversionMethods.ConvertFileToByteArray(file.InputStream).Result;
                        operation.Date = DateTime.Now;
                        Image image = Image.FromStream(file.InputStream);
                        operation.Width = image.PhysicalDimension.Width;
                        operation.Height = image.PhysicalDimension.Height;
                    }
                    );


                await task;
                //save operation in DB
                this.Operations.Add(operation);
                this.SaveChanges();

                return operation.Id;
            }
            return Guid.Empty;
        }

        public async Task<Operation> GetOperation(Guid guid)
        {
            if (guid != Guid.Empty)
            {
                var operation = this.Operations.FirstOrDefaultAsync(o => o.Id == guid);
                await operation;

                return operation.Result;
            }
            return null;
        }

        public async Task EmptyByteArray(Operation operation)
        {
            if (operation != null)
            {
                operation.FileUploaded = null;
                this.Entry(operation).State = EntityState.Modified;
                await this.SaveChangesAsync();
            }
            return;
        }

        public async Task<Dictionary<Social_Network, List<Network_Image_Spec>>> GetSocialNetworksCompatibleWithFile(FileInfo fileInfo)
        {
            if (fileInfo != null)
            {
                try
                {
                    //var attr = fileInfo.Attributes;
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
        public async Task<Dictionary<Social_Network, List<Network_Image_Spec>>> GetSocialNetworksCompatibleWithFile(Operation operation)
        {
            if (operation != null)
            {
                try
                {

                    //Keep networks profiles which can use/accept the operation's width and height


                    Dictionary<Social_Network, List<Network_Image_Spec>> dictionnary = new Dictionary<Social_Network, List<Network_Image_Spec>>();
                    var networks = await this.Social_Networks.ToListAsync();
                    foreach (var n in networks)
                    {
                        dictionnary.Add(n, await GetSocialNetworkProfiles(n, operation));
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
        public async Task<List<Network_Image_Spec>> GetSocialNetworkProfiles(Social_Network social_Network, Operation operation)
        {
            if (social_Network != null)
            {
                var networks_specs = await this.Network_Image_Specs.ToListAsync();
                List<Network_Image_Spec> image_Specs = new List<Network_Image_Spec>();
                foreach (var spec in networks_specs)
                {
                    if (spec.Social_Network == social_Network)
                    {
                        //check acceptable original file dimensions before adding
                        //if there is a ratio in widths too low the spec is not choosen
                        double ratioWidth = operation.Width / spec.Width;
                        double ratioHeight = operation.Height / spec.Height;
                        if (ratioWidth >= 0.33 && ratioHeight >= 0.33)
                        {
                            image_Specs.Add(spec);
                        }

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