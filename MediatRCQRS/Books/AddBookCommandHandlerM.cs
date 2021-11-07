using MediatR;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MediatRCQRS
{
    public class AddBookCommandHandlerM : IRequestHandler<AddBookCommandM, bool>
    {
        private Database db { get; }

        public AddBookCommandHandlerM(Database db)
        {
            this.db = db;
        }

        public Task<bool> Handle(AddBookCommandM command, CancellationToken token)
        {
            Book book = new Book
            {
                Title = command.Title,
                ReleaseDate = command.ReleaseDate

            };
            book.Authors = db.Authors.Where(a => command.AuthorsIDs.Contains(a.Id)).ToList();
            db.Books.Add(book);
            db.SaveChanges();

            return Task.FromResult(true);
        }
    }
}
