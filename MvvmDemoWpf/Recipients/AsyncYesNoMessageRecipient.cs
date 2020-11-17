using Microsoft.Toolkit.Mvvm.Messaging;
using MvvmDemo.Messages;
using System.Threading.Tasks;
using System.Windows;

namespace MvvmDemoWPF.Recipients {

    class AsyncYesNoMessageRecipient : IRecipient<AsyncYesNoMessage> {

        public void Receive(AsyncYesNoMessage message) {
            var result = MessageBox.Show(message.Text, "Confirm", MessageBoxButton.YesNo);
            message.Reply(result == MessageBoxResult.Yes);
        }
    }
}
