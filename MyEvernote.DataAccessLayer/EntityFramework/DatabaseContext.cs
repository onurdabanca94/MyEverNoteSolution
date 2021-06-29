using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DataAccessLayer.EntityFramework
{
    public class DatabaseContext : DbContext
    {
        public DbSet<EvernoteUser> EvernoteUsers { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Liked> Likes { get; set; }

        public DatabaseContext()
        {
            Database.SetInitializer(new MyInitializer());
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
            // FluentAPI DataAnnotations işlemlerinin bir türevi daha çok SOLID mimari prensiplerine uygunluk açısından dolayı tercih sebebi teşkil ediyor. Attributes ile Class içerisinde iş kuralı koyuyormuşuz gibi bir anlam çıkarıldığı için alternatif bir yöntemdir.
           /* modelBuilder.Entity<Note>()*/ //Note Entitysi
                /*.HasMany(n => n.Comments)*/ //Çok ilişkilidir Commentsler(tablosu) ile
                /*.WithRequired(c => c.Note)*/ //Comment'in note propertysi ile ilişkili boş geçilemez kuralı vardır.
                /*.WillCascadeOnDelete(true);*/ //Delete cascade'i kod tarafında işaretlemiş oluyoruz. 196.Video'da bunu DB üzerinden manuel olarak yapımını anlatıyor.

        //    modelBuilder.Entity<Note>()
        //        .HasMany(n => n.Likes)
        //        .WithRequired(c => c.Note)
        //        .WillCascadeOnDelete(true);
        //}
    }
}
