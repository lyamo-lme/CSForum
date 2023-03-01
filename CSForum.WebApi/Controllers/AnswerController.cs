using AutoMapper;
using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using CSForum.Infrastructure.MapperConfigurations;
using CSForum.Services.Extensions;
using CSForum.Services.MapperConfigurations;
using CSForum.Shared.Models.dtoModels;
using CSForum.Shared.Models.dtoModels.Answer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CSForum.WebApi.Controllers;

[ApiController]
[Route("api/answers")]
public class AnswerController : Controller
{
    private readonly IUnitOfWorkRepository _uofRepository;
    private readonly IMapper _dtoMapper;
    private readonly ILogger<AnswerController> _logger;
    private readonly UserManager<User> _userManager;

    public AnswerController(IUnitOfWorkRepository uofRepository, UserManager<User> userManager,
        ILogger<AnswerController> logger)
    {
        _uofRepository = uofRepository;
        _userManager = userManager;
        _logger = logger;
        _dtoMapper = MapperFactory.CreateMapper<DtoMapper>();
    }

    [HttpPost, Route(""), Authorize]
    public async Task<ActionResult<Answer>> CreateAnswer([FromBody] CreateAnswerDto model)
    {
        try
        {
            var mappedPost = _dtoMapper.Map<Answer>(model);
            var post = await _uofRepository.GenericRepository<Answer>().CreateAsync(mappedPost);
            await _uofRepository.SaveAsync();
            return Ok(post);
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e, e.Message);
            throw;
        }
    }

    [HttpPut, Route("state"), Authorize]
    public async Task<ActionResult<Answer>> UpdateState([FromBody] EditAnswerDto answerDto)
    {
        try
        {
            var answer = await _uofRepository.GenericRepository<Answer>().FindAsync(x => x.Id == answerDto.Id);
            var post = await _uofRepository.GenericRepository<Post>().FindAsync(x => x.Id == answer.PostId);
            if (post.UserId != _userManager.GetId(User))
            {
                return BadRequest();
            }

            var mappedAnswer = _dtoMapper.Map<Answer>(answerDto);
            var updAnswer = await _uofRepository.GenericRepository<Answer>().UpdateAsync(mappedAnswer);
            await _uofRepository.SaveAsync();
            return Ok(updAnswer);
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e, e.Message);
            return BadRequest();
        }
    }

    [HttpGet, Route("{Id}")]
    public async Task<ActionResult<Answer>> GetAnswer(int id)
    {
        try
        {
            var answer = await _uofRepository.GenericRepository<Answer>().FindAsync(x => x.Id == id);
            return Ok(answer);
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e, e.Message);
            return BadRequest();
        }
    }
    [HttpGet, Route("user/{userId}")]
    public async Task<ActionResult<List<Answer>>> GetUsersAnswer(int userId)
    {
        try
        {
            var answer = await _uofRepository.GenericRepository<Answer>().GetAsync(x => x.UserId == userId);
            return Ok(answer);
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e, e.Message);
            return BadRequest();
        }
    }
}