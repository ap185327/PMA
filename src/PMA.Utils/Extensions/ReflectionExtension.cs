// <copyright file="ReflectionExtension.cs" company="Andrey Pospelov">
//     Copyright 2017-2021 Andrey Pospelov. All rights reserved.
// </copyright>

using System.Reflection;

namespace PMA.Utils.Extensions
{
    /// <summary>
    /// Defines method extensions for reflection.
    /// </summary>
    public static class ReflectionExtension
    {
        /// <summary>
        /// Uses reflection to get the field value from an object.
        /// </summary>
        /// <typeparam name="T">The type of field value.</typeparam>
        /// <param name="instance">This instance object.</param>
        /// <param name="fieldName">The field name.</param>
        /// <returns>The field value.</returns>
        public static T GetNotPublicValue<T>(this object instance, string fieldName)
        {
            var field = instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);

            return (T)field?.GetValue(instance);
        }

        /// <summary>
        /// Uses reflection to execute the object method.
        /// </summary>
        /// <param name="instance">This instance object.</param>
        /// <param name="methodName">The method name.</param>
        /// <param name="parameters">The method parameters.</param>
        public static void ExecuteNotPublicMethod(this object instance, string methodName, params object[] parameters)
        {
            var method = instance.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);

            method?.Invoke(instance, parameters);
        }
    }
}
