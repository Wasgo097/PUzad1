using MediatR;
using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MediatRCQRS
{
    public class DeleteAuthorCommandHandlerM : IRequestHandler<DeleteAuthorCommandM, bool>
    {
        private readonly Database db;

        public DeleteAuthorCommandHandlerM(Database db)
        {
            this.db = db;
        }

        public Task<bool> Handle(DeleteAuthorCommandM command, CancellationToken token)
        {
            Author del = db.Authors.Include(x => x.Books).Where(x => x.Id == command.id).FirstOrDefault();

            if (del.Books.Any() == false)
            {
                db.Authors.Remove(del);
                db.SaveChanges();
            }

            return Task.FromResult(true);

        }
    }
}
