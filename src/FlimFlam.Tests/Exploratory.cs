namespace Plisky.FlimFlam.Tests;

using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using AutoFixture;

using Plisky.Diagnostics;
using Plisky.Diagnostics.FlimFlam;
using Plisky.FlimFlam;
using Plisky.Plumbing;
using Shouldly;
using Xunit;

public class Exploratory {
    private readonly Fixture fx;
    public Exploratory() {
        fx = new Fixture();
        
        var f = new FeatureHardCodedProvider();
        f.AddFeature(new Feature("Bilge-OdsOOP", true));

        Feature.AddProvider(f);
        

    }


    [Fact]
    public void Activate_ods_gatherer_should_trigger_activation() {
        var mods = new MockOdsGatherer();
        var sut = new IncomingMessageManager2(new Hub(),mods, new DataStructureManager());

        sut.ActivateODSGatherer();

        mods.IsActive.ShouldBeTrue();
    }

    [Fact]
    public void Ods_on_sends_notification_if_successful() {
        bool userMessageFound = false;
        var h = new Hub();

        var not = h.LookFor<UserNotificationMessage>(x => {
            userMessageFound = true;
            x.MessageType.ShouldBe(UserMessageType.InformationMessage);
            x.Message.ShouldBe(UserMessages.ODSListenerTurnedOn);
        });
        var mods = new MockOdsGatherer();
        var sut = new IncomingMessageManager2(h,mods,new DataStructureManager());

        sut.ActivateODSGatherer();

        userMessageFound.ShouldBeTrue();
    }


    [Fact]
    public void Ods_on_sends_warning_if_fails() {
        bool userMessageFound = false;
        var h = new Hub();

        var not = h.LookFor<UserNotificationMessage>(x => {
            userMessageFound = true;
            x.MessageType.ShouldBe(UserMessageType.WarningMessage);
            x.Message.ShouldBe(UserMessages.ODSListenerTurnedOn);
        });
        var mods = new MockOdsGatherer(false);
        var sut = new IncomingMessageManager2(h,mods, new DataStructureManager());

        sut.ActivateODSGatherer();

        userMessageFound.ShouldBeTrue();
    }


    [Fact]
    public void Deactivate_ods_gatherer_should_trigger_deactivation() {
        var mods = new MockOdsGatherer();
        var sut = new IncomingMessageManager2(new Hub(),mods, new DataStructureManager());

        sut.ActivateODSGatherer();
        sut.DeactivateODSGatherer();

        mods.IsActive.ShouldBeFalse();
    }







}