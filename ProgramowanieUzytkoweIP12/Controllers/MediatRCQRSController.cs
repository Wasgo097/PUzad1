using MediatR;
using MediatRCQRS;
using Microsoft.AspNetCore.Mvc;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgramowanieUzytkoweIP12.Controllers
{
    public class MediatRCQRSController : Controller
    {
        private readonly IMediator mediator;

        public MediatRCQRSController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("/mbooks")]
        public Task<List<BookDTO>> GetBooks([FromQuery] GetBooksQueryM query)
        {
            return mediator.Send(query);
        }

        [HttpGet("/mbook")]
        public Task<BookDTO> GetBook([FromQuery] GetBookQueryM query)
        {
            return mediator.Send(query);
        }

        [HttpPost("/maddbook")]
        public Task<bool> PostBook([FromBody] AddBookCommandM command)
        {
            return mediator.Send(command);
        }

        [HttpDelete("{id}/mdeletebook")]
        public Task<bool> DeleteBook(int id)
        {
            return mediator.Send(new DeleteBookCommandM(id));
        }

        [HttpPost("/mratebook")]
        public Task<bool> PostBookRate([FromBody] int id, int rate)
        {
            return mediator.Send(new AddBookRateCommandM(id, rate));
        }

        //authors

        [HttpGet("/mauthors")]
        public Task<List<AuthorDTO>> GetAuthors([FromQuery] GetAuthorsQueryM query)
        {
            return mediator.Send(query);
        }

        [HttpPost("/maddauthors")]
        public Task<bool> PostAuthor([FromBody] AddAuthorCommandM command)
        {
            return mediator.Send(command);
        }

        [HttpDelete("{id}/mdeleteauthor")]
        public Task<bool> DeleteAuthor(int id)
        {
            return mediator.Send(new DeleteAuthorCommandM(id));
        }

        [HttpPost("/mrateauthor")]
        public Task<bool> PostAuthorRate([FromBody] int id, int rate)
        {
            return mediator.Send(new AddAuthorRateCommandM(id, rate));
        }
    }
}
