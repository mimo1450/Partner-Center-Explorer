// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Owin.Security.DataProtection;
using System.Web.Security;

namespace Microsoft.Store.PartnerCenter.Samples.Common.Cache
{
    /// <summary>
    /// Protects data using the <see cref="MachineKey"/>.
    /// </summary>
    /// <seealso cref="Microsoft.Owin.Security.DataProtection.IDataProtector" />
    public class MachineKeyDataProtector : IDataProtector
    {
        private readonly string[] _purposes;

        /// <summary>
        /// Initializes a new instance of the <see cref="MachineKeyDataProtector"/> class.
        /// </summary>
        /// <param name="purposes">THe purpose of the data being protected.</param>
        public MachineKeyDataProtector(string[] purposes)
        {
            _purposes = purposes;
        }

        /// <summary>
        /// Protects the specified data.
        /// </summary>
        /// <param name="userData">The data to be protected.</param>
        /// <returns></returns>
        public byte[] Protect(byte[] userData)
        {
            return MachineKey.Protect(userData, _purposes);
        }

        /// <summary>
        /// Unprotects the specified data.
        /// </summary>
        /// <param name="protectedData">The data to be unprotected.</param>
        /// <returns></returns>
        public byte[] Unprotect(byte[] protectedData)
        {
            return MachineKey.Unprotect(protectedData, _purposes);
        }
    }
}