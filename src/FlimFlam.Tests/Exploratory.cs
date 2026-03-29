namespace Plisky.FlimFlam.Tests;

using System;
using System.Text;
using System.Xml.Linq;
using AutoFixture;
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
    public void Display_enumerable_dump_can_be_simplified() {
        string dump = " <dmp><flds><fld><t>System.Char</t><n>m_Item1</n><v>8</v></fld> \r\n<fld><t>int</t><n>m_Item2</n><v>14</v></fld> \r\n</flds><prps><prp><t>System.Char</t><n>Item1</n><v>8</v></prp> \r\n<prp><t>int</t><n>Item2</n><v>14</v></prp> \r\n</prps></dmp>";

        // Parse the XML and produce lines of the form: type : name : value
        var doc = XDocument.Parse(dump);
        var sb = new StringBuilder();

        var flds = doc.Root?.Element("flds");
        if (flds != null) {
            foreach (var fld in flds.Elements("fld")) {
                var type = fld.Element("t")?.Value ?? string.Empty;
                var name = fld.Element("n")?.Value ?? string.Empty;
                var value = fld.Element("v")?.Value ?? string.Empty;
                sb.AppendLine($"{type} : {name} : {value}");
            }
        }

        string result = sb.ToString();
        // write to console so test output shows it when running tests
        Console.Write(result);
    }

    [Fact]
    public void Activate_ods_gatherer_should_trigger_activation() {
        var mods = new MockOdsGatherer();
        var sut = new IncomingMessageManager2(new Hub(), mods, new DataStructureManager());

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
        var sut = new IncomingMessageManager2(h, mods, new DataStructureManager());

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
        var sut = new IncomingMessageManager2(h, mods, new DataStructureManager());

        sut.ActivateODSGatherer();

        userMessageFound.ShouldBeTrue();
    }


    [Fact]
    public void Deactivate_ods_gatherer_should_trigger_deactivation() {
        var mods = new MockOdsGatherer();
        var sut = new IncomingMessageManager2(new Hub(), mods, new DataStructureManager());

        sut.ActivateODSGatherer();
        sut.DeactivateODSGatherer();

        mods.IsActive.ShouldBeFalse();
    }






}