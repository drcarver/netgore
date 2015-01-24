using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace NetGore
{
    /// <summary>
    /// A thread-safe buffer that reads the input from the console and buffers it until it is ready to be handled.
    /// </summary>
    public class ConsoleInputBuffer : IDisposable
    {
        readonly Queue<string> _buffer = new Queue<string>();
        readonly object _syncObj = new object();
        bool _isRunning = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleInputBuffer"/> class.
        /// </summary>
        public ConsoleInputBuffer()
        {
            Thread inputThread = new Thread(ReceiveInputLoop) { IsBackground = true, Name = "Console Input" };
            inputThread.Start();
        }

        /// <summary>
        /// Gets an IEnumerable of the input strings since the last call to this method.
        /// </summary>
        /// <returns>An IEnumerable of the input strings since the last call to this method.</returns>
        public IEnumerable<string> GetBuffer()
        {
            if (_buffer.Count == 0)
                return Enumerable.Empty<string>();

            IEnumerable<string> ret;
            lock (_syncObj)
            {
                if (_buffer.Count == 0)
                    ret = Enumerable.Empty<string>();
                else
                {
                    ret = _buffer.ToArray();
                    _buffer.Clear();
                }
            }

            return ret;
        }

        /// <summary>
        /// The thread that does the actual reading from the console.
        /// </summary>
        void ReceiveInputLoop()
        {
            while (_isRunning)
            {
                string input = Console.ReadLine();

                if (!string.IsNullOrEmpty(input))
                {
                    lock (_syncObj)
                    {
                        _buffer.Enqueue(input);
                    }
                }
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _isRunning = false;
        }

        #endregion
    }
}