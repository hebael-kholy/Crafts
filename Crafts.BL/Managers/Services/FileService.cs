using Crafts.DAL.Repos.CategoryRepo;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crafts.BL.Managers.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;
        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public Tuple<int, string> SaveImage(IFormFile imageFile)
        {
            try
            {
                //Save the file to the images directory
                var imagePath = Path.Combine(_env.WebRootPath, "Images");

                //Check if the images directory exist, if not it will be created
                if (!Directory.Exists(imagePath))
                {
                    Directory.CreateDirectory(imagePath);
                }

                //Check allowed extensions
                var ext = Path.GetExtension(imageFile.FileName);
                var allowedExtensions = new string [] { ".jpg" , ".jpeg" , ".png"};
                
                if(!allowedExtensions.Contains(ext))
                {
                    //Note: string.Format is like the string concatenation
                    string msg = string.Format("Only {0} extensions are allowed", string.Join(",", allowedExtensions));
                    return new Tuple<int, string>(0, msg);
                }
                
                string uniqueString = Guid.NewGuid().ToString(); //To add a unique name to the imageFileName
                var newFileName = uniqueString + ext; //And to add the extension to the unique name created   
                var filePath = Path.Combine(imagePath, newFileName); //Then Combine path again with the new filename

                //Creating fileStream
                var stream = new FileStream(filePath, FileMode.Create);
                imageFile.CopyTo(stream);
                stream.Close();

                return new Tuple<int, string>(1, newFileName);
            }
            catch(Exception ex)
            {
                return new Tuple<int, string>(0, "An Error Has Occurred");
            }
        }
        public bool DeleteImage(string imageFileName)
        {
            try
            {
                var wwwPath = _env.WebRootPath;
                var imagePath = Path.Combine(wwwPath, "Images\\", imageFileName);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
