
namespace Plisky.FlimFlam; 
public class UserNotificationMessage {
    public UserMessages Message { get; internal set; }
    public UserMessageType MessageType { get; internal set; }
    public string Parameter { get; internal set; }
}