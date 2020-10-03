using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SendInvite.DTOs;
using SendInvite.Implementation;
using SendInvite.Interface;

namespace SendInvite.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SendInviteController : ControllerBase
    {

        private readonly ILogger<SendInviteController> _logger;

        public SendInviteController(ILogger<SendInviteController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Result(SendInvitesDTO data)
        {
            return new SendInvites().Result(data.Numbers, data.Messages);
        }
    }
}
