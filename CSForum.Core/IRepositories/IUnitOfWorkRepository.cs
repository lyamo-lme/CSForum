using CSForum.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSForum.Core.IRepositories
{
    public interface IUnitOfWorkRepository
    {
        IRepository<Tag> Tags { get; }
        IRepository<Post> Posts { get; }
        IRepository<User> Users { get; }
        IRepository<Answer> Answers { get; }
        IRepository<PostTag> PostTags { get; }
        Task SaveAsync();
    }
}
