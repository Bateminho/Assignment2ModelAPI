using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace ModelAPI.Hubs;

[HubName("expenseHub")]
public class ExpenseHub : Hub
{
}