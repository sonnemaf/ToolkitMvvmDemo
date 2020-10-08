using Microsoft.Toolkit.Mvvm.Messaging;
using MvvmDemo.Messages;
using System.Threading.Tasks;
using System.Windows;

namespace MvvmDemoWPF.Recipients {

    class AsyncYesNoMessageRecipient : IRecipient<AsyncYesNoMessage> {

        public void Receive(AsyncYesNoMessage message) {
            Task<bool> GetResult() {
                var result = MessageBox.Show(message.Text, "Confirm", MessageBoxButton.YesNo);
                return Task.FromResult(result == MessageBoxResult.Yes);
            }
            message.Reply(GetResult());
        }
    }
}
