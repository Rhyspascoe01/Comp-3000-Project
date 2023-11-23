using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace B.A.S.S.Models
{
    [Table("StopTable", Schema = "dbo")]

    public class StopController
    {
        [Key]
        [Column("StopID")]
        public int StopID { get; set; }

        [NotMapped]
        public bool Valid { get; set; }
        
        public int RouteID { get; set; }
        public string StopName { get; set; } = string.Empty;

        public double StopTime { get; set; } 
        public string RouteName { get; set; } = string.Empty;
        public string RouteDescription { get; set; } = string.Empty;

    }
}
