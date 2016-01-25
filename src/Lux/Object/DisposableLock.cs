using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Lux
{
    public class DisposableLock
    {
        private readonly Queue<Token> _tokens = new Queue<Token>();
        private readonly IDisposable _disposable;
        private bool _busy;

        public DisposableLock()
        {
            _disposable = new Disposable(this);
        }


        //public IDisposable Enter()
        //{
        //    var task = EnterAsync();
        //    task.Wait();
        //    return task.Result;
        //}

        public Task<IDisposable> EnterAsync()
        {
            lock (this)
            {
                Token token = new Token();
                _tokens.Enqueue(token);
                if (!_busy)
                {
                    _busy = true;
                    _tokens.Dequeue().Signal(true);
                }
                var task = Task.Factory.FromAsync(token, result => _disposable);
                return task;
            }
        }

        private void Exit()
        {
            lock (this)
            {
                if (_tokens.Any())
                {
                    _tokens.Dequeue().Signal(false);
                }
                else
                {
                    _busy = false;
                }
            }
        }



        private class Token : IAsyncResult
        {
            private ManualResetEvent _event = new ManualResetEvent(false);
            private bool _synchronous = false;

            public object AsyncState
            {
                get { return null; }
            }

            public WaitHandle AsyncWaitHandle
            {
                get { return _event; }
            }

            public bool CompletedSynchronously
            {
                get { return _synchronous; }
            }

            public bool IsCompleted
            {
                get { return _event.WaitOne(TimeSpan.Zero); }
            }

            public void Signal(bool synchronous)
            {
                _synchronous = synchronous;
                _event.Set();
            }
        }

        private class Disposable : IDisposable
        {
            private readonly DisposableLock _owner;

            public Disposable(DisposableLock owner)
            {
                _owner = owner;
            }

            public void Dispose()
            {
                _owner.Exit();
            }
        }
    }
}
