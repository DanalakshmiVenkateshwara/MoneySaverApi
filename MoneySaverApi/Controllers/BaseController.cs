using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace MoneySaverApi.Controllers
{
    [ApiController]
    [EnableCors("PickupAndDeliveryCors")]
    public abstract class BaseController : ControllerBase
    {
        public static List<string> SaveFiles(string path, IFormFileCollection files, string type)
        {
            List<string> fileNames = new();

            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (files != null && files.Count > 0)
                {
                    foreach (var file in files)
                    {
                        string name = string.IsNullOrEmpty(type) ? file.Name : type;
                        if (file.Length > 0)
                        {
                            string fileName = $"{Guid.NewGuid()}_{name}_{ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"')}";
                            using (var stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                            {
                                file.CopyTo(stream);
                            }
                            fileNames.Add(fileName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return fileNames;
        }
    }
}
