// <copyright file="SingleElementPartitioner.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace PMA.Utils.Collections
{
    /// <summary>
    /// Simple partitioner that will extract one item at a time, in a thread-safe fashion, from the underlying collection.
    /// </summary>
    public class SingleElementPartitioner<T> : Partitioner<T>
    {
        #region Partitioner Implementation

        /// <summary>
        /// Produces a list of "numPartitions" IEnumerators that can each be used to traverse the underlying collection in a thread-safe manner. This will return a static number of enumerators, as opposed to GetDynamicPartitions(), the result of which can be used to produce any number of enumerators.
        /// </summary>
        /// <param name="numPartitions">Number of partitions.</param>
        /// <returns></returns>
        public override IList<IEnumerator<T>> GetPartitions(int numPartitions)
        {
            if (numPartitions < 1)
            {
                throw new ArgumentOutOfRangeException($"Number of partitions ({numPartitions}) must be greater than 1");
            }

            var list = new List<IEnumerator<T>>(numPartitions);

            // Since we are doing static partitioning, create an InternalEnumerable with reference
            // counting of spawned InternalEnumerators turned on. Once all of the spawned enumerators
            // are disposed, dynamicPartitions will be disposed.
            var dynamicPartitions = new InternalEnumerable<T>(_referenceEnumerable.GetEnumerator(), true);

            for (int i = 0; i < numPartitions; i++)
            {
                list.Add(dynamicPartitions.GetEnumerator());
            }

            return list;
        }

        #endregion Partitioner Implementation

        /// <summary>
        /// The collection being wrapped by this Partitioner
        /// </summary>
        private readonly IEnumerable<T> _referenceEnumerable;

        /// <summary>
        /// Constructor just grabs the collection to wrap.
        /// </summary>
        /// <param name="enumerable"></param>
        public SingleElementPartitioner(IEnumerable<T> enumerable)
        {
            // Verify that the source IEnumerable is not null

            _referenceEnumerable = enumerable ?? throw new ArgumentNullException($"{nameof(enumerable)} is null");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Returns an instance of internal Enumerable class. GetEnumerator() can then be called on that (multiple times) to produce shared enumerators.</returns>
        public override IEnumerable<T> GetDynamicPartitions()
        {
            // Since we are doing dynamic partitioning, create an InternalEnumerable with reference
            // counting of spawned InternalEnumerators turned off.  This returned InternalEnumerable
            // will need to be explicitly disposed.
            return new InternalEnumerable<T>(_referenceEnumerable.GetEnumerator(), false);
        }
    }
}
