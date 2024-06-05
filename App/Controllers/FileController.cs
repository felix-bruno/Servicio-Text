using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using App.Domain.Interface;
using App.Domain.Models;


namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class FileController : ControllerBase
    {
        private readonly IFileConvert _fileConvert;
        public FileController(IFileConvert fileConvert)
        {
            _fileConvert = fileConvert;
        }
        [HttpPost("UploadFile")]
        public IActionResult UploadFile(IFormFile file, int numberOfObjects)
        {
            if (file == null || file.Length == 0 || file.ContentType != "text/plain")
            {
                return BadRequest("File is empty");
            }
            try
            {
                List<AppRegistro>? data = _fileConvert.ConvertirData(file, numberOfObjects);
                string jsonContent = JsonConvert.SerializeObject(data);
                List<AppRegistro>? deserializedData = JsonConvert.DeserializeObject<List<AppRegistro>>(jsonContent);
                return Ok(deserializedData);
            }
            catch (Exception ex)
            {
                ResponseRespuesta res = new ResponseRespuesta()
                {
                    Codigo = "4000",
                    Mensaje = "Ocurrio un incoveniente"
                };
                return StatusCode(400,res);
            }
        }
    }
}