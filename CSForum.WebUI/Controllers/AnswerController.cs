using AutoMapper;
using CSForum.Core.IHttpClients;
using CSForum.Core.Models;
using CSForum.Infrastructure.MapperConfigurations;
using CSForum.Services.HttpClients;
using CSForum.Services.MapperConfigurations;
using CSForum.Shared.Models.dtoModels;
using CSForum.Shared.Models.dtoModels.Tags;
using CSForum.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CSForum.WebUI.Controllers;

public class AnswerController:Controller
{
    private readonly IMapper _mapper;
    private readonly ApiHttpClientBase _forumClient;
    private readonly UserManager<User> _userManager;
    public AnswerController(ApiHttpClientBase client, UserManager<User> userManager)
    {
        _forumClient = client;
        _userManager = userManager;
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
            var answer = await _forumClient.PostAsync<CreateAnswerDto, Answer>(answerDto, "api/answers/create");
            return Redirect($"/post/post/{answer.PostId}");
        }
        catch(Exception e)
        {
            throw new Exception(e.Message, e);
        }
    }
}