﻿namespace DotNetty.Transport.Channels
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using DotNetty.Common.Concurrency;
    using DotNetty.Common.Utilities;

    public static class TaskExtensions
    {
        [MethodImpl(InlineMethod.AggressiveOptimization)]
        public static Task CloseOnComplete(this Task task, IChannel channel)
        {
            if (task.IsCompleted)
            {
                _ = channel.CloseAsync();
                return TaskUtil.Completed;
            }
            else
            {
                return task.ContinueWith(CloseChannelOnCompleteAction, channel, TaskContinuationOptions.ExecuteSynchronously);
            }
        }
        private static readonly Action<Task, object> CloseChannelOnCompleteAction = CloseChannelOnComplete;
        private static void CloseChannelOnComplete(Task t, object c) => _ = ((IChannel)c).CloseAsync();


        [MethodImpl(InlineMethod.AggressiveOptimization)]
        public static Task CloseOnComplete(this Task task, IChannel channel, IPromise promise)
        {
            if (task.IsCompleted)
            {
                _ = channel.CloseAsync(promise);
                return TaskUtil.Completed;
            }
            else
            {
                return task.ContinueWith(CloseWrappedChannelOnCompleteAction, Tuple.Create(channel, promise), TaskContinuationOptions.ExecuteSynchronously);
            }
        }
        private static readonly Action<Task, object> CloseWrappedChannelOnCompleteAction = CloseWrappedChannelOnComplete;
        private static void CloseWrappedChannelOnComplete(Task t, object s)
        {
            var wrapped = (Tuple<IChannel, IPromise>)s;
            _ = wrapped.Item1.CloseAsync(wrapped.Item2);
        }


        [MethodImpl(InlineMethod.AggressiveOptimization)]
        public static Task CloseOnComplete(this Task task, IChannelHandlerContext ctx)
        {
            if (task.IsCompleted)
            {
                _ = ctx.CloseAsync();
                return TaskUtil.Completed;
            }
            else
            {
                return task.ContinueWith(CloseContextOnCompleteAction, ctx, TaskContinuationOptions.ExecuteSynchronously);
            }
        }
        private static readonly Action<Task, object> CloseContextOnCompleteAction = CloseContextOnComplete;
        private static void CloseContextOnComplete(Task t, object c) => _ = ((IChannelHandlerContext)c).CloseAsync();


        [MethodImpl(InlineMethod.AggressiveOptimization)]
        public static Task CloseOnComplete(this Task task, IChannelHandlerContext ctx, IPromise promise)
        {
            if (task.IsCompleted)
            {
                _ = ctx.CloseAsync(promise);
                return TaskUtil.Completed;
            }
            else
            {
                return task.ContinueWith(CloseWrappedContextOnCompleteAction, Tuple.Create(ctx, promise), TaskContinuationOptions.ExecuteSynchronously);
            }
        }
        private static readonly Action<Task, object> CloseWrappedContextOnCompleteAction = CloseWrappedContextOnComplete;
        private static void CloseWrappedContextOnComplete(Task t, object s)
        {
            var wrapped = (Tuple<IChannelHandlerContext, IPromise>)s;
            _ = wrapped.Item1.CloseAsync(wrapped.Item2);
        }


        [MethodImpl(InlineMethod.AggressiveOptimization)]
        public static Task CloseOnFailure(this Task task, IChannel channel)
        {
            if (task.IsCompleted)
            {
                if (task.IsFault())
                {
                    _ = channel.CloseAsync();
                }
                return TaskUtil.Completed;
            }
            else
            {
                return task.ContinueWith(CloseChannelOnFailureAction, channel, TaskContinuationOptions.ExecuteSynchronously);
            }
        }
        private static readonly Action<Task, object> CloseChannelOnFailureAction = CloseChannelOnFailure;
        private static void CloseChannelOnFailure(Task t, object c)
        {
            if (t.IsFault())
            {
                _ = ((IChannel)c).CloseAsync();
            }
        }


        [MethodImpl(InlineMethod.AggressiveOptimization)]
        public static Task CloseOnFailure(this Task task, IChannel channel, IPromise promise)
        {
            if (task.IsCompleted)
            {
                if (task.IsFault())
                {
                    _ = channel.CloseAsync(promise);
                }
                return TaskUtil.Completed;
            }
            else
            {
                return task.ContinueWith(CloseWrappedChannelOnFailureAction, Tuple.Create(channel, promise), TaskContinuationOptions.ExecuteSynchronously);
            }
        }
        private static readonly Action<Task, object> CloseWrappedChannelOnFailureAction = CloseWrappedChannelOnFailure;
        private static void CloseWrappedChannelOnFailure(Task t, object s)
        {
            if (t.IsFault())
            {
                var wrapped = (Tuple<IChannel, IPromise>)s;
                _ = wrapped.Item1.CloseAsync(wrapped.Item2);
            }
        }


        [MethodImpl(InlineMethod.AggressiveOptimization)]
        public static Task CloseOnFailure(this Task task, IChannelHandlerContext ctx)
        {
            if (task.IsCompleted)
            {
                if (task.IsFault())
                {
                    _ = ctx.CloseAsync();
                }
                return TaskUtil.Completed;
            }
            else
            {
                return task.ContinueWith(CloseContextOnFailureAction, ctx, TaskContinuationOptions.ExecuteSynchronously);
            }
        }
        private static readonly Action<Task, object> CloseContextOnFailureAction = CloseContextOnFailure;
        private static void CloseContextOnFailure(Task t, object c)
        {
            if (t.IsFault())
            {
                _ = ((IChannelHandlerContext)c).CloseAsync();
            }
        }


        [MethodImpl(InlineMethod.AggressiveOptimization)]
        public static Task CloseOnFailure(this Task task, IChannelHandlerContext ctx, IPromise promise)
        {
            if (task.IsCompleted)
            {
                if (task.IsFault())
                {
                    _ = ctx.CloseAsync(promise);
                }
                return TaskUtil.Completed;
            }
            else
            {
                return task.ContinueWith(CloseWrappedContextOnFailureAction, Tuple.Create(ctx, promise), TaskContinuationOptions.ExecuteSynchronously);
            }
        }
        private static readonly Action<Task, object> CloseWrappedContextOnFailureAction = CloseWrappedContextOnFailure;
        private static void CloseWrappedContextOnFailure(Task t, object s)
        {
            if (t.IsFault())
            {
                var wrapped = (Tuple<IChannelHandlerContext, IPromise>)s;
                _ = wrapped.Item1.CloseAsync(wrapped.Item2);
            }
        }


        public static Task FireExceptionOnFailure(this Task task, IChannelPipeline pipeline)
        {
            if (task.IsCompleted)
            {
                if (task.IsFault())
                {
                    _ = pipeline.FireExceptionCaught(TaskUtil.Unwrap(task.Exception));
                }
                return TaskUtil.Completed;
            }
            else
            {
                return task.ContinueWith(FirePipelineExceptionOnFailureAction, pipeline, TaskContinuationOptions.ExecuteSynchronously);
            }
        }
        private static readonly Action<Task, object> FirePipelineExceptionOnFailureAction = FirePipelineExceptionOnFailure;
        private static void FirePipelineExceptionOnFailure(Task t, object s)
        {
            if (t.IsFault())
            {
                _ = ((IChannelPipeline)s).FireExceptionCaught(TaskUtil.Unwrap(t.Exception));
            }
        }


        public static Task FireExceptionOnFailure(this Task task, IChannelHandlerContext ctx)
        {
            if (task.IsCompleted)
            {
                if (task.IsFault())
                {
                    _ = ctx.FireExceptionCaught(TaskUtil.Unwrap(task.Exception));
                }
                return TaskUtil.Completed;
            }
            else
            {
                return task.ContinueWith(FireContextExceptionOnFailureAction, ctx, TaskContinuationOptions.ExecuteSynchronously);
            }
        }
        private static readonly Action<Task, object> FireContextExceptionOnFailureAction = FireContextExceptionOnFailure;
        private static void FireContextExceptionOnFailure(Task t, object s)
        {
            if (t.IsFault())
            {
                _ = ((IChannelHandlerContext)s).FireExceptionCaught(TaskUtil.Unwrap(t.Exception));
            }
        }

        /// <summary>TBD</summary>
        [MethodImpl(InlineMethod.AggressiveOptimization)]
        private static bool IsFault(this Task task)
        {
#if NETCOREAPP || NETSTANDARD_2_0_GREATER
            return !task.IsCompletedSuccessfully;
#else
            return task.IsFaulted || task.IsCanceled;
#endif
        }
    }
}
