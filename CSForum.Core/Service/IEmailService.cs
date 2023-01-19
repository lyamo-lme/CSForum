using CSForum.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSForum.Core.Service
{
    public interface IEmailService
    {
        public Task SendMessage<T>(T mail) where T:Email ;
    }
}
