using AutoMapper;
using CSForum.Core.Models;
using CSForum.Infrastructure.MapperConfigurations;
using CSForum.Services.Extensions;
using CSForum.Services.Http;
using CSForum.Services.MapperConfigurations;
using CSForum.Shared.Models.dtoModels;
using CSForum.WebUI.Models;
using CSForum.WebUI.Services.HttpClients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CSForum.WebUI.Controllers;

public class AnswerController:Controller
{
    private readonly IMapper _mapper;
    private readonly ApiHttpClientBase _forumClient;
    private readonly ILogger<AnswerController> _logger;
    private readonly UserManager<User> _userManager;
    public AnswerController(ApiHttpClientBase client, UserManager<User> userManager, ILogger<AnswerController> logger)
    {
        _forumClient = client;
        _userManager = userManager;
        _logger = logger;
        _mapper = MapperFactory.CreateMapper<DtoMapper>();
    }
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> CreateAnswer(CreateAnswerViewModel model)
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);
            var answerDto = _mapper.Map<CreateAnswerDto>(model);
            answerDto.UserId = user.Id;        
            var answer = await _forumClient.PostAsync<CreateAnswerDto, Answer>(answerDto, "api/answers");
            return Redirect($"/post/{answer.PostId}");
        }
        catch(Exception e)
        {
            _logger.Log(LogLevel.Error,e, e.Message);
            throw ;
        }
    }
    // [HttpPost,Route("state")]
    // [Authorize]
    // public async Task<ActionResult> UpdateState([FromBody]int answerId)
    // {
    //     try
    //     {
    //         var userId =  _userManager.GetId(User);
    //         var result = _forumClient.
    //         return Redirect($"/post/{answer.PostId}");
    //     }
    //     catch(Exception e)
    //     {
    //         _logger.Log(LogLevel.Error,e, e.Message);
    //         throw ;
    //     }
    // }
}