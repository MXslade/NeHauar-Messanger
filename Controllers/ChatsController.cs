using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NeHauar.DTO;
using NeHauar.Helpers;
using NeHauar.Models;
using NeHauar.Services;

namespace NeHauar.Controllers
{
    [Microsoft.AspNetCore.Authorization.Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ChatsController : ControllerBase
    {
        private IChatService _chatService;
        private IMapper _mapper;

        public ChatsController(IChatService chatService, IMapper mapper)
        {
            _chatService = chatService;
            _mapper = mapper;
        }

        [HttpGet("user-chats")]
        public IActionResult UserChats()
        {
            var claimsPrincipal = HttpContext.User;
            var userId = claimsPrincipal.FindFirstValue(ClaimTypes.Name);
            return Ok(_chatService.GetAllUserChat(int.Parse(userId)));
        }

        [HttpPost("")]
        public IActionResult CreateChat(ChatDto chatDto)
        {
            var chat = _mapper.Map<Chat>(chatDto);
            try
            {
                _chatService.Create(chat);
                return Ok();
            }
            catch (AppException e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
        
    }
}