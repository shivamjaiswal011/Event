using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using eventapi.Models;
using events.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace events.Controllers
{
    [EnableCors("AnotherPolicy")]
    [ApiController]
    [Route("[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IMongoCollection<Eventdetail> Collection;

        private readonly IMongoCollection<Footer> Col;

        private readonly IWebHostEnvironment _hostEnvironment;

        public EventsController(IMongoClient client, IWebHostEnvironment hostEnvironment)
        {
            var database = client.GetDatabase("events");
            Collection = database.GetCollection<Eventdetail>("Event");
            Col = database.GetCollection<Footer>("Footer");
            this._hostEnvironment = hostEnvironment;
        }

        [EnableCors("AnotherPolicy")]
        [HttpGet]
        public object Get()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(Collection.Find(new BsonDocument()).ToList());
        }
        [EnableCors("AnotherPolicy")]
        [Route("footer")]
        [HttpGet]
        public object Getfooter()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(Col.Find(new BsonDocument()).ToList());
        }

        [EnableCors("AnotherPolicy")]
        [Route("admin")]
        [HttpPost]
        public async Task<ActionResult<Eventdetail>> Addnew([FromForm] Eventdetail objVM)
        {
            objVM.Imagename = await SaveImage(objVM.Imagefile);

            objVM.Imagesrc = $"{Request.Scheme}://{Request.Host}/Images/{objVM.Imagename}";
            
            Collection.InsertOne(objVM);

            return StatusCode(201);
        }

        [EnableCors("AnotherPolicy")]
        [NonAction]
        public async Task<string> SaveImage(IFormFile Imagefile)
        {
            string Imagename = new String(Path.GetFileNameWithoutExtension(Imagefile.FileName).Take(10).ToArray()).Replace(' ', '-');
            Imagename = Imagename + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(Imagefile.FileName);
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", Imagename);
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
               await Imagefile.CopyToAsync(fileStream);
            }
            return Imagename;
        }

        [EnableCors("AnotherPolicy")]
        [Route("admin/get")]
        [HttpGet]
        public object GetAll()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(Collection.Find(new BsonDocument()).ToList());
        }

        [EnableCors("AnotherPolicy")]
        [Route("admin/delete")]
        [HttpDelete]
        public object Delete(string tittle)
        {
            try
            {
                Collection.DeleteOneAsync(
                               Builders<Eventdetail>.Filter.Eq("Tittle", tittle));
                return new Status
                { Result = "Success", Message = "Employee Details Delete  Successfully" };
            }
            catch (Exception ex)
            {
                return new Status
                { Result = "Error", Message = ex.Message.ToString() };
            }
        }
    }
}
