using Model;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Authors
{
    public class AddAuthorCommandHandler: ICommandHandler<AddAuthorCommand>
    {
        private Database db { get; }
        private readonly IElasticClient _elasticClient;

        public AddAuthorCommandHandler(Database db, IElasticClient client)
        {
            this.db = db;
            _elasticClient = client;
        }

        public void Handle(AddAuthorCommand command)
        {
            Author author = new Author
            {
                FirstName = command.FirstName,
                SecondName = command.SecondName

            };
            author.Books = db.Books.Where(a => command.BooksIDs.Contains(a.Id)).ToList();
            db.Authors.Add(author);
            db.SaveChanges();
        }
    }
}
