// -----------------------------------------------------------------------
// <copyright file="InMemoryCacheManager.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Explorer.Cache
{
    using Logic;
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Represent the in-memory cache management strategy.
    /// </summary>
    /// <remarks>This stragtey should only be utilized for single server deployments.</remarks>
    public class InMemoryCacheManager : ICacheManager
    {
        private static readonly Lazy<Dictionary<string, string>> _cache =
           new Lazy<Dictionary<string, string>>(() => new Dictionary<string, string>());

        private readonly MachineKeyDataProtector _protector;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryCacheManager"/> class.
        /// </summary>
        public InMemoryCacheManager()
        {
            _protector = new MachineKeyDataProtector(new[] { typeof(InMemoryCacheManager).FullName });
        }

        /// <summary>
        /// Determines whether the specified key exists or not.
        /// </summary>
        /// <param name="key">The key value to be checked.</param>
        /// <returns>
        /// <c>true</c> if the key exists; otherwise <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// key
        /// </exception>
        public bool Exists(string key)
        {
            key.AssertNotEmpty(nameof(key));

            return Cache.ContainsKey(key);
        }

        /// <summary>
        /// Reads the value for the specified key.
        /// </summary>
        /// <param name="key">The key of the value.</param>
        /// <returns>The value associated with the specified key.</returns>
        /// <exception cref="ArgumentException">
        /// key
        /// </exception>
        public string Read(string key)
        {
            key.AssertNotEmpty(nameof(key));

            byte[] data = _protector.Unprotect(
                Convert.FromBase64String(Cache[key]));

            return Encoding.ASCII.GetString(data);
        }

        /// <summary>
        /// Clears the cache by deleting all the items.
        /// </summary>
        public void Clear()
        {
            Cache.Clear();
        }

        /// <summary>
        /// Deletes the value with specified key.
        /// </summary>
        /// <param name="key">The key of the element to be deleted.</param>
        /// <exception cref="ArgumentException">
        /// key
        /// </exception>
        public void Delete(string key)
        {
            key.AssertNotEmpty(nameof(key));

            Cache.Remove(key);
        }

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
        public void Write(string key, string value)
        {
            key.AssertNotEmpty(nameof(key));
            value.AssertNotEmpty(nameof(value));

            string data = Convert.ToBase64String(
                _protector.Protect(Encoding.ASCII.GetBytes(value)));

            Cache[key] = data;
        }

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
        public void Write(string key, string value, TimeSpan expires)
        {
            key.AssertNotEmpty(nameof(key));
            value.AssertNotEmpty(nameof(value));

            string data = Convert.ToBase64String(
                _protector.Protect(Encoding.ASCII.GetBytes(value)));

            Cache[key] = data;
        }

        private static Dictionary<string, string> Cache => _cache.Value;
    }
}