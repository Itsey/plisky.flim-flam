namespace Plisky.Diagnostics.FlimFlam {

    public abstract class EventParserLinkBase : ChainBase<RawApplicationEvent, SingleOriginEvent> {
        protected IOriginIdentityProvider originSource;

        protected virtual SingleOriginEvent GetEvent(string identifier1, string identifier2) {
            int oi = originSource.GetOriginIdentity(identifier1, identifier2);
            var result = new SingleOriginEvent(oi);
            return result;
        }

        public EventParserLinkBase Link(EventParserLinkBase eplb) {
            next = eplb;
            return this;
        }

        public EventParserLinkBase(IOriginIdentityProvider iop) {
            originSource = iop;
        }
    }

    public class UnknownFormatLink : EventParserLinkBase {

        public UnknownFormatLink(IOriginIdentityProvider i) : base(i) {
        }

        public override SingleOriginEvent Handle(RawApplicationEvent source) {
            if (source == null) {
                throw new ArgumentNullException(nameof(source));
            }

            var result = GetEvent(source.Machine, source.Process);
            result.SetRawText(source.Text);
#if DEBUG
            result.createdBy = nameof(UnknownFormatLink);
#endif
            return result;
        }
    }
}