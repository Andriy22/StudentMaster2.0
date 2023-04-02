using backend.API.Hubs.Models;
using backend.DAL.Entities;
using backend.DAL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ChatMessage = backend.DAL.Entities.ChatMessage;

namespace backend.API.Hubs;

[EnableCors("signalr")]
public class ChatHub : Hub
{
    private readonly IRepository<Ban> _banRepository;
    private readonly IRepository<ChatMessage> _chatRepository;
    private readonly UserManager<User> _userManager;
    private readonly IRepository<User> _userRepository;

    public ChatHub(IRepository<User> userRepository, IRepository<ChatMessage> chatRepository,
        UserManager<User> userManager, IRepository<Ban> banRepository)
    {
        _userRepository = userRepository;
        _chatRepository = chatRepository;
        _userManager = userManager;
        _banRepository = banRepository;
    }

    [Authorize]
    public async Task SendToAsync(string message, string userId)
    {
        var user = await _userRepository.GetQueryable(x => x.Id == Context.User.Identity.Name).Include(x => x.Img)
            .FirstOrDefaultAsync();
        var role = await GetRoleAsync(user.Id);
        var color = "black";
        if (role == "Admin")
            color = "darkred";
        if (role == "Teacher")
            color = "mediumvioletred";

        var msg = new Models.ChatMessage
        {
            Message = message,
            Date = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"),
            OwnerId = userId,
            SenderAvatar = user.Img.Path,
            SenderId = user.Id,
            SenderFullName = $"{user.FirstName} {user.Name} {user.LastName}",
            Color = color
        };
        _chatRepository.Add(new ChatMessage
        {
            Color = msg.Color,
            Date = DateTime.Now,
            Message = msg.Message,
            OwnerId = msg.OwnerId,
            SenderId = msg.SenderId
        });
        await Clients.Caller.SendAsync("ReceiveMessage", msg);
        await Clients.User(userId).SendAsync("ReceiveMessage", msg);
        await GetContacts(userId);
        await GetContacts(Context.User.Identity.Name);
    }

    [Authorize]
    public async Task SwitchGroup(int category)
    {
        if (category < 1)
            category = _userRepository.GetQueryable(x => x.Id == Context.User.Identity.Name).Include(x => x.Group)
                .FirstOrDefault().Group.Id;

        await Groups.AddToGroupAsync(Context.ConnectionId, category.ToString());
    }

    [Authorize]
    public async Task UnBanUser(string uid)
    {
        var role = await GetRoleAsync(Context.User.Identity.Name);
        if (role == "Student")
        {
            await Clients.User(Context.User.Identity.Name).SendAsync("ReceiveMessage", new Models.ChatMessage
            {
                Color = "red",
                Date = DateTime.Now.ToShortDateString(),
                Message = "You dont have permisions!",
                SenderFullName = "StudentMasterBot",
                SenderId = "StudentMasterBot"
            });

            return;
        }

        var ban = _banRepository.GetQueryable(x => x.UserId == uid).FirstOrDefault();

        if (ban != null) _banRepository.Delete(ban);

        await Clients.User(Context.User.Identity.Name).SendAsync("ReceiveMessage", new Models.ChatMessage
        {
            Color = "red",
            Date = DateTime.Now.ToShortDateString(),
            Message = "User has been unbanned!",
            SenderFullName = "StudentMasterBot",
            SenderId = "StudentMasterBot"
        });
        await Clients.User(uid).SendAsync("ReceiveMessage", new Models.ChatMessage
        {
            Color = "red",
            Date = DateTime.Now.ToShortDateString(),
            Message = "You are unbanned!",
            SenderFullName = "StudentMasterBot",
            SenderId = "StudentMasterBot"
        });
    }


