using Microsoft.AspNetCore.Mvc;
using SendInvite.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SendInvite.Interface
{
    public interface ISendInvites
    {
        public ObjectResult Result(string[] numbers, string message);
    }
}
