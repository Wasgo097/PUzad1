using Model;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Books
{
    public class AddBookRateCommandHandler : ICommandHandler<AddBookRateCommand>
    {
        private Database db { get; }
        private readonly IElasticClient _elasticClient;

        public AddBookRateCommandHandler(Database db, IElasticClient client)
        {
            this.db = db;
            _elasticClient = client;
        }
        public void Handle(AddBookRateCommand command)
        {
            Book book = db.Books.Where(x => x.Id == command.id).Single();
            if (book!=null)
            {
                db.BooksRates.Add(new BookRate
                {
                    Value = (short)command.rate,
                    Date = DateTime.Now,
                    Type = RateType.BookRate,
                    FkBook = book.Id,
                    Book = book
                });
                db.SaveChanges();
            }
        }
    }
}
