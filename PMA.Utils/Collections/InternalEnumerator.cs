// <copyright file="InternalEnumerator.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;

namespace PMA.Utils.Collections
{
    /// <summary>
    ///  Internal class that serves as a shared enumerator for the underlying collection.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class InternalEnumerator<T> : IEnumerator<T>
    {
        #region IEnumerator<T> Implementation

        public T Current => _current;

        object IEnumerator.Current => _current;

        public void Dispose()
        {
            if (_isDisposed) return;

            // Delegate to parent enumerable DisposeEnumerator() method
            _controllingEnumerable.DisposeEnumerator();

            _isDisposed = true;
        }

        /// <summary>
        /// This method is the crux of this class.  Under lock, it calls MoveNext() on the underlying enumerator and grabs Current.
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            bool value;

            lock (_source)
            {
                value = _source.MoveNext();
                _current = value ? _source.Current : default;
            }

            return value;
        }

        public void Reset()
        {
            throw new NotSupportedException("Reset() not supported");
        }

        #endregion IEnumerator<T> Implementation

        private T _current;
        private bool _isDisposed;
        private readonly InternalEnumerable<T> _controllingEnumerable;
        private readonly IEnumerator<T> _source;

        public InternalEnumerator(IEnumerator<T> source, InternalEnumerable<T> controllingEnumerable)
        {
            _source = source;
            _current = default;
            _controllingEnumerable = controllingEnumerable;
        }
    }
}
