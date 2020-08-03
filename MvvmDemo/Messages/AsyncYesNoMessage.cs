using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace MvvmDemo.Messages {
    public sealed class AsyncYesNoMessage : AsyncRequestMessage<bool> {

        public AsyncYesNoMessage(string text) {
            Text = text;
        }

        public string Text { get; }
    }
}
