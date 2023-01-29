using AutoMapper;
using CSForum.Core.IHttpClients;
using CSForum.Core.Models;
using CSForum.Services.MapperConfigurations;
using CSForum.Shared.Models.dtoModels;
using CSForum.Shared.Models.dtoModels.Tag;
using CSForum.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CSForum.WebUI.Controllers;

public class AnswerController:Controller
{
    private readonly IMapper _mapper;
    private readonly IForumClient _forumClient;
    public AnswerController(IForumClient client)
    {
        _forumClient = client;
        _mapper = MapperFactory.CreateMapper<DtoMapper>();
    }
    [HttpPost]
    public async Task<ActionResult> CreateAnswer(CreateAnswerViewModel model)
    {
        try
        {
            var answerDto = _mapper.Map<CreateAnswerDto>(model);
            var answer = await _forumClient.PostAsync<CreateAnswerDto, Answer>(answerDto, "/api/answers/create");
            return Redirect($"Post/Post/{answer.PostId}");
        }
        catch(Exception e)
        {
            throw new Exception(e.Message, e);
        }
    }
}