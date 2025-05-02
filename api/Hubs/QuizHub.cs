using Microsoft.AspNetCore.SignalR;

namespace api.Hubs
{
    public class QuizHub : Hub
    {
        public async Task SendAnswer(string roomId, string answer)
        {
            // Broadcast the answer to other connected clients in the room
            await Clients.OthersInGroup(roomId).SendAsync("ReceiveAnswer", answer);
        }

        public async Task SendQuestion(string roomId, string question)
        {
            // Broadcast the question to all clients in the room
            await Clients.Group(roomId).SendAsync("ReceiveQuestion", question);
        }

        public async Task JoinRoom(string roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        }

        public async Task LeaveRoom(string roomId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
        }
    }
}
