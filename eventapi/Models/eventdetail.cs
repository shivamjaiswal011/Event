using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson.Serialization.Attributes;

namespace events.Models
{
    [BsonIgnoreExtraElements]
    public class Eventdetail
    {
        [BsonElement("Tittle")]
        public string Tittle { get; set; }

        [BsonElement("Location")]
        public string Location { get; set; }

        [BsonElement("Price")]
        public string Price { get; set; }

        [BsonElement("Date")]
        public string Date { get; set; }

        [BsonElement("About")]
        public string About { get; set; }

        [BsonElement("Learn")]
        public string Learn { get; set; }

        [BsonElement("Imagename")]
        public string Imagename { get; set; }

        [BsonElement("Imagesrc")]
        public string Imagesrc { get; set; }

        [BsonIgnore]
        public IFormFile Imagefile { get; set; }

        public Eventdetail()
        {
        }
    }
}
