using System.Diagnostics;

namespace MvvmDemo.Services {
    public class DebugLogger : ILogger {
        public void Log(string message) {
            Debug.WriteLine(message);
        }

    }
}
