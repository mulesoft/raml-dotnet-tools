using System;
using System.Threading.Tasks;
using System.Windows.Threading;
using Microsoft.VisualStudio.Threading;

namespace AMF.Common
{
    public static class TaskExtensions
    {
        public static void WaitWithPumping(this JoinableTask task)
        {
            if (task == null) throw new ArgumentNullException("task");
            var nestedFrame = new DispatcherFrame();
            var ts = TaskScheduler.FromCurrentSynchronizationContext();
            task.Task.ContinueWith(_ => nestedFrame.Continue = false, ts);
            Dispatcher.PushFrame(nestedFrame);
            new JoinableTaskFactory(new JoinableTaskContext()).Run(async () => await task);
            //task.Wait();
        }

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