using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Authors
{
    public class DeleteAuthorCommandHandler:ICommandHandler<DeleteAuthorCommand>
    {
        private readonly Database db;

        public DeleteAuthorCommandHandler(Database db)
        {
            this.db = db;
        }

        public void Handle(DeleteAuthorCommand command)
        {
            Author authortoremove = db.Authors
                .Include(a => a.Books)
                .Where(x => x.Id == command.id).Single();
            if (authortoremove!=null)
            {
                if (authortoremove.Books.Count>0)
                {
                    db.Authors.Remove(authortoremove);
                    db.SaveChanges();
                }
            }

        }
    }
}
