using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace B.A.S.S.Models
{
    [Table("BusDriverTable", Schema = "dbo")]

    public class BusController
    {
        [Key]
        [Column("BusDriverID")]
        public int BusDriverID { get; set; }

        [NotMapped]
        public bool Valid { get; set; }

        public int RouteID { get; set; }
        public string BusDriverName { get; set; } = string.Empty;

        public string BusDriverDOB { get; set; } = string.Empty;
        public double BusDriverStartShift { get; set; }
        public double BusDriverEndShift { get; set; } 

    }
}