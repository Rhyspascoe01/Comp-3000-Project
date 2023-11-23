using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace B.A.S.S.Models
{
    [Table("RouteTable", Schema = "dbo")]

    public class RouteController
    {
        [Key]
        [Column("RouteID")]
        public int RouteID { get; set; }

        [NotMapped]
        public bool Valid { get; set; }

        public string RouteName { get; set; } = string.Empty;
        public int RouteNumber { get; set; }
        public string RouteDescription { get; set; } = string.Empty;
        public string RouteDirection { get; set; } = string.Empty;
    }
}
