using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using SignalRCore.Chat.Mvc.Data;
using SignalRCore.Chat.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRCore.Chat.Mvc.Hubs
{
    public class ChatHub : Hub
    {
        private readonly UserManager<ChatUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ChatHub(ApplicationDbContext context, UserManager<ChatUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task BroadcastFromClient(string message)
        {
            try
            {
                var currentUser = await _userManager.GetUserAsync(Context.User);
                String userEmailFirstSection = currentUser.Email.Split('@').First();
                Message m = new Message()
                {
                    MessageBody = message,
                    MessageDtTm = DateTime.Now,
                    FromUser = currentUser
                };

                _context.Messages.Add(m);
                await _context.SaveChangesAsync();

                await Clients.All.SendAsync(
                    "Broadcast",
                    new
                    {

                        messageBody = m.MessageBody,
                        fromUser = userEmailFirstSection,
                        messageDtTm = m.MessageDtTm.ToString(
                                "hh:mm tt MMM dd", CultureInfo.InvariantCulture
                            )
                    });
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("HubError", new { error = ex.Message });
            }
        }

        public override async Task OnConnectedAsync()
        {
            var currentUser = await _userManager.GetUserAsync(Context.User);
            String userEmailFirstSection = currentUser.Email.Split('@').First();
            await Clients.All.SendAsync(
                "UserConnected",
                new
                {
                    userId = userEmailFirstSection,
                    connectionId = Context.ConnectionId,
                    connectionDtTm = DateTime.Now,
                    messageDtTm = DateTime.Now.ToString(
                                "hh:mm tt MMM dd", CultureInfo.InvariantCulture
                            )
                }) ;
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            var currentUser = await _userManager.GetUserAsync(Context.User);
            String userEmailFirstSection = currentUser.Email.Split('@').First();

            await Clients.All.SendAsync("UserDisconnected",
                new { 
                elementId=userEmailFirstSection,
                message=$"User disconnected, user id: {userEmailFirstSection}",
                    messageDtTm = DateTime.Now.ToString(
                                "hh:mm tt MMM dd", CultureInfo.InvariantCulture
                            )
                }
                );
        }
    }
}
