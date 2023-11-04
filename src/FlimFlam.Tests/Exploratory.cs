namespace FlimFlam.Tests {
    using System;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using AutoFixture;
    using Plisky.Diagnostics;
    using Plisky.Diagnostics.FlimFlam;
    using Support.ImportManagement;
    using Xunit;

    public class Exploratory {
        private readonly Fixture fx;
        public Exploratory() {
            fx = new Fixture();
        }

        [Theory]
        [InlineData("", false)]
        [InlineData("arflebarflegloop", false)]
        [InlineData(null, false)]
        [InlineData("Razer Synapse Service Process.exe Information: 0 : ", false)]
        [InlineData("{[CALYPSO][23376][1][1][alerting][0][::-ba-alert-]}#ALT#TestClient Online.  @[25/05/2020 15:36:17] on [CALYPSO]~~#~~{  \"alert-name\": \"online\" \"onelineAt\": \"25 / 05 / 2020 15:36:17\" \"machine-name\": \"CALYPSO\" \"appName\": \"TestClient\"{ ", true)]
        [InlineData("{[MMD-3814966][16416][1][1][E:\\_AgentWork\\0\\8\\s\\Toolset.NetCore.Logging\\ToolsetExceptionLogger.cs][23][::.ctor]}#LOG#Create ToolsetExceptionLogger", true)]
        public void RegExTest1(string data, bool match) {
            bool asr = TraceMessageFormat.IsTexString(data);

            Assert.Equal(match, asr);
        }




        [Fact(DisplayName = nameof(TestMethod))]

        public void TestMethod() {
            _ = new Fixture();
        }







        [Fact(DisplayName = nameof(EventImportTest))]
        public void EventImportTest() {
            int hits = 0;
            var fx = new Fixture();
            var rax = fx.Create<RawApplicationEvent>();
            var inney = new Subject<RawApplicationEvent>();
            var sut = new EventImport();
            sut.ProvideEvents(inney);
            var outey = sut.Events.Subscribe(v => { hits++; });

            //var inney1 = Observable.Return<RawApplicationEvent>(rax);
            inney.OnNext(rax);





            Assert.Equal(1, hits);


        }

    }
}