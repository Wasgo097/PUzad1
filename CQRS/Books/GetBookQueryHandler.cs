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
            var x = _elasticClient.Get<BookDTO>(query.id).Source;
            return x;
        }
    }
}
