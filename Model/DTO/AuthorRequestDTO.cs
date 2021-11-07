using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTO
{
    public class AuthorRequestDTO
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public List<int> BooksId { get; set; }
    }
}
