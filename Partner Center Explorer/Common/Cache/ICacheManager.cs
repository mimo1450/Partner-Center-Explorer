// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Store.PartnerCenter.Samples.Common.Cache
{
    public interface ICacheManager
    {
        /// <summary>
        /// Determines whether the specified key exists or not.
        /// </summary>
        /// <param name="key">The key value to be checked.</param>
        /// <returns><c>true</c> if the key exists; otherwise <c>false</c>.</returns>
        bool KeyExists(string key);

        /// <summary>
        /// Reads the value for the specified key.
        /// </summary>
        /// <param name="key">The key of the value.</param>
        /// <returns></returns>
        string Read(string key);

        /// <summary>
        /// Deletes the specified key.
        /// </summary>
        /// <param name="key">The key value to be deleted.</param>
        void Delete(string key);

        /// <summary>
        /// Writes the specified key value pair.
        /// </summary>
        /// <param name="key">The key value to be written.</param>
        /// <param name="value">The value to be written.</param>
        void Write(string key, string value);

        /// <summary>
        /// Writes the specified key value pair.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="expires">The expires.</param>
        void Write(string key, string value, TimeSpan expires);
    }
}