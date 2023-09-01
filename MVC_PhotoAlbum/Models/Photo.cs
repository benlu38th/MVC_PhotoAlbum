using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVC_PhotoAlbum.Models
{
    public class Photo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "編號")]
        public int Id { get; set; }

        [Display(Name = "名稱")]
        public string Title { get; set; }

        [Display(Name = "介紹")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "相片路徑")]
        public string PhotoUrl { get; set; }

        [Display(Name ="設為封面")]
        public Boolean IsCover { get; set; }

        [Display(Name = "創建時間")]
        [DataType(DataType.DateTime)]
        public DateTime? InitDate { get; set; }

        [Display(Name = "分類")]
        public int PhotoCategoryId { get; set; }
        [JsonIgnore]
        [ForeignKey("PhotoCategoryId")]
        public virtual PhotoCategory MyPhotoCategory { get; set; }
    }
}