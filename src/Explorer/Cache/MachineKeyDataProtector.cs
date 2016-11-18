// -----------------------------------------------------------------------
// <copyright file="MachineKeyDataProtector.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Explorer.Cache
{
    using Owin.Security.DataProtection;
    using System.Web.Security;

    /// <summary>
    /// Protects data using the <see cref="MachineKey"/>.
    /// </summary>
    /// <seealso cref="Microsoft.Owin.Security.DataProtection.IDataProtector" />
    internal sealed class MachineKeyDataProtector : IDataProtector
    {
        private readonly string[] _purposes;

        /// <summary>
        /// Initializes a new instance of the <see cref="MachineKeyDataProtector"/> class.
        /// </summary>
        /// <param name="purposes">The purpose of the data being protected.</param>
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