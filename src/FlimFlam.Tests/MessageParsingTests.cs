using System;
using AutoFixture;
using Plisky.Diagnostics;
using Plisky.Diagnostics.FlimFlam;
using Plisky.FlimFlam;
using Plisky.Plumbing;
using Shouldly;
using Xunit;

namespace FlimFlam.Tests;

public class MessageParsingTests {
    private readonly Fixture fx;

    public MessageParsingTests() {
        fx = new Fixture();

        var f = new FeatureHardCodedProvider();
        f.AddFeature(new Feature("Bilge-OdsOOP", true));

        Feature.AddProvider(f);
    }

    [Fact]
    [Trait("Bug", "LFY-42")]
    public void Special_characters_do_not_prevent_message_parsing() {
        string incomingMessage = "{[DAVYJONES][55516][0][2][X:\\Code\\nosccm\\BilgeSample\\BilgeSample\\Program.cs][17][::< Main >$]}#LOG#Hello Wonderful World\r\n";

        var rae = fx.Build<RawApplicationEvent>()
            .With(x => x.Text, incomingMessage)
            .Create();

        OriginIdentityStore store = new();
        FFV2FormatLink comparer = new(store);

        var res = comparer.Handle(rae);

        res.ShouldNotBeNull();
        res.Type.ShouldBe(TraceCommandTypes.LogMessage);
        res.Text.ShouldContain("Hello Wonderful World");
    }

    [Theory]
    [InlineData("{[DAVYJONES][55516][0][2][X:\\Code\\nosccm\\BilgeSample\\BilgeSample\\Program.cs][17][::< Main >$]}#LOG#Hello Wonderful World\r\n")]
    [InlineData("{[Norway][1025][8901][Donner][ChimneyApproach.cs][125][ChimneyApproach::SlopeSolve]}#LOG#Approach solved grinch; roofPitch=32°, ropeSlack=grinch 0.12m, flueAngle=+6°; descentRate=0.8m/s grinch; safetyMargin=grinch 0.19")]
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
}