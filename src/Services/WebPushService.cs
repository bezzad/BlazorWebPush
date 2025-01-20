using System.Text.Json;
using BlazorWebPush.Models;
using WebPush;

namespace BlazorWebPush.Services;

public class WebPushService(Vapid vapid)
{
    private static readonly List<NotificationSubscription> Subscriptions = [];
    
    public void AddSubscription(NotificationSubscription subscription)
    {
        Subscriptions.Add(subscription);
    }
    
    public void RemoveSubscription(NotificationSubscription subscription)
    {
        Subscriptions.Remove(subscription);
    }
    
    public async ValueTask BroadcastNotificationAsync(string title, string message, CancellationToken token)
    {
        using var client = new WebPushClient();
        var payload = JsonSerializer.Serialize(new { title, message });
        var vapidDetails = new VapidDetails(vapid.Subject, vapid.PublicKey, vapid.PrivateKey);
        
        foreach (var sub in Subscriptions)
        {
            if(token.IsCancellationRequested)
                return;
            
            await client.SendNotificationAsync(sub.ToPushSubscription, payload, vapidDetails, token);
        }
    }
    
    public async ValueTask SendNotificationAsync(NotificationSubscription sub, string title, string message, CancellationToken token = default)
    {
        using var client = new WebPushClient();
        var payload = JsonSerializer.Serialize(new { title, message });
        var vapidDetails = new VapidDetails(vapid.Subject, vapid.PublicKey, vapid.PrivateKey);
        await client.SendNotificationAsync(sub.ToPushSubscription, payload, vapidDetails, token);
    }
    
    public NotificationSubscription[] GetSubscriptions()
    {
        return Subscriptions.ToArray();
    }
}