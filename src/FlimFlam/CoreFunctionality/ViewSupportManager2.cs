using Plisky.Plumbing;

namespace Plisky.FlimFlam; 
public class ViewSupportManager2 : ViewSupportManager {
    protected Hub h;

    public ViewSupportManager2(Hub hb) : base() {
        h = hb;
        h.LookFor<UserNotificationMessage>(x => {
            AddUserNotificationMessageByIndex(x.Message, x.MessageType, x.Parameter);
        });
    }

}