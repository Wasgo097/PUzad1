using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS
{
    public class AddBookCommandHandler : ICommandHandler<AddBookCommand>
    {
        private Database db { get; }
        public AddBookCommandHandler(Database db)
        {
            this.db = db;
        }
        public void Handle(AddBookCommand command)
        {
            Book newbook = new Book
            {
                Title = command.Title,
                ReleaseDate = command.ReleaseDate,

            };
            newbook.Authors = db.Authors.Where(a => command.AuthorsIds.Contains(a.Id)).ToList();
            db.Books.Add(newbook);
            db.SaveChanges();
        }
    }
}
