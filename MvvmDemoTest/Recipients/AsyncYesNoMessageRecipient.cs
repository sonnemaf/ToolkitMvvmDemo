using Microsoft.Toolkit.Mvvm.Messaging;
using MvvmDemo.Messages;

namespace MvvmDemoTest.Recipients {

    class AsyncYesNoMessageRecipient : IRecipient<AsyncYesNoMessage> {

        public AsyncYesNoMessageRecipient(bool reply) {
            this.Reply = reply;
        }

        public bool Reply { get; }

        public void Receive(AsyncYesNoMessage message) {
            message.Reply(Reply);
        }
    }
}
