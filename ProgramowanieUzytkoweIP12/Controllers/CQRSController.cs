using CQRS;
using Microsoft.AspNetCore.Mvc;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgramowanieUzytkoweIP12.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CQRSController : ControllerBase
    {
        private readonly CommandBus commandBus;
        private readonly QueryBus querybus;
        public CQRSController(CommandBus commandBus,QueryBus querybus)
        {
            this.commandBus= commandBus;
            this.querybus = querybus;
        }
        [HttpPost]
        public void Post([FromBody] AddBookCommand command)
        {
            commandBus.Handle(command);
        }
        [HttpDelete("{id}")]
        public void Delete(int id)
        {

            commandBus.Handle(new DeleteBookCommand(id));
        }
        [HttpGet]
        public List<BookDTO> GetBooks([FromQuery]GetBooksQuery query)
        {
            return querybus.Handle<GetBooksQuery, List<BookDTO>>(query);
        }
    }
}
