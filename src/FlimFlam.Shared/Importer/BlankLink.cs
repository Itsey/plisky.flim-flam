namespace Plisky.Diagnostics.FlimFlam {

    public class BlankLink : EventParserLinkBase {

        public BlankLink(IOriginIdentityProvider i) : base(i) {
        }

        public override SingleOriginEvent Handle(RawApplicationEvent source) {
            throw new NotImplementedException();
        }
    }
}