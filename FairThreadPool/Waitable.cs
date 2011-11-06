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
    /// A simple interface "waitable" objects.
    /// </summary>
    public interface IWaitable
    {
        /// <summary>
        /// Wait until signaled.
        /// </summary>
        void Wait();

        /// <summary>
        /// Wait until signaled or the specified delay is spent.
        /// </summary>
        /// <param name="msec">A maximum delay in milliseconds to wait for the signal.</param>
        /// <returns>true if return occurs before the delay, false otherwise</returns>
        bool Wait(int msec);

        /// <summary>
        /// Wait until signaled or the specified delay is spent.
        /// </summary>
        /// <param name="span">A maximum delay to wait for the signal.</param>
        /// <returns>true if return occurs before the delay, false otherwise</returns>
        bool Wait(TimeSpan span);
    }
}
