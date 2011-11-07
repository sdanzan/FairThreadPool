/*

    Copyright (c) 2011 Serge Danzanvilliers <serge.danzanvilliers@gmail.com>

    This file is part of "The Fair Thread Pool".

    "The Fair Thread Pool" is free software; you can redistribute it and/or 
    modify it under the terms of the Lesser GNU General Public License as
    published by the Free Software Foundation; either version 3 of the License,
    or (at your option) any later version.

    "The Fair Thread Pool" is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    Lesser GNU General Public License for more details.

    You should have received a copy of the Lesser GNU General Public License
    along with this program. If not, see <http://www.gnu.org/licenses/>.

*/

using System;
using System.Threading;

namespace FairThreadPool
{
    /// <summary>
    /// A holder for "future" values.
    /// </summary>
    /// <typeparam name="TData">The type of the underlying value.</typeparam>
    public sealed class Future<TData> : IWaitable
    {
        /// <summary>
        /// Access to the undelrying value.
        /// Read access will block until a value is set or an exception is transmitted.
        /// Write access will set the underlying value and signal all waiting threads.
        /// Once set the value cannot be changed and trying to do so will throw.
        /// </summary>
        public TData Value
        {
            get
            {
                lock (this)
                {
                    if (!_set)
                    {
                        Monitor.Wait(this);
                    }
                }

                if (_exception != null) throw new FutureValueException(_exception);
                return _data;
            }

            set
            {
                lock (this)
                {
                    if (_set) throw new FutureAlreadySetException();
                    _data = value;
                    _set = true;
                    Monitor.PulseAll(this);
                }
            }
        }

        /// <summary>
        /// Pass an exception to the Future and signal all waiting
        /// threads. The passed exception will be rethrown in all waiting threads.
        /// </summary>
        /// <param name="exception">The exception to signal to waiting threads.</param>
        public void Throw(Exception exception)
        {
            lock (this)
            {
                if (_set) throw new FutureAlreadySetException();
                _exception = exception;
                _set = true;
                Monitor.PulseAll(this);
            }
        }

        /// <summary>
        /// True if the Future is ready for access, false otherwise.
        /// Ready means either a value has been set or an exception transmitted.
        /// </summary>
        public bool IsSet
        {
            get
            {
                return _set;
            }
        }

        /// <summary>
        /// Wait for the Furture to be ready.
        /// </summary>
        public void Wait()
        {
            lock (this)
            {
                if (!_set)
                {
                    Monitor.Wait(this);
                }
            }
        }

        /// <summary>
        /// Wait for the Future to be ready, up to to a given delay.
        /// </summary>
        /// <param name="msec">Waiting delay in milliseconds.</param>
        /// <returns>True if the Future was ready before the delay, false otherwise.</returns>
        public bool Wait(int msec)
        {
            lock (this)
            {
                if (!_set)
                {
                    return Monitor.Wait(this, msec);
                }
            }
            return true;
        }

        /// <summary>
        /// Wait for the Future to be ready, up to to a given delay.
        /// </summary>
        /// <param name="msec">Waiting delay expressed as as TimeSpan.</param>
        /// <returns>True if the Future was ready before the delay, false otherwise.</returns>
        public bool Wait(TimeSpan span)
        {
            return Wait(span.Milliseconds);
        }

        TData _data;
        volatile Exception _exception;
        volatile bool _set;
    }

    /// <summary>
    /// An exception to signal exception occuring while setting a Future value.
    /// </summary>
    public class FutureValueException : Exception
    {
        public FutureValueException(Exception ex)
            : base("Exception while setting the value of a Future. Check inner exception to get the original exception.", ex)
        {
        }
    }

    /// <summary>
    /// An exception to signal multiple value set.
    /// </summary>
    public class FutureAlreadySetException : Exception
    {
        public FutureAlreadySetException()
            : base("Trying to set a value to an already set Future.")
        {
        }
    }
}