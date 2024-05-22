﻿//using Microsoft.AspNet.SignalR;
using SignalR_API.Data;
using SignalR_API.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;


namespace SignalR_API
{
    public class NotificationHub : Hub
    {
        private readonly ApplicationDbContext _context;

        public NotificationHub(ApplicationDbContext context)
        {
            this._context = context;
        }

        // add user Id and Connection Id to UserConnection Table
        public override async Task OnConnectedAsync()
        {
            // get user Id when user logging
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var connectionId = Context.ConnectionId;

            if (!string.IsNullOrEmpty(userId))
            {
                var userConnection = new UserConnection
                {
                    UserId = userId,
                    ConnectionId = connectionId
                };

                // add userId and connectionId to database
                _context.UserConnections.Add(userConnection);
                await _context.SaveChangesAsync();

                // Rejoin groups on reconnect
                var userGroups = await _context.UserGroups
                    .Where(ug => ug.UserId == userId)
                    .Select(ug => ug.Group.Name)
                    .ToListAsync();

                // when user reconnected get groups name from database and added again
                foreach (var group in userGroups)
                {
                    await Groups.AddToGroupAsync(connectionId, group);
                }
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var connectionId = Context.ConnectionId;
            var userConnection = await _context.UserConnections
                .FirstOrDefaultAsync(uc => uc.ConnectionId == connectionId);

            if (userConnection != null)
            {
                // remove user Id and userConnection from databse when user diconnec
                _context.UserConnections.Remove(userConnection);
                await _context.SaveChangesAsync();
            }

            await base.OnDisconnectedAsync(exception);
        }

        // add user Id and group name  to Group table
        public async Task AddUserToGroup(string groupName, int groupId)
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                throw new HubException("User ID is not available.");
            }

            var group = await _context.Groups
                .FirstOrDefaultAsync(g => g.Name == groupName);

            if (group == null)
            {
                group = new Group {GroupId = groupId, Name = groupName };
                _context.Groups.Add(group);
                await _context.SaveChangesAsync();
            }

            var userGroup = new UserGroup
            {
                GroupId = group.GroupId,
                UserId = userId
            };
            // add GroupId  and group name  to userGroup table

            _context.UserGroups.Add(userGroup);
            await _context.SaveChangesAsync();

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task RemoveUserFromGroup(string groupName)
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                throw new HubException("User ID is not available.");
            }

            var group = await _context.Groups
                .FirstOrDefaultAsync(g => g.Name == groupName);

            if (group != null)
            {
                var userGroup = await _context.UserGroups
                    .FirstOrDefaultAsync(ug => ug.GroupId == group.Id && ug.UserId == userId);

                if (userGroup != null)
                {
                    _context.UserGroups.Remove(userGroup);
                    await _context.SaveChangesAsync();
                }

                await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            }
        }
    }
}
