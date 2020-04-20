using SharpCompress.Archives;
using SharpCompress.Archives.Zip;
using SharpCompress.Common;
using SocialNetwork_Img_Resizer.Helpers;
using SocialNetwork_Img_Resizer.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SocialNetwork_Img_Resizer.Controllers
{
    public class ResizerController : Controller
    {
        private readonly ResizerDbContext _context;

        public ResizerController()
        {
            _context = new ResizerDbContext();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }



        public ActionResult Contact()
        {
            ViewBag.Message = "Je suis Stéphane, développeur web et mobile, pour plus";

            return View();
        }

        //[HttpPost]
        //public ActionResult UploadFile()
        //{
        //    string directory = Server.MapPath("~/Files_Repo/");//@"C:\Temp\";

        //    HttpPostedFileBase file = Request.Files["file"];

        //    if (file != null && file.ContentLength > 0)
        //    {
        //        var fileName = Path.GetFileName(file.FileName);

        //        file.SaveAs(Path.Combine(directory, fileName));
        //        return Content(Url.Action("ResizeImage"));
        //    }
        //    //return status 
        //    return Json(file.FileName);

        //}

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            try
            {
                string directory = Server.MapPath("~/Files_Repo/");//@"C:\Temp\";

                //HttpPostedFileBase file = Request.Files["file"];

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);

                    file.SaveAs(Path.Combine(directory, fileName));
                    return RedirectToAction("ResizeImage");
                }
                //return status 
                return Json(file.FileName);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                throw e;
            }

        }

        public async Task<ActionResult> ResizeImage()
        {
            //Get infos about the image and social networks to present
            try
            {
                var directory = new DirectoryInfo(Server.MapPath("~/Files_Repo/"));
                var myFile = directory.GetFiles()
                    .OrderByDescending(f => f.LastWriteTime)
                    .First();
                UploadedIMGViewModel model = new UploadedIMGViewModel();
                model.IMG_Name = myFile.Name;
                model.IMG_Path = myFile.FullName;


                model.CompatibleNetworks = await ConvertToVM(await _context.GetSocialNetworksCompatibleWithFile(myFile));
                return View(model);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
                throw e;
            }


        }

        [HttpPost]
        public async Task<ActionResult> ResizeImage(UploadedIMGViewModel model)
        {
            if (ModelState.IsValid)
            {
                //get original image on server
                Image img = Image.FromFile(model.IMG_Path);

                //resize the images by selected profiles and compile in a zip file and send as download
                foreach (var network in model.CompatibleNetworks)
                {
                    if (network.IsSelected)
                    {
                        foreach (var profile in network.IMG_Profile_Specs)
                        {
                            if (profile.IsSelected)
                            {
                                var imageResized = await ResizingMethods.ResizeImage(img, profile.Width, profile.Height);
                                //save img in folder Files_ToZip
                                imageResized.Save(Path.Combine(Server.MapPath("~/Files_ToZip/"), network.Name + "-" + profile.ProfileName.Replace(" ", "_") + "__" + model.IMG_Name));
                            }
                        }
                    }
                }


                //create the zip file to be downloaded
                using (var archive = ZipArchive.Create())
                {
                    archive.AddAllFromDirectory(Server.MapPath("~/Files_ToZip/"));
                    archive.SaveTo(Path.Combine(Server.MapPath("~/File_Zip/") + "ResizedImages.zip"), CompressionType.Deflate);
                }


                // delete files and download result

                string[] filesToDelete = Directory.GetFiles(Server.MapPath("~/Files_ToZip/"));
                foreach (string file in filesToDelete)
                {
                    //if (file.Contains(fileUniqueName.ToString()))
                    //{
                    FileInfo fi = new FileInfo(file);
                    fi.Delete();
                    //}
                }

                Response.ClearContent();
                Response.ClearHeaders();
                //Set zip file name  
                //Response.AppendHeader("content-disposition", "attachment; filename=ResizedImages.zip");

                return File(Path.Combine(Server.MapPath("~/File_Zip/") + "ResizedImages.zip"), "application/zip");//RedirectToAction("Contact");

            }

            return RedirectToAction("Error");
        }

        private async Task<List<Social_Network_VM>> ConvertToVM(Dictionary<Social_Network, List<Network_Image_Spec>> social_Networks_specs)
        {
            if (social_Networks_specs != null)
            {
                List<Social_Network_VM> network_VMs = new List<Social_Network_VM>();
                var t = Task.Run(
                    () =>
                    {
                        network_VMs = social_Networks_specs.Select(s => new Social_Network_VM()
                        {
                            IconFa = s.Key.Icon,
                            Id = s.Key.Id,
                            Name = s.Key.Name,
                            IMG_Profile_Specs = s.Value.Select(i => new IMG_Profile_Spec_VM()
                            {
                                Height = i.Height,
                                Width = i.Width,
                                ProfileName = i.ProfileName,
                                ImgCDNSrc = string.IsNullOrEmpty(i.ImageCDNSrc) ? "#" : i.ImageCDNSrc
                            }).ToList()
                        }).ToList();
                    }

                );

                await t;
                return network_VMs;
            }
            return null;
        }
    }
}