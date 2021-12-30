using Microsoft.EntityFrameworkCore;
using Model;
using Model.DTO;
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
                var booktoindex=db.Books.
                   Include(b => b.Rates).
                   Include(b => b.Authors).
                   ToList().Select
                   (b => new BookDTO
                   {
                       Id = b.Id,
                       ReleaseDate = b.ReleaseDate,
                       AvarageRate = b.Rates.Count > 0 ? b.Rates.Average(r => r.Value) : 0,
                       RatesCount = b.Rates.Count(),
                       Title = b.Title,
                       Authors = b.Authors.Select(a => new BookAuthorDTO
                       {
                           FirstName = a.FirstName,
                           Id = a.Id,
                           SecondName = a.SecondName
                       }).ToList()
                   }
                   ).Where(b => b.Id == command.id).Single();
                _elasticClient.Update<BookDTO>(command.id, s => s.Index("books_index").Doc(booktoindex));
            }
        }
    }
}
