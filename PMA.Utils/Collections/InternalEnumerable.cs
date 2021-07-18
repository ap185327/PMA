// <copyright file="InternalEnumerable.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace PMA.Utils.Collections
{
    /// <summary>
    /// Internal class that serves as a shared enumerable for the underlying collection.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class InternalEnumerable<T> : IEnumerable<T>, IDisposable
    {
        #region IDisposable Implementation

        /// <summary>
        ///  Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed) return;

            // Only dispose the source enumerator if you are doing dynamic partitioning
            if (!_downCountEnumerators)
            {
                _reader.Dispose();
            }

            _isDisposed = true;
        }

        #endregion IDisposable Implementation

        #region IEnumerable<T> Implementation

        public IEnumerator<T> GetEnumerator()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("InternalEnumerable: Can't call GetEnumerator() after disposing");
            }

            // For static partitioning, keep track of the number of active enumerators.
            if (_downCountEnumerators)
            {
                Interlocked.Increment(ref _activeEnumerators);
            }

            return new InternalEnumerator<T>(_reader, this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)this).GetEnumerator();
        }

        #endregion IEnumerable<T> Implementation

        /// <summary>
        /// Was the object disposed of or not.
        /// </summary>
        private bool _isDisposed;

        private readonly IEnumerator<T> _reader;

        // These two are used to implement Dispose() when static partitioning is being performed
        private int _activeEnumerators;
        private readonly bool _downCountEnumerators;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="downCountEnumerators">Will be true for static partitioning, false for dynamic partitioning.</param>
        public InternalEnumerable(IEnumerator<T> reader, bool downCountEnumerators)
        {
            _reader = reader;
            _activeEnumerators = 0;
            _downCountEnumerators = downCountEnumerators;
        }

        /// <summary>
        /// Called from Dispose() method of spawned InternalEnumerator.  During static partitioning, the source enumerator will be automatically disposed once all requested InternalEnumerators have been disposed.
        /// </summary>
        public void DisposeEnumerator()
        {
            if (!_downCountEnumerators) return;

            if (Interlocked.Decrement(ref _activeEnumerators) == 0)
            {
                _reader.Dispose();
            }
        }
    }
}
