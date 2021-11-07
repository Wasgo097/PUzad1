using MediatR;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks; 

namespace MediatRCQRS
{
    public class DeleteBookCommandHandlerM : IRequestHandler<DeleteBookCommandM, bool>
    {
        private readonly Database db;

        public DeleteBookCommandHandlerM(Database db)
        {
            this.db = db;
        }

        public Task<bool> Handle(DeleteBookCommandM command, CancellationToken token)
        {
            Book del = db.Books.Where(x => x.Id == command.id).FirstOrDefault();

            db.Books.Remove(del);
            db.SaveChanges();

            return Task.FromResult(true);
        }
    }
}
