using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.VisualBasic.FileIO;

namespace Crafts.Api.Controllers
{
    public class upload
    {
        public static string  UploadImageOnCloudinary(IFormFile? file)
        {
            if (file is not null)
            {
                var extension = Path.GetExtension(file.FileName);
                var fileName = $"Img{DateTime.Now.Ticks}{extension}";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                var cloudinary = new Cloudinary(new Account("dwmkkev1m", "473938573154887", "wgKufjzmu74MTJ0LIH5jNo4Q18M"));
                ImageUploadParams uploadParams = new() { File = new FileDescription(path), FilenameOverride = fileName };
                ImageUploadResult uploadResult =  cloudinary.Upload(uploadParams);

                FileSystem.DeleteFile(path);

                return uploadResult.Url.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
