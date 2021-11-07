using Model;
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

        public AddAuthorRateCommandHandler(Database db)
        {
            this.db = db;
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
        }
    }
}
