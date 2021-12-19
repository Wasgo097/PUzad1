using Model;
using Model.DTO;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS
{
    public class DeleteBookCommandHandler : ICommandHandler<DeleteBookCommand>
    {
        private readonly Database db;
        private readonly IElasticClient _elasticClient;
        public DeleteBookCommandHandler(Database db, IElasticClient client)
        {
            this.db = db;
            _elasticClient = client;
        }

        public void Handle(DeleteBookCommand command)
        {
            Book book = db.Books.Find(command.id);
            if(book !=null)
                db.Books.Remove(book);
            db.SaveChanges();
            _elasticClient.Delete<BookDTO>(command.id);
        }
    }
}
