using AutoFixture;
using Plisky.FlimFlam;
using Plisky.Plumbing;
using Shouldly;
using Xunit;

namespace Plisky.FlimFlam.Tests;
public class ImportFeature {
    public ImportFeature() {

        var f = new FeatureHardCodedProvider();
        f.AddFeature(new Feature("FF-NewImport", true));
        Feature.AddProvider(f);
    }


    [Fact]
    public void ProcessMessages_leaves_empty_queue() {
        var mods = new MockOdsGatherer();
        var mdat = new MockDataStructureManager();
        var sut = new IncomingMessageManagerTest(new Hub(), mods, mdat);

        for (int i = 0; i < 3; i++) {
            sut.AddIncomingMessage(InternalSource.Other, "Test", 1);
        }

        sut.GetMessageQueueLength().ShouldBe(3);
        sut.ProcessNextStoredMessage(false);

        sut.GetMessageQueueLength().ShouldBe(0);

    }

    [Theory]
    [InlineData(3, true, 1)]
    [InlineData(3, false, 3)]
    public void Remove_duplicates_takes_effect_when_set(int repeats, bool dupeSetting, int survived) {
        var mods = new MockOdsGatherer();
        var mdat = new MockDataStructureManager();
        var sut = new IncomingMessageManagerTest(new Hub(), mods, mdat);
        sut.RemoveDupesOnImport = dupeSetting;


        for (int i = 0; i < repeats; i++) {
            sut.AddIncomingMessage(InternalSource.Other, "Test", 1);
        }

        sut.GetMessageQueueLength().ShouldBe(repeats);

        sut.ProcessNextStoredMessage(false);

        sut.GetMessageQueueLength().ShouldBe(0);
        mdat.EventEntriesRecieved.ShouldBe(survived);
    }




}