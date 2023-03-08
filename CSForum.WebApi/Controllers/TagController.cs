using AutoMapper;
using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using CSForum.Data.Repositories;
using CSForum.Infrastructure.MapperConfigurations;
using CSForum.Shared.Models.dtoModels.Tags;
using Microsoft.AspNetCore.Mvc;

namespace CSForum.WebApi.Controllers
{
    [ApiController]
    [Route("api/tags")]
    public class TagController : Controller
    {
        private readonly IUnitOfWorkRepository _uofRepository;
        private readonly IMapper _dtoMapper;

        public TagController(IUnitOfWorkRepository uofRepository)
        {
            _uofRepository = uofRepository;
            _dtoMapper = MapperFactory.CreateMapper<DtoMapper>();
        }

        [HttpPost, Route("create")]
        public async Task<ActionResult<Tag>> CreateTag([FromBody] CreateTagDto model)
        {
            try
            {
                var mappedPost = _dtoMapper.Map<Tag>(model);
                var tag = await _uofRepository.GenericRepository<Tag>().CreateAsync(mappedPost);
                await _uofRepository.SaveAsync();
                return Ok(tag);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        [HttpPost, Route("edit")]
        public async Task<ActionResult<Tag>> EditTag([FromBody] EditTagDto model)
        {
            try
            {
                var tag = await _uofRepository.GenericRepository<Tag>().UpdateAsync(_dtoMapper.Map<Tag>(model));
                await _uofRepository.SaveAsync();
                return Ok(tag);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        [HttpGet, Route("tagId/{tagId}")]
        public async Task<ActionResult<Tag>> FindTag(int tagId)
        {
            try
            {
                return Ok(await _uofRepository.GenericRepository<Tag>().FindAsync(x => x.Id == tagId));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        [HttpGet, Route("{name}")]
        public async Task<ActionResult<Tag>> FindByName(string name, int? skip = null, int? take = null)
        {
            try
            {
                return Ok(await _uofRepository.GenericRepository<Tag>().GetAsync(
                    x => x.Name.Contains(name),
                    skip: skip,
                    take: take
                ));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        [HttpDelete, Route("delete")]
        public async Task<ActionResult<bool>> DeleteTag(int tagId)
        {
            try
            {
                var state = await _uofRepository.GenericRepository<Tag>().DeleteAsync(new Tag()
                {
                    Id = tagId
                });
                await _uofRepository.SaveAsync();
                return Ok(state);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
        [HttpGet]
        [Route("count/{tagId}")]
        public async Task<ActionResult<Tag>> PostsCount(int tagId)
        {
            try
            {
                return Ok((await _uofRepository
                    .GenericRepository<PostTag>()
                    .GetAsync(x=>x.TagId==tagId))
                    .Count());
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
        [HttpGet]
        public async Task<ActionResult<Tag>> Get(int? skip = null, int? take = null)
        {
            try
            {
                return Ok(await _uofRepository.GenericRepository<Tag>().GetAsync(
                    skip: skip,
                    take: take
                ));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
    }
}