// <copyright file="CustomException.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using System;

namespace PMA.Domain.Exceptions
{
    /// <summary>
    /// Represents errors that occur during application execution.To browse the .NET Framework source code for this type, see the Reference Source.
    /// </summary>
    public class CustomException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the System.Exception class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public CustomException(string message) : base(message) { }
    }
}
