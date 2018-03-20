using Naylah.Core.Commands.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naylah.Core.Services.External
{
    public interface IEmailAppService
    {
        Task<bool> SendEmail(SendEmailCommand command);

    }
}
