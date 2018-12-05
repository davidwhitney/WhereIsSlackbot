using System;
using NUnit.Framework;
using WhereIs.Slack;

namespace WhereIs.Test.Unit
{
    [TestFixture]
    public class LocateTests
    {
        [Test]
        public void CanDeserializePayload()
        {
            const string payload = "token=UPB5sDjFlKyzqRAX0SPO44fm&team_id=TEM9Z8ZU6" +
                                   "&team_domain=electric-head&channel_id=CEK9TU8SD&" +
                                   "channel_name=random&user_id=UELV368CX&user_name=david&" +
                                   "command=%2Fwhereis&text=david" +
                                   "&response_url=https%3A%2F%2Fhooks.slack.com%2Fcommands%2FTEM9Z8ZU6%2F495994816658%2Fw7dustPvf1OcLQBxlE7p7Zry&trigger_id=495826548660.497339305958.4248e2d037959185626c79146a32eace";

            var result = PayloadMapper.Map(payload);

            Assert.That(result.Token, Is.EqualTo("UPB5sDjFlKyzqRAX0SPO44fm"));
            Assert.That(result.TeamId, Is.EqualTo("TEM9Z8ZU6"));
            Assert.That(result.TeamDomain, Is.EqualTo("electric-head"));
            Assert.That(result.ChannelId, Is.EqualTo("CEK9TU8SD"));
            Assert.That(result.ChannelName, Is.EqualTo("random"));
            Assert.That(result.UserId, Is.EqualTo("UELV368CX"));
            Assert.That(result.UserName, Is.EqualTo("david"));
            Assert.That(result.Command, Is.EqualTo("/whereis"));
            Assert.That(result.Text, Is.EqualTo("david"));
            Assert.That(result.ResponseUrl, Is.EqualTo("https://hooks.slack.com/commands/TEM9Z8ZU6/495994816658/w7dustPvf1OcLQBxlE7p7Zry"));
        }
    }
}
