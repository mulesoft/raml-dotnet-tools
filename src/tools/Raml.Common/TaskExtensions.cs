using System;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Raml.Common
{
    public static class TaskExtensions
    {
        public static void WaitWithPumping(this Task task)
        {
            if (task == null) throw new ArgumentNullException("task");
            var nestedFrame = new DispatcherFrame();
            task.ContinueWith(_ => nestedFrame.Continue = false);
            Dispatcher.PushFrame(nestedFrame);
            task.Wait();
        }
    }
}