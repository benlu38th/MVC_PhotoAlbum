using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace MVC_PhotoAlbum.Models
{
    public partial class Context : DbContext
    {
        public Context()
            : base("name=Context")
        {
        }
        public virtual DbSet<PhotoCategory> PhotoCategories { get; set; }
        public virtual DbSet<Photo> Photos { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
