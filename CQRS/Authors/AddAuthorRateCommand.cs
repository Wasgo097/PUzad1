using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Authors
{
    public record AddAuthorRateCommand(int id, int rate) : ICommand;
}
