using Model;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nest;

namespace CQRS
{
    public class GetBooksQueryHandler : IQueryHandler<GetBooksQuery, List<BookDTO>>
    {
        private readonly Database db;
        private readonly IElasticClient _elasticClient;
        public GetBooksQueryHandler(Database db, IElasticClient client)
        {
            this.db = db;
            _elasticClient = client;
        }
        public List<BookDTO> Handle(GetBooksQuery query)
        {
            return db.Books.
                Include(b => b.Rates).
                Include(b => b.Authors).
                Skip(query.Count * query.Page).
                Take(query.Count).
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
                }).ToList();
        }
    }
}
