using CommunityToolkit.Mvvm.Messaging;
using MvvmDemo.Messages;
using System;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace MvvmDemoUWP.Recipients {
    class AsyncYesNoMessageRecipient : IRecipient<AsyncYesNoMessage> {

        public static AsyncYesNoMessageRecipient Current { get; } = new AsyncYesNoMessageRecipient();

        private AsyncYesNoMessageRecipient() {
            // Singleton, to avoid GC.Collect when using it with a WeakReferenceMessenger
        }

        public void Receive(AsyncYesNoMessage message) {
            async Task<bool> GetResult() {
                var md = new MessageDialog(message.Text);
                bool result = false;
                md.Commands.Add(new UICommand("Yes",
                    new UICommandInvokedHandler((cmd) => result = true)));
                md.Commands.Add(new UICommand("No"));
                await md.ShowAsync().AsTask();
                return result;
            }
            message.Reply(GetResult());
        }
    }
}