    [Authorize]
    public async Task BanUser(string uid, string minutes, string reason)
    {
        var role = await GetRoleAsync(Context.User.Identity.Name);
        if (role == "Student")
        {
            await Clients.User(Context.User.Identity.Name).SendAsync("ReceiveMessage", new Models.ChatMessage
            {
                Color = "red",
                Date = DateTime.Now.ToShortDateString(),
                Message = "You dont have permisions!",
                SenderFullName = "StudentMasterBot",
                SenderId = "StudentMasterBot"
            });
        }
        else
        {
            var ban = _banRepository.GetQueryable(x => x.UserId == uid).FirstOrDefault();
            if (ban == null)
            {
                _banRepository.Add(new Ban
                {
                    UserId = uid,
                    DateOfBan = DateTime.Now,
                    Reason = reason,
                    To = DateTime.Now.AddMinutes(Convert.ToInt32(minutes))
                });
            }
            else
            {
                ban.DateOfBan = DateTime.Now;
                ban.Reason = reason;
                ban.To = DateTime.Now.AddMinutes(Convert.ToInt32(minutes));
                _banRepository.Edit(ban);
            }

            await Clients.User(Context.User.Identity.Name).SendAsync("ReceiveMessage", new Models.ChatMessage
            {
                Color = "red",
                Date = DateTime.Now.ToShortDateString(),
                Message = "User has been banned!",
                SenderFullName = "StudentMasterBot",
                SenderId = "StudentMasterBot"
            });
            await Clients.User(uid).SendAsync("ReceiveMessage", new Models.ChatMessage
            {
                Color = "red",
                Date = DateTime.Now.ToShortDateString(),
                Message = "You are banned for " + reason + " to " +
                          DateTime.Now.AddMinutes(Convert.ToInt32(minutes)).ToLongTimeString(),
                SenderFullName = "StudentMasterBot",
                SenderId = "StudentMasterBot"
            });
        }
    }


    [Authorize]
    public async Task SendMessage(string message, int category)
    {
        var ban = _banRepository.GetQueryable(x => x.UserId == Context.User.Identity.Name).FirstOrDefault();
        if (ban != null && ban.To > DateTime.Now)
        {
            await Clients.User(Context.User.Identity.Name).SendAsync("ReceiveMessage", new Models.ChatMessage
            {
                Color = "red",
                Date = DateTime.Now.ToShortDateString(),
                Message = "You are banned for " + ban.Reason + " to " + ban.To.ToString("MM/dd/yyyy HH:mm:ss"),
                SenderFullName = "StudentMasterBot",
                SenderId = "StudentMasterBot"
            });

            return;
        }

        if (category < 1)
            category = _userRepository.GetQueryable(x => x.Id == Context.User.Identity.Name).Include(x => x.Group)
                .FirstOrDefault().Group.Id;

        var sender = await _userRepository.GetQueryable(x => x.Id == Context.User.Identity.Name).Include(x => x.Img)
            .FirstOrDefaultAsync();
        var role = await GetRoleAsync(Context.User.Identity.Name);
        var color = "black";
        if (role == "Admin")
            color = "darkred";
        if (role == "Teacher")
            color = "mediumvioletred";

        _chatRepository.Add(new ChatMessage
        {
            Color = color,
            Date = DateTime.Now,
            Message = message,
            Sender = sender,
            Room = category.ToString()
        });
        await Clients.Group(category.ToString()).SendAsync("ReceiveMessageGroup", new Models.ChatMessage
        {
            Color = color,
            Date = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"),
            Message = message,
            SenderId = sender.Id,
            SenderFullName = $"{sender.FirstName} {sender.Name} {sender.LastName}",
            SenderAvatar = sender.Img.Path
        });
        //}
    }

    [Authorize]
    public void GetAllMessageWithUser(string uid)
    {
        var your_uid = Context.User.Identity.Name;
        var result = new List<Models.ChatMessage>();

        foreach (var el in _chatRepository
                     .GetQueryable(x =>
                         (x.SenderId == your_uid && x.OwnerId == uid) || (x.OwnerId == your_uid && x.SenderId == uid))
                     .Include(x => x.Sender).ThenInclude(x => x.Img))
            result.Add(new Models.ChatMessage
            {
                Color = el.Color,
                Date = el.Date.ToString("MM/dd/yyyy HH:mm:ss"),
                Message = el.Message,
                OwnerId = el.OwnerId,
                SenderAvatar = el.Sender.Img.Path,
                SenderId = el.SenderId,
                SenderFullName = $"{el.Sender.FirstName} {el.Sender.Name} {el.Sender.LastName}"
            });
        Clients.User(your_uid).SendAsync("ReceiveMessages", result);
    }

