using MyEvernote.BusinessLayer.Abstract;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLayer
{
    public class CategoryManager : ManagerBase<Category>
    {
        public override int Delete(Category category)
        {
            NoteManager noteManager = new NoteManager();
            LikedManager likedManager = new LikedManager();
            CommentManager commentManager = new CommentManager();
            // Kategori ile ilişkili notların silinmesi gerekiyor.
            foreach (Note note in category.Notes.ToList())
            {
                // Note ile ilişkili like'ların silinmesi.
                foreach (Liked like in note.Likes.ToList())
                {
                    likedManager.Delete(like);
                }

                // Note ile ilişkili comment'lerin silinmesi
                foreach (Comment comment in note.Comments.ToList())
                {
                    commentManager.Delete(comment); 
                }

                noteManager.Delete(note);
            }
            // Base görev çağrılarak devam ettirilecek işlem eğer istemeseydik değiştirebiliriz.
            return base.Delete(category);
        } 
    }
}
