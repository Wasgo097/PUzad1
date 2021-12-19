using Model.DTO;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramowanieUzytkoweIP12
{
    public class ElasticConnection : ConnectionSettings
    {
        public ElasticConnection(Uri uri = null) : base(uri)
        {
            DefaultMappingFor<AuthorDTO>(x => x.IndexName("authors_index"));
            DefaultMappingFor<BookDTO>(x => x.IndexName("books_index"));
        }
    }
}
