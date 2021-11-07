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
    public class LibraryController : ControllerBase
    {
        private LibraryRepository repo;

        public LibraryController(LibraryRepository rep)
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
        [HttpPost("/addbook")]
        public void PostBook([FromBody]BookRequestDTO book)
        {
            repo.PostBook(book);
        }
        [HttpDelete("{id}/deletebook")]
        public void DeleteBook(int id)
        {
            repo.DeleteBook(id);
        }
        [HttpPost("/ratebook")]
        public void RateBook([FromBody]int rate,int id)
        {
            repo.AddRateToBook(rate, id);
        }

        ////authors


        [HttpGet("/authors")]
        public IEnumerable<AuthorDTO> GetAuthors([FromQuery] PaginationDTO pagination)
        {
            return repo.GetAuthors(pagination);
        }
        [HttpPost("/addauthor")]
        public void PostAuthor(AuthorRequestDTO author)
        {
            repo.PostAuthor(author);
        }
        [HttpDelete("{id}/deleteauthor")]
        public void DeleteAuthor(int id)
        {
           repo.DeleteAuthor(id);
        }
        [HttpPost("/rateauthor")]
        public void RateAuthor([FromBody] int rate, int id)
        {
            repo.AddRateToAuthor(rate, id);
        }
    }
}
