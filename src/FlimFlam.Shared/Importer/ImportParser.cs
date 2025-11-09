namespace Plisky.Diagnostics.FlimFlam {

    /// <summary>
    /// Responsible for parsing a RawInput structure into a Single Origin Event.
    /// </summary>
    public class ImportParser {
        public EventParserLinkBase parser;

        public ImportParser(OriginIdentityStore ois) {
            parser = new FFV4FormatLink(ois).Link(
                new FFV3FormatLink(ois).Link(
                new V1FormatterLink(ois).Link(
                new FFV2FormatLink(ois).Link(
                new UnknownFormatLink(ois)))));
        }

        public SingleOriginEvent Parse(RawApplicationEvent rae) {
            return parser.Handle(rae);
        }
    }
}