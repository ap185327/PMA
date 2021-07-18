// <copyright file="ParameterExtension.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using PMA.Domain.Constants;
using System.Collections.Generic;
using System.Linq;

namespace PMA.Application.Extensions
{
    /// <summary>
    /// Defines method extensions for morphological parameters.
    /// </summary>
    internal static class ParameterExtension
    {
        /// <summary>
        /// Updates morphological parameters by another morphological parameters.
        /// </summary>
        /// <param name="thisParameters">This morphological parameters.</param>
        /// <param name="newParameters">A new morphological parameters.</param>
        /// <returns>Morphological parameters are updated or not.</returns>
        public static bool UpdateByParameters(this byte[] thisParameters, byte[] newParameters)
        {
            bool isUpdated = false;

            for (int i = 0; i < thisParameters.Length; i++)
            {
                if (newParameters[i] == MorphConstants.UnknownTermId || thisParameters[i] != MorphConstants.UnknownTermId) continue;

                thisParameters[i] = newParameters[i];
                isUpdated = true;
            }

            return isUpdated;
        }

        /// <summary>
        /// Gets collective morphological parameters from the collection of morphological parameters.
        /// </summary>
        /// <param name="parameterCollection">A collection of morphological parameters</param>
        /// <returns>Collective morphological parameters.</returns>
        public static byte[] GetCollectiveParameters(this IList<byte[]> parameterCollection)
        {
            byte[] collectiveParameters = (byte[])parameterCollection.First().Clone();

            if (parameterCollection.Count == 1)
            {
                return collectiveParameters;
            }

            for (int i = 0; i < parameterCollection.Count; i++)
            {
                byte[] parameters = parameterCollection[i];

                for (int j = 0; j < collectiveParameters.Length; j++)
                {
                    switch (collectiveParameters[j])
                    {
                        case MorphConstants.AlternativeUnknownTermId:
                            continue;
                        case MorphConstants.UnknownTermId:
                            collectiveParameters[j] = parameters[j];
                            break;
                        default:
                            if (collectiveParameters[j] > 0 && collectiveParameters[j] != parameters[j])
                            {
                                collectiveParameters[j] = MorphConstants.AlternativeUnknownTermId;
                            }
                            break;
                    }
                }
            }

            for (int i = 0; i < collectiveParameters.Length; i++)
            {
                if (collectiveParameters[i] == MorphConstants.AlternativeUnknownTermId) collectiveParameters[i] = MorphConstants.UnknownTermId;
            }

            return collectiveParameters;
        }

        /// <summary>
        /// Overrides morphological parameters by another parameters.
        /// </summary>
        /// <param name="thisParameters">This morphological parameters.</param>
        /// <param name="newParameters">New morphological parameters.</param>
        public static void OverrideBy(this byte[] thisParameters, byte[] newParameters)
        {
            for (int i = 0; i < thisParameters.Length; i++)
            {
                if (newParameters[i] == MorphConstants.AlternativeUnknownTermId)
                {
                    thisParameters[i] = MorphConstants.UnknownTermId;
                }
                else if (newParameters[i] != MorphConstants.UnknownTermId)
                {
                    thisParameters[i] = newParameters[i];
                }
            }
        }
    }
}
