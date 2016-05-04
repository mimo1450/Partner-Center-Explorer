using Microsoft.Samples.AzureAD.Graph.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Samples.AzureAD.Graph.API
{
    public interface IGraphClient
    {
        /// <summary>
        /// Gets a collection of users that belong to the specified tenant identifer. 
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <returns>A collection of users that belong to the specified tenant identifer.</returns>
        List<User> GetUsers(string tenantId);

        /// <summary>
        /// Asynchronously get a collection of users that belong to the specified tenant identifier.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <returns>A collection of users that belong to the specified tenant identifer.</returns>
        Task<List<User>> GetUsersAsync(string tenantId);
    }
}