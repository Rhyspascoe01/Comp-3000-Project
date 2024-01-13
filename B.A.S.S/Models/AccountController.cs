using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace B.A.S.S.Models
{
    [Table("Accounts", Schema = "dbo")]
    public class AccountController 
    {
        [Key]
        [Column("UserName")]
        public string UserName { get; set; } = string.Empty;

        [NotMapped]
        public bool Valid { get; set; }

        public string Passwords { get; set; } = string.Empty;

        public string Roles { get; set; } = string.Empty;

    }
}
