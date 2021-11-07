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
    public class AddAuthorCommandHandlerM : IRequestHandler<AddAuthorCommandM, bool>
    {
        private Database db { get; }

        public AddAuthorCommandHandlerM(Database db)
        {
            this.db = db;
        }

        public Task<bool> Handle(AddAuthorCommandM command, CancellationToken token)
        {
            Author aut = new Author
            {
                FirstName = command.FirstName,
                SecondName = command.SecondName

            };
            aut.Books = db.Books.Where(a => command.BooksIDs.Contains(a.Id)).ToList();
            db.Authors.Add(aut);
            db.SaveChanges();

            return Task.FromResult(true);
        }

    }
}
