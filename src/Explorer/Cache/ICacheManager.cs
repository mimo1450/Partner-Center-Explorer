// -----------------------------------------------------------------------
// <copyright file="ICacheManager.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Explorer.Cache
{
    using System;

    /// <summary>
    /// Represents cache management strategies for caching access tokens.
    /// </summary>
    public interface ICacheManager
    {
        /// <summary>
        /// Determines whether the specified key exists or not.
        /// </summary>
        /// <param name="key">The key value to be checked.</param>
        /// <returns><c>true</c> if the key exists; otherwise <c>false</c>.</returns>
        /// <exception cref="ArgumentException">
        /// key
        /// </exception>
        bool Exists(string key);

        /// <summary>
        /// Reads the value for the specified key.
        /// </summary>
        /// <param name="key">The key of the value.</param>
        /// <returns>The value associated with the specified key.</returns>
        /// <exception cref="ArgumentException">
        /// key
        /// </exception>
        string Read(string key);

        /// <summary>
        /// Clears the cache by deleting all the items.
        /// </summary>
        void Clear();

        /// <summary>
        /// Deletes the value with specified key.
        /// </summary>
        /// <param name="key">The key of the element to be deleted.</param>
        /// <exception cref="ArgumentException">
        /// key
        /// </exception>
        void Delete(string key);

        /// <summary>
        /// Writes the specified key value pair.
        /// </summary>
        /// <param name="key">The key value to be written.</param>
        /// <param name="value">The value to be associated with the specified key.</param>
        /// <exception cref="ArgumentException">
        /// key
        /// or
        /// value
        /// </exception>
        void Write(string key, string value);

        /// <summary>
        /// Writes the specified key value pair.
        /// </summary>
        /// <param name="key">The key value to be written.</param>
        /// <param name="value">The value to be associated with the specified key.</param>
        /// <param name="expires">When the specified key expires.</param>
        /// <exception cref="ArgumentException">
        /// key
        /// or 
        /// value
        /// </exception>
        void Write(string key, string value, TimeSpan expires);
    }
}