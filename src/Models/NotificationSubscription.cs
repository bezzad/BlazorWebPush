using WebPush;

namespace BlazorWebPush.Models;

public class NotificationSubscription
{
    public string EndPoint { get; set; }
    public string P256dh { get; set; }
    public string Auth { get; set; }
    public PushSubscription ToPushSubscription => new(EndPoint, P256dh, Auth);
}