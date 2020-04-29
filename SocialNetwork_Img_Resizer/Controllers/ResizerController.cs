using SharpCompress.Archives.Zip;
using SocialNetwork_Img_Resizer.Helpers;
using SocialNetwork_Img_Resizer.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
                //string directory = Server.MapPath("~/Files_Repo/");//@"C:\Temp\";

                //HttpPostedFileBase file = Request.Files["file"];

                if (file != null && file.ContentLength > 0)
                {
                    //var fileName = Path.GetFileName(file.FileName);

                    //file.SaveAs(Path.Combine(directory, fileName));

                    //SAVE FILE IN DB
                    Guid savedGuid = Task.Run(async () => await _context.SaveFileInDB(file)).GetAwaiter().GetResult();
                    if (savedGuid != Guid.Empty)
                    {
                        TempData["guid"] = savedGuid;
                        return RedirectToAction("ResizeImage");
                    }


                }
                //return status 
                return View("Error");//return Json(file.FileName);
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                ViewBag.Error = e.Message;
                throw e;
            }

        }

        public async Task<ActionResult> ResizeImage()
        {
            //Get infos about the image and social networks to present

            try
            {
                Guid savedGuid = new Guid(TempData["guid"].ToString());

                if (savedGuid != Guid.Empty)
                {
                    //Retrieve the file operation
                    Operation operation = Task.Run(async () => await _context.GetOperation(savedGuid)).GetAwaiter().GetResult();

                    if (operation != null)
                    {
                        //var directory = new DirectoryInfo(Server.MapPath("~/Files_Repo/"));
                        //var myFile = directory.GetFiles()
                        //    .OrderByDescending(f => f.LastWriteTime)
                        //    .First();
                        UploadedIMGViewModel model = new UploadedIMGViewModel();
                        model.IMG_Name = operation.FileName;
                        model.IMG_ContentType = operation.ContentType;
                        model.IMG_Guid = savedGuid.ToString();


                        model.CompatibleNetworks = await ConvertToVM(await _context.GetSocialNetworksCompatibleWithFile(operation));
                        return View(model);
                    }
                }
                return View("Error");
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
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
                Operation operation = Task.Run(async () => await _context.GetOperation(new Guid(model.IMG_Guid))).GetAwaiter().GetResult();

                if (operation != null)
                {
                    Image img = Task.Run(async () => await FileConversionMethods.ConvertByteArrayToImage(operation.FileUploaded)).GetAwaiter().GetResult();


                    if (img != null)
                    {
                        MemoryStream zipStream = new MemoryStream();
                        Dictionary<string, MemoryStream> memoryStreams = new Dictionary<string, MemoryStream>();

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
                                        //imageResized.Save(Path.Combine(Server.MapPath("~/Files_ToZip/"), network.Name + "-" + profile.ProfileName.Replace(" ", "_") + "__" + model.IMG_Name));
                                        MemoryStream ms = new MemoryStream();
                                        ImageFormat imageFormat = Utility.GetImageFormat(operation.ContentType.Split('/')[1]);
                                        imageResized.Save(ms, imageFormat);
                                        memoryStreams.Add(network.Name + "/" + network.Name + "-" + profile.ProfileName.Replace(" ", "_") + "__" + model.IMG_Name, ms);
                                    }
                                }
                            }
                        }


                        //create the zip file to be downloaded
                        using (var archive = ZipArchive.Create())
                        {
                            //archive.AddAllFromDirectory(Server.MapPath("~/Files_ToZip/"));
                            foreach (var ms in memoryStreams)
                            {
                                if (!string.IsNullOrEmpty(ms.Key) && ms.Value != null)
                                {
                                    archive.AddEntry(ms.Key, ms.Value, ms.Value.Length, DateTime.Now);
                                }
                            }



                            //archive.SaveTo(Path.Combine(Server.MapPath("~/File_Zip/") + "ResizedImages.zip"), CompressionType.Deflate);
                            archive.SaveTo(zipStream);

                        }

                        //free img ressource
                        img.Dispose();

                        // delete files and download result
                        //memoryStreams = null;
                        //List<string> filesToDelete = new List<string>(Directory.GetFiles(Server.MapPath("~/Files_ToZip/")));
                        //filesToDelete.AddRange(Directory.GetFiles(Server.MapPath("~/Files_Repo/")));

                        //foreach (string file in filesToDelete)
                        //{
                        //    //if (file.Contains(fileUniqueName.ToString()))
                        //    //{
                        //    FileInfo fi = new FileInfo(file);
                        //    if (fi != null && fi.Exists && fi.FullName != model.IMG_Path)
                        //        fi.Delete();
                        //    //}
                        //}

                        Task.Run(async () => await _context.EmptyByteArray(operation)).GetAwaiter().GetResult();

                        //Response.ClearContent();
                        //Response.ClearHeaders();
                        //Set zip file name  

                        //Response.AppendHeader("statusCode", "200");
                        string fileName = "ResizedImages_" + operation.Id.ToString().Substring(5, 5) + "_" + DateTime.Now.ToShortDateString() + "_resizeit.zip";
                        //Response.AppendHeader("content-disposition", "attachment; filename=" + fileName);
                        var cd = new System.Net.Mime.ContentDisposition
                        {
                            // for example foo.bak
                            FileName = fileName,

                            // always prompt the user for downloading, set to true if you want 
                            // the browser to try to show the file inline
                            Inline = true,
                        };
                        Response.AppendHeader("Content-Disposition", cd.ToString());



                        return File(zipStream.ToArray(), "application/rar");//File(Path.Combine(Server.MapPath("~/File_Zip/") + "ResizedImages.zip"), "application/zip");//RedirectToAction("Contact");

                    }
                }
                return View("Error");
            }

            return RedirectToAction("Index");
        }

        public ActionResult ShowImage(Guid id)
        {
            Operation operation = Task.Run(async () => await _context.GetOperation(id)).GetAwaiter().GetResult();

            if (operation != null)
            {
                return File(operation.FileUploaded, operation.ContentType);
            }
            return Content("#");
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