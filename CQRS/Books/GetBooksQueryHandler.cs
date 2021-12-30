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
    public class GetBooksQueryHandler : IQueryHandler<GetBooksQuery, List<BookDTO>>
    {
        private readonly Database db;
        private readonly IElasticClient _elasticClient;
        public GetBooksQueryHandler(Database db, IElasticClient client)
        {
            this.db = db;
            _elasticClient = client;
        }
        public List<BookDTO> Handle(GetBooksQuery query)
        {
            var x=_elasticClient.Search<BookDTO>(x => x.Size(query.Count).Skip(query.Count * query.Page)).Documents.ToList();
            return x;
        }
    }
}
