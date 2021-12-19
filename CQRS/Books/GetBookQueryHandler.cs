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
    public class GetBookQueryHandler : IQueryHandler<GetBookQuery, BookDTO>
    {
        private readonly Database db;
        private readonly IElasticClient _elasticClient;
        public GetBookQueryHandler(Database db, IElasticClient client)
        {
            this.db = db;
            _elasticClient = client;
        }
        public BookDTO Handle(GetBookQuery query)
        {
            return db.Books.
                   Include(b => b.Rates).
                   Include(b => b.Authors).
                   ToList().Select(b => new BookDTO
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
                   }).Where(b => b.Id == query.id).Single();
        }
    }
}
