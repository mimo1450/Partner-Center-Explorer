using Newtonsoft.Json;
using System;

namespace Microsoft.Store.PartnerCenter.Samples.Common.Models
{
    /// <summary>
    /// Represents an authornization token.
    /// </summary>
    public class AuthorizationToken
    {
        private long _expiresIn;

        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <value>
        /// The access token.
        /// </value>
        [JsonProperty("access_token")]
        public string AccessToken
        { get; private set; }

        /// <summary>
        /// Determines whether or the access is near expiration.
        /// </summary>
        /// <returns><c>true</c> if near expiration; otherwise <c>false</c>.</returns>
        public bool IsNearExpiry()
        {
            return DateTime.UtcNow > ExpiresOn.AddMinutes(-1);
        }

        /// <summary>
        /// Gets the point in time in which the Access Token returned in the AccessToken property ceases to be valid.
        /// </summary>
        /// <value>
        /// The point in time when the Access Token ceases to be valid.
        /// </value>
        public DateTime ExpiresOn
        {
            get
            {
                return DateTime.UtcNow.AddSeconds(_expiresIn);
            }
        }

        [JsonProperty("expires_in")]
        private long ExpiresIn
        {
            get { return _expiresIn; }
            set { _expiresIn = value; }
        }
    }
}