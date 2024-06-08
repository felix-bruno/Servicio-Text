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
        [HttpGet]
        public IActionResult UploadFile()
        {
            return Ok(true);
        }
        [HttpPost("UploadFile")]
        public IActionResult UploadFile(IFormFile file, int numberOfObjects)
        {
            if (file == null || file.Length == 0 || file.ContentType != "text/plain")
            {
                return BadRequest("File is empty");
            }
            if(numberOfObjects <= 0)
            {
                return BadRequest("Enviar un valor mayor a cero");
            }
            try
            {
                List<AppRegistro>? data = _fileConvert.ConvertirData(file, numberOfObjects);
                if(data == null || data.Count == 0)
                {
                    throw new Exception("Ocurrio un inconveniente comunicarse con soporte");
                }
                string jsonContent = JsonConvert.SerializeObject(data);
                List<AppRegistro>? deserializedData = JsonConvert.DeserializeObject<List<AppRegistro>>(jsonContent);
                return Ok(deserializedData);
            }
            catch (Exception ex)
            {
                ResponseRespuesta res = new ResponseRespuesta()
                {
                    Codigo = "5000",
                    Mensaje = ex.Message
                };
                return StatusCode(500,res);
            }
        }
    }
}