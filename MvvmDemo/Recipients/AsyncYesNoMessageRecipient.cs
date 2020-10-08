using Microsoft.Toolkit.Mvvm.Messaging;
using MvvmDemo.Messages;
using System;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace MvvmDemoUWP.Recipients {

    class AsyncYesNoMessageRecipient : IRecipient<AsyncYesNoMessage> {

        public void Receive(AsyncYesNoMessage message) {

            async Task<bool> GetResult() {
                var md = new MessageDialog(message.Text);
                bool result = false;
                md.Commands.Add(new UICommand("Yes", new UICommandInvokedHandler((cmd) => result = true)));
                md.Commands.Add(new UICommand("No"));
                await md.ShowAsync().AsTask();
                return result;
            }

            message.Reply(GetResult());
        }
    }
}
