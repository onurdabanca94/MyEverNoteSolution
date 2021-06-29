using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using MyEvernote.Entities;

namespace MyEvernote.DataAccessLayer.EntityFramework
{
    public class MyInitializer : CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            //Admin kullanıcı ekleme.
            EvernoteUser admin = new EvernoteUser()
            {
                Name = "Onur",
                Surname = "Onur",
                Email = "yourmail@gmail.com",
                ActiveGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = true,
                Username = "onuronur",
                ProfileImageFilename = "user_boy.png",
                Password = "123456",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUsername = "onuronur"
            };

            //Standart kullanıcı ekleme.
            EvernoteUser standartUser = new EvernoteUser()
            {
                Name = "Abc",
                Surname = "Abc",
                Email = "abcdbnc@gmail.com",
                ActiveGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = false,
                Username = "abcabc",
                ProfileImageFilename = "user_boy.png",
                Password = "654321",
                CreatedOn = DateTime.Now.AddHours(1),
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUsername = "onuronur"
            };

            context.EvernoteUsers.Add(admin);
            context.EvernoteUsers.Add(standartUser);

            for (int i = 0; i < 8; i++)
            {
                EvernoteUser user = new EvernoteUser()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    Surname = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    ProfileImageFilename = "user_boy.png",
                    ActiveGuid = Guid.NewGuid(),
                    IsActive = true,
                    IsAdmin = false,
                    Username = $"user{i}",
                    //$"user{i}" yeni string formatıdır ve bir değişkeni veya kod bütününü çağırabiliyorsunuz. 
                    Password = "123",
                    CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1),DateTime.Now),
                    ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedUsername = $"user{i}"
                };

                context.EvernoteUsers.Add(user);
            }

            context.SaveChanges();

            //User list for using.
            List<EvernoteUser> userlist = context.EvernoteUsers.ToList();

            //Fake Kategori Ekleme.
            for (int i = 0; i < 10; i++)
            {
                Category cat = new Category()
                {
                    Title = FakeData.PlaceData.GetStreetName(),
                    Description = FakeData.PlaceData.GetAddress(),
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    ModifiedUsername = "onuronur"
                };

                context.Categories.Add(cat);

                //Fake Not Ekleme.
                for (int k = 0; k < FakeData.NumberData.GetNumber(5,9); k++)
                {
                    EvernoteUser owner = userlist[FakeData.NumberData.GetNumber(0, userlist.Count - 1)];

                    string title = FakeData.TextData.GetSentence();
                    if (title.Length > 50) 
                    {
                        title = title.Substring(0, 50);
                    }
                    Note note = new Note()
                    {
                        Title = title,
                        Text = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1,3)),
                        IsDraft = false, //Taslak mı? Değil.
                        LikeCount = FakeData.NumberData.GetNumber(1,9),
                        Owner = owner, // çift sayılarda admin aksi halde standartUser'ı ver.
                        CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1),DateTime.Now),
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedUsername = owner.Username,
                    };

                    cat.Notes.Add(note);

                    //Fake Yorum Ekleme.
                    for (int j = 0; j < FakeData.NumberData.GetNumber(3,5); j++)
                    {
                        EvernoteUser comment_owner = userlist[FakeData.NumberData.GetNumber(0, userlist.Count - 1)];

                        Comment comment = new Comment()
                        {
                            Text = FakeData.TextData.GetSentence(),
                            Owner = comment_owner,
                            CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedUsername = comment_owner.Username
                        };

                        note.Comments.Add(comment);
                    }

                    // Fake Beğeni Ekleme.

                    for (int m = 0; m < note.LikeCount ; m++)
                    {
                        Liked liked = new Liked()
                        {
                            LikedUser = userlist[m]
                        };

                        note.Likes.Add(liked);
                    }
                }
            }

            context.SaveChanges();
        }
    }
}