    [Authorize]
    public async Task SendAllMessages(int category)
    {
        if (category < 1)
            category = _userRepository.GetQueryable(x => x.Id == Context.User.Identity.Name).Include(x => x.Group)
                .FirstOrDefault().Group.Id;

        var msgs = _chatRepository.GetQueryable(x => x.Room == category.ToString()).Include(x => x.Sender)
            .ThenInclude(x => x.Img);
        var result = new HashSet<Models.ChatMessage>();
        foreach (var el in msgs)
        {
            var ban = _banRepository.GetQueryable(x => x.UserId == el.SenderId && x.To > DateTime.Now).FirstOrDefault();

            result.Add(new Models.ChatMessage
            {
                Color = el.Color,
                Date = el.Date.ToString("MM/dd/yyyy HH:mm:ss"),
                Message = ban == null ? el.Message : "Повідомлення видалено модератором.",
                SenderId = el.SenderId,
                SenderFullName = $"{el.Sender.FirstName} {el.Sender.Name} {el.Sender.LastName}",
                SenderAvatar = el.Sender.Img.Path
            });
        }

        await Clients.Group(category.ToString()).SendAsync("receiveAllMessages", result);
        result.Clear();
    }

    [Authorize]
    public async Task GetMyContactsAsync()
    {
        var uid = Context.User.Identity.Name;
        var result = new List<Contact>();

        var msgs = _chatRepository
            .GetQueryable(x =>
                (x.SenderId == Context.User.Identity.Name || x.OwnerId == Context.User.Identity.Name) &&
                (x.Room == null || x.Room == "")).Include(x => x.Owner).Include(x => x.Sender).ThenInclude(x => x.Img)
            .ToList();

        msgs.OrderBy(x => x.Date);

        var users = new List<User>();

        foreach (var el in msgs)
            if (el.SenderId == uid)
                users.Add(el.Owner);
            else
                users.Add(el.Sender);

        users.Distinct();
        foreach (var el in users)
            result.Add(new Contact
                { Id = el.Id, FullName = $"{el.FirstName} {el.Name} {el.LastName}", Url = el.Img.Path });

        foreach (var el in await _userRepository.GetAsync())
            if (result.FirstOrDefault(x => x.Id == el.Id) == null)
                result.Add(new Contact
                    { Id = el.Id, FullName = $"{el.FirstName} {el.Name} {el.LastName}", Url = el.Img.Path });

        result.Distinct();

        await Clients.User(uid).SendAsync("receiveContacts", result);
    }

    public async Task GetContacts(string uid)
    {
        var result = new List<Contact>();

        var msgs = _chatRepository.GetQueryable(x => x.SenderId == uid || x.OwnerId == uid).Include(x => x.Owner)
            .Include(x => x.Sender).ToList();

        msgs.OrderBy(x => x.Date);

        var users = new List<User>();

        foreach (var el in msgs)
            if (el.SenderId == uid)
                users.Add(el.Owner);
            else
                users.Add(el.Sender);

        users.Distinct();
        foreach (var el in users)
            result.Add(new Contact
                { Id = el.Id, FullName = $"{el.FirstName} {el.Name} {el.LastName}", Url = el.Img.Path });

        foreach (var el in await _userRepository.GetAsync())
            if (result.FirstOrDefault(x => x.Id == el.Id) == null)
                result.Add(new Contact
                    { Id = el.Id, FullName = $"{el.FirstName} {el.Name} {el.LastName}", Url = el.Img.Path });

        result.Distinct();

        await Clients.User(uid).SendAsync("receiveContacts", result);
    }

    private async Task<string> GetRoleAsync(string uid)
    {
        var roles = await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(uid));
        if (roles.Contains("Admin"))
            return "Admin";
        if (roles.Contains("Teacher"))
            return "Teacher";
        return "Student";
    }
}