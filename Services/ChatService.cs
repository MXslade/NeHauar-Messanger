using System;
using System.Collections.Generic;
using System.Linq;
using NeHauar.Contexts;
using NeHauar.Models;

namespace NeHauar.Services
{
    public interface IChatService
    {
        IEnumerable<Chat> GetAllUserChat(int userId);
        Chat Create(Chat chat);
    }
    
    public class ChatService : IChatService
    {
        private DataContext _dataContext;

        public ChatService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        
        public IEnumerable<Chat> GetAllUserChat(int userId)
        {
            var chats = _dataContext.Chats.Where(x => x.FirstUserId == userId || x.SecondUserId == userId);
            foreach (var chat in chats)
            {
                chat.FirstUser = _dataContext.Users.FirstOrDefault(x => x.Id == chat.FirstUserId);
                chat.SecondUser = _dataContext.Users.FirstOrDefault(x => x.Id == chat.SecondUserId);
            }
            return chats.AsEnumerable();
        }

        public Chat Create(Chat chat)
        {
            chat.FirstUser = _dataContext.Users.FirstOrDefault(x => x.Id == chat.FirstUserId);
            chat.SecondUser = _dataContext.Users.FirstOrDefault(x => x.Id == chat.SecondUserId);

            _dataContext.Chats.Add(chat);
            _dataContext.SaveChanges();

            return chat;
        }
    }
}