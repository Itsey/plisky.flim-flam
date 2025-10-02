namespace FlimFlam.Tests {
    using System;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using AutoFixture;
    
    using Plisky.Diagnostics;
    using Plisky.Diagnostics.FlimFlam;
    using Plisky.FlimFlam;
    using Shouldly;
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
        [InlineData("{[DAVYJONES][55516][0][2][X:\\Code\\nosccm\\BilgeSample\\BilgeSample\\Program.cs][17][::<Main>$]}#LOG#Hello Wonderful World\r\n", true)]
        public void RegExTest1(string data, bool match) {
            bool asr = TraceMessageFormat.IsTexString(data);

            Assert.Equal(match, asr);
        }




        [Fact(DisplayName = nameof(TestMethod))]

        public void TestMethod() {
            _ = new Fixture();
        }


        
        [Fact]
        [Trait("Bug","LFY-42")]        
        public void V2LFYBugIsCorrected() {
            string incomingMessage = "{[DAVYJONES][55516][0][2][X:\\Code\\nosccm\\BilgeSample\\BilgeSample\\Program.cs][17][::< Main >$]}#LOG#Hello Wonderful World\r\n";
            var rae = new RawApplicationEvent {
                Text = incomingMessage,
                ArrivalTime = DateTime.Now,
                OriginId = 1,
                Process = "-1",
                Machine = "DAVYJONES"
            };
            OriginIdentityStore store = new();
            FFV2FormatLink comparer = new(store);
            var res = comparer.Handle(rae);

            res.ShouldNotBeNull();
            res.Type.ShouldBe(TraceCommandTypes.LogMessage);
            res.Text.ShouldContain("Hello Wonderful World");
        }


        
        [Theory]        
        [InlineData("{[DAVYJONES][55516][0][2][X:\\Code\\nosccm\\BilgeSample\\BilgeSample\\Program.cs][17][::< Main >$]}#LOG#Hello Wonderful World\r\n")]
        public void V2ChainLinkCorrectlyParsesV2Format(string incomingMessage) {
            //V2 >> {[MACHINENAME][PROCESSID][THREADID][NETTHREADID][MODULENAME][LINENUMBER][MOREDATA]}#CMD#TEXTOFDEBUGSTRING
            var rae = new RawApplicationEvent {
                Text = incomingMessage,
                ArrivalTime = DateTime.Now,
                OriginId = 1,
                Process = "-1",
                Machine = "DAVYJONES"
            };
            OriginIdentityStore store = new();
            FFV2FormatLink comparer = new(store);
            var res = comparer.Handle(rae);

            res.ShouldNotBeNull();
            res.Type.ShouldBe(TraceCommandTypes.LogMessage);
            res.Text.ShouldContain("Hello Wonderful World");
        }


        [Theory]
        [InlineData("{[DAVYJONES][55516][0][2][X:\\Code\\nosccm\\BilgeSample\\BilgeSample\\Program.cs][17][::< Main >$]}#LOG#Hello Wonderful World\r\n")]
        public void TestChain(string incomingMessage) {
            EventRecieverForComparisons comparer = new();
            OriginIdentityStore store = new();
            var newImporter = new ImportParser(store);
            var dp = new DataParser(store, newImporter, comparer);

            var rae = new RawApplicationEvent {
                Text = incomingMessage,
                ArrivalTime = DateTime.Now,
                OriginId = 1,
                Process = "-1",
                Machine = "DAVYJONES"
            };

            dp.AddRawEvent(rae);

            var evt = comparer.GetEvent();

            evt.ShouldNotBeNull();
            evt.Type.ShouldBe(TraceCommandTypes.LogMessage);
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