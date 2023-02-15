using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CSForum.WebUI.Controllers;

public class ChatController:Controller
{
    public ChatController()
    {
            
    }
    [Authorize, Route("")]
    public async Task<IActionResult> Chat()
    {
        return View("Chat");
    }
}