using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NetCoreSelenium.Library
{
 
        
        public static class TaskHelper
        {
            private static LogHelper log = new LogHelper(typeof(TaskHelper));
            public static class Retry
            {
                private const int _maxAttemptCount = 1000;

                public static void Do(Action action, TimeSpan retryInterval, int maxAttemptCount = _maxAttemptCount)
                {
                    Do<object>(() =>
                    {
                        action();
                        return null;
                    }, retryInterval, maxAttemptCount);
                }

                public static T Do<T>(Func<T> action, TimeSpan retryInterval, int maxAttemptCount = _maxAttemptCount)
                {
                    var exceptions = new List<Exception>();

                    for (int attempted = 0; attempted < maxAttemptCount; attempted++)
                    {
                        try
                        {
                            if (attempted > 0)
                            {
                                Thread.Sleep(retryInterval);
                            }
                            return action();
                        }
                        catch (Exception ex)
                        {
                            exceptions.Add(ex);
                        }
                    }
                    throw new AggregateException(exceptions);
                }
            }

            public static Task StartNew(Action action)
            {
                return Task.Factory.StartNew(() => action());
                //return Task.Factory.StartNew(() => action()).ContinueWith(TaskExceptionHandler, TaskContinuationOptions.OnlyOnFaulted).;
            }

            public static Task StartNew(Action action, Action<Task> TaskExceptionHandler)
            {
                return Task.Factory.StartNew(() => action()).ContinueWith(TaskExceptionHandler, TaskContinuationOptions.OnlyOnFaulted);
            }

            //public static void TaskExceptionHandler(Task task)
            //{
            //    var ex = task.Exception;
            //    log.Error(ex.Message, ex);
            //    throw ex;
            //}
        }


    
}
