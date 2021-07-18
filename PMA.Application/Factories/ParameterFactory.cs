// <copyright file="ParameterFactory.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Constants;
using System;
using System.Data;

namespace PMA.Application.Factories
{
    /// <summary>
    /// The parameter factory class.
    /// </summary>
    public static class ParameterFactory
    {
        /// <summary>
        /// Create a new instance of the <see cref="byte"/>[] class with default values.
        /// </summary>
        /// <returns>A new instance of the <see cref="byte"/>[] class.</returns>
        public static byte[] Create()
        {
            return new byte[MorphConstants.ParameterCount];
        }

        /// <summary>
        /// Create a new instance of the <see cref="byte"/>[] class.
        /// </summary>
        /// <param name="language">Language.</param>
        /// <param name="part">Part.</param>
        /// <param name="pos1">PoS1.</param>
        /// <param name="pos2">PoS2.</param>
        /// <param name="pos3">PoS3.</param>
        /// <param name="tense">Tense.</param>
        /// <param name="voice">Voice.</param>
        /// <param name="gender">Gender.</param>
        /// <param name="mode">Mode.</param>
        /// <param name="person">Person.</param>
        /// <param name="number">Number.</param>
        /// <param name="case">Case.</param>
        /// <param name="isIrregular">IsIrregular.</param>
        /// <param name="isName">IsName.</param>
        /// <param name="isNegative">IsNegative.</param>
        /// <param name="withAugment">WithAugment.</param>
        /// <param name="formation">Formation.</param>
        /// <param name="parent">Parent.</param>
        /// <param name="type1">Type1.</param>
        /// <param name="type2">Type2.</param>
        /// <param name="type3">Type3.</param>
        /// <returns>A new instance of the <see cref="byte"/>[] class.</returns>
        public static byte[] Create(byte language,
            byte part,
            byte pos1,
            byte pos2,
            byte pos3,
            byte tense,
            byte voice,
            byte gender,
            byte mode,
            byte person,
            byte number,
            byte @case,
            byte isIrregular,
            byte isName,
            byte isNegative,
            byte withAugment,
            byte formation,
            byte parent,
            byte type1,
            byte type2,
            byte type3)
        {
            return new[]
            {
                language,
                part,
                pos1,
                pos2,
                pos3,
                tense,
                voice,
                gender,
                mode,
                person,
                number,
                @case,
                isIrregular,
                isName,
                isNegative,
                withAugment,
                formation,
                parent,
                type1,
                type2,
                type3
            };
        }

        /// <summary>
        /// Create a new instance of the <see cref="byte"/>[] class.
        /// </summary>
        /// <param name="row">The data row.</param>
        /// <returns>A new instance of the <see cref="byte"/>[] class.</returns>
        public static byte[] Create(DataRow row)
        {
            return new[]
            {
                Convert.ToByte(row["Language"]),
                Convert.ToByte(row["Part"]),
                Convert.ToByte(row["PoS1"]),
                Convert.ToByte(row["PoS2"]),
                Convert.ToByte(row["PoS3"]),
                Convert.ToByte(row["Tense"]),
                Convert.ToByte(row["Voice"]),
                Convert.ToByte(row["Gender"]),
                Convert.ToByte(row["Mode"]),
                Convert.ToByte(row["Person"]),
                Convert.ToByte(row["Number"]),
                Convert.ToByte(row["Case"]),
                Convert.ToByte(row["IsIrregular"]),
                Convert.ToByte(row["IsName"]),
                Convert.ToByte(row["IsNegative"]),
                Convert.ToByte(row["WithAugment"]),
                Convert.ToByte(row["Formation"]),
                Convert.ToByte(row["Parent"]),
                Convert.ToByte(row["Type1"]),
                Convert.ToByte(row["Type2"]),
                Convert.ToByte(row["Type3"])
            };
        }

        /// <summary>
        /// Gets a clone of the <see cref="byte"/>[] class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>A new instance of the <see cref="byte"/>[] class.</returns>
        public static byte[] Clone(byte[] source)
        {
            return (byte[])source.Clone();
        }
    }
}
