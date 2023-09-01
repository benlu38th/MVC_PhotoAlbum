using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVC_PhotoAlbum.Models
{
    public class PhotoCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name ="Id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "相簿名稱")]
        [MaxLength(20)]
        public string Name { get; set; }

        [Display(Name = "創建時間")]
        [DataType(DataType.DateTime)]
        public DateTime InitDate { get; set; }

    }
}