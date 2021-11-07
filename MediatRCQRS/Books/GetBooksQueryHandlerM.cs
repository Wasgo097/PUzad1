using MediatR;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MediatRCQRS
{
    public class GetBooksQueryHandlerM : IRequestHandler<GetBooksQueryM,List<BookDTO>>
    {
        private readonly Database db;

        public GetBooksQueryHandlerM(Database db)
        {
            this.db = db;
        }

        public Task<List<BookDTO>> Handle(GetBooksQueryM query, CancellationToken token)
        {
            return Task.FromResult(db.Books.
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
                }).ToList());
        }

    }
}
