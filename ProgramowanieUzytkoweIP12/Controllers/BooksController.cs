using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.DTO;
using RepositoryPattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgramowanieUzytkoweIP12.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private BooksRepository repo;

        public BooksController(BooksRepository rep)
        {
            repo = rep;
        }

        [HttpGet("/books")]
        public IEnumerable<BookDTO> GetBooks([FromQuery] PaginationDTO pagination)
        {
            return repo.GetBooks(pagination);
        }
        [HttpGet("/book")]
        public BookDTO GetBook(int id)
        {
            return repo.GetBook(id);
        }
        //edit
        [HttpPut("{id}")]
        public BookDTO Put(int id, [FromBody] BookRequestDTO book)
        {
            //return repo.GetBooks();
            return null;
        }
        //add
        [HttpPost]
        public void Post(BookRequestDTO book)
        {
            repo.PostBook(book);
        }
        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            var result=repo.DeleteBook(id);
            return result;
        }
    }
}
