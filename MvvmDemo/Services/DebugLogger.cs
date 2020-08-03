using System.Diagnostics;

namespace MvvmDemo.Services {
    class DebugLogger : ILogger {
        public void Log(string message) {
            Debug.WriteLine(message);
        }

    }
}
