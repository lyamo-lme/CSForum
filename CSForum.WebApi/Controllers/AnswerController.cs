using AutoMapper;
using CSForum.Core.IRepositories;
using CSForum.Core.Models;
using CSForum.Infrastructure.MapperConfigurations;
using CSForum.Services.MapperConfigurations;
using CSForum.Shared.Models.dtoModels;
using Microsoft.AspNetCore.Mvc;

namespace CSForum.WebApi.Controllers;

[ApiController]
[Route("api/answers")]
public class AnswerController:Controller
{
    private readonly IUnitOfWorkRepository _uofRepository;
    private readonly IMapper _dtoMapper;

    public AnswerController(IUnitOfWorkRepository uofRepository)
    {
        _uofRepository = uofRepository;
        _dtoMapper = MapperFactory.CreateMapper<DtoMapper>();
    }
    [HttpPost, Route("")]
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
            throw;
        }
    }
    // [HttpPost, Route("state")]
    // public async Task<ActionResult<Answer>> UpdateState([FromBody]EditAn)
    // {
    //     try
    //     {
    //         var mappedPost = _dtoMapper.Map<Answer>(model);
    //         var post = await _uofRepository.GenericRepository<Answer>().CreateAsync(mappedPost);
    //         await _uofRepository.SaveAsync();
    //         return Ok(post);
    //     }
    //     catch (Exception e)
    //     {
    //         throw;
    //     }
    // }
}