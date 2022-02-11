using CommunityToolkit.Mvvm.Messaging;
using MvvmDemo.Messages;

namespace MvvmDemoTest.Recipients {

    class FakeAsyncYesNoMessageRecipient : IRecipient<AsyncYesNoMessage> {

        public FakeAsyncYesNoMessageRecipient(bool reply) {
            Reply = reply;
        }

        public bool Reply { get; }

        public void Receive(AsyncYesNoMessage message) {
            message.Reply(Reply);
        }
    }
}
