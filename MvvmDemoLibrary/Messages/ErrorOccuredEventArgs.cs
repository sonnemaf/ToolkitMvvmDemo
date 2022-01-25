using System;

namespace MvvmDemo.Messages {


    public class ErrorOccuredEventArgs : EventArgs {

        public string Message { get; }

        public ErrorOccuredEventArgs(string message) {
            this.Message = message;
        }

    }

    public delegate void ErrorOccuredEventHandler(object sender, ErrorOccuredEventArgs e);

}
