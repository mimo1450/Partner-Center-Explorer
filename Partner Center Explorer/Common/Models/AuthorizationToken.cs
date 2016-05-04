using Newtonsoft.Json;
using System;

namespace Microsoft.Store.PartnerCenter.Samples.Common.Models
{
    /// <summary>
    /// Represents an authornization token.
    /// </summary>
    public class AuthorizationToken
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationToken"/> class.
        /// </summary>
        /// <param name="accessToken">The access token value.</param>
        /// <param name="expiresIn">When the token will expire.</param>
        public AuthorizationToken(string accessToken, long expiresIn)
        {
            AccessToken = accessToken;
            ExpiresOn = DateTime.UtcNow.AddSeconds(expiresIn);
        }

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
        /// Gets or set the token expiry time.
        /// </summary>
        /// <value>
        /// The time when the the token expires.
        /// </value>
        //[JsonProperty("expires_on")]
        // TODO - This needs to corrected it cannot convert 
        private DateTime ExpiresOn
        { get; set; }
    }
}