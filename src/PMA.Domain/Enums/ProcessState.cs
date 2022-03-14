// <copyright file="ProcessState.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

namespace PMA.Domain.Enums
{
    /// <summary>
    /// Enumerates different types of process states.
    /// </summary>
    public enum ProcessState
    {
        /// <summary>
        /// The idle state.
        /// </summary>
        Idle = 0,

        /// <summary>
        /// The process in progress.
        /// </summary>
        InProgress,

        /// <summary>
        /// The process is canceled.
        /// </summary>
        Canceled,

        /// <summary>
        /// The process is completed.
        /// </summary>
        Completed,

        /// <summary>
        /// The process exited with an error.
        /// </summary>
        Error
    }
}
