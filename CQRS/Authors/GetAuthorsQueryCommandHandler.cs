using Microsoft.EntityFrameworkCore;
using Model;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Authors
{
    public class GetAuthorsQueryCommandHandler: IQueryHandler<GetAuthorsQueryCommand, List<AuthorDTO>>
    {
        private readonly Database db;

        public GetAuthorsQueryCommandHandler(Database db)
        {
            this.db = db;
        }
        public List<AuthorDTO> Handle(GetAuthorsQueryCommand query)
        {
            return db.Authors.Include(a => a.Rates)
            .Include(a => a.Books)
            .Skip(query.Page * query.Count)
            .Take(query.Count)
            .ToList().Select(a => new AuthorDTO
            {
                Id = a.Id,
                FirstName = a.FirstName,
                SecondName = a.SecondName,
                AvarageRate = (a.Rates.Count() > 0 ? a.Rates.Average(r => r.Value) : 0),
                RatesCount = (a.Rates.Count() > 0 ? a.Rates.Count() : 0),
                Books = a.Books.Select(b => new SlimBookDTO
                {
                    Title = b.Title,
                    Id = b.Id,
                }).ToList()
            }).ToList();
        }
    }
}
