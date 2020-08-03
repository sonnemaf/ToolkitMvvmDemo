using Microsoft.Toolkit.Mvvm.Messaging.Messages;

namespace MvvmDemo.Messages {
    public sealed class AsyncYesNoMessage : AsyncRequestMessage<bool> {

        public AsyncYesNoMessage(string text) {
            Text = text;
        }

        public string Text { get; }
    }
}
