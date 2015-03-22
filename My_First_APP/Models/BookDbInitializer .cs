using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
namespace My_First_APP.Models
{
    public class BookDbInitializer : DropCreateDatabaseAlways<BookContext>
    {
        protected override void Seed(BookContext db)
        {
            db.Books.Add(new Book { Login = "Война и мир", Author = "Л. Толстой", Price = 220 });
            db.Books.Add(new Book { Login = "Отцы и дети", Author = "И. Тургенев", Price = 180 });
            db.Books.Add(new Book { Login = "Чайка", Author = "А. Чехов", Price = 150 });

            base.Seed(db);
        }
    }
}