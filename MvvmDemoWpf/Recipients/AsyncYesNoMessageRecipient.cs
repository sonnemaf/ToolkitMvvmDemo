using Microsoft.Toolkit.Mvvm.Messaging;
using MvvmDemo.Messages;
using System.Threading.Tasks;
using System.Windows;

namespace MvvmDemoWPF.Recipients {

    sealed class AsyncYesNoMessageRecipient : IRecipient<AsyncYesNoMessage> {

        public static AsyncYesNoMessageRecipient Current { get; } = new AsyncYesNoMessageRecipient();

        private AsyncYesNoMessageRecipient() {
            // Singleton, to avoid GC.Collect when using it with a WeakReferenceMessenger
        }

        public void Receive(AsyncYesNoMessage message) {
            var result = MessageBox.Show(message.Text, "Confirm", MessageBoxButton.YesNo);
            message.Reply(result == MessageBoxResult.Yes);
        }
    }
}
