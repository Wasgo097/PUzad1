using Microsoft.EntityFrameworkCore;
using Model;
using Model.DTO;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Authors
{
    public class AddAuthorRateCommandHandler:ICommandHandler<AddAuthorRateCommand>
    {
        private Database db { get; }
        private readonly IElasticClient _elasticClient;

        public AddAuthorRateCommandHandler(Database db, IElasticClient client)
        {
            this.db = db;
            _elasticClient = client;
        }

        public void Handle(AddAuthorRateCommand command)
        {
            Author author = db.Authors.Where(x => x.Id == command.id).FirstOrDefault();

            db.AuthorsRates.Add(new AuthorRate
            {
                Type = RateType.AuthorRate,
                Author = author,
                FkAuthor = author.Id,
                Date = DateTime.Now,
                Value = (short)command.rate
            });
            db.SaveChanges();
            var authortoindex= db.Authors.Include(a => a.Rates)
            .Include(a => a.Books)
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
            }).Where(b=>b.Id==command.id).Single();
            _elasticClient.IndexDocument<AuthorDTO>(authortoindex);
        }
    }
}
