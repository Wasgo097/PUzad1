using CQRS;
using CQRS.Authors;
using CQRS.Books;
using Microsoft.AspNetCore.Mvc;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgramowanieUzytkoweIP12.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CQRSController : ControllerBase
    {
        private readonly CommandBus commandBus;
        private readonly QueryBus querybus;
        public CQRSController(CommandBus commandBus,QueryBus querybus)
        {
            this.commandBus= commandBus;
            this.querybus = querybus;
        }
        [HttpGet("/cqrsbooks")]
        public IEnumerable<BookDTO> GetBooks([FromQuery] GetBooksQuery query)
        {
            return querybus.Handle<GetBooksQuery, List<BookDTO>>(query);
        }
        [HttpGet("/cqrsbook")]
        public List<BookDTO> GetBook([FromQuery] GetBookQuery query)
        {
            return querybus.Handle<GetBookQuery, List<BookDTO>>(query);
        }
        [HttpPost("/cqrsaddbook")]
        public void PostBook([FromBody] AddBookCommand command)
        {
            commandBus.Handle(command);
        }
        [HttpDelete("{id}/cqrsdeletebook")]
        public void DeleteBook(int id)
        {
            commandBus.Handle(new DeleteBookCommand(id));
        }
        [HttpPost("/cqrsratebook")]
        public void PostBookRate([FromBody] int id, int rate)
        {
            commandBus.Handle(new AddBookRateCommand(id, rate));
        }

        ///authors

        [HttpGet("/cqrsauthors")]
        public List<AuthorDTO> GetAuthors([FromQuery] GetAuthorsQuery query)
        {
            return querybus.Handle<GetAuthorsQuery, List<AuthorDTO>>(query);
        }

        [HttpPost("/cqrsaddauthor")]
        public void PostAuthor([FromBody] AddAuthorCommand command)
        {
            commandBus.Handle(command);
        }

        [HttpDelete("{id}/cqrsdeleteauthor")]
        public void DeleteAuthor(int id)
        {
            commandBus.Handle(new DeleteAuthorCommand(id));
        }

        [HttpPost("/cqrsrateauthor")]
        public void PostAuthorRate([FromBody] int id, int rate)
        {
            commandBus.Handle(new AddAuthorRateCommand(id, rate));
        }

    }
}
