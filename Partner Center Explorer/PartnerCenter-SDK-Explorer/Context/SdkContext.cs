using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Store.PartnerCenter.Extensions;
using Microsoft.Store.PartnerCenter.Samples.Common;
using System.Threading.Tasks;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Context
{
    /// <summary>
    /// Context class used to obtain base object to interact with the Partner Center SDK. 
    /// </summary>
    public class SdkContext
    {
        private static IAggregatePartner _userPartnerOperations;

        /// <summary>
        /// Gets the an instance of <see cref="IAggregatePartner"/>.
        /// </summary>
        /// <value>
        /// An instnace of <see cref="IAggregatePartner"/> used by the SDK to perform operations. 
        /// </value>
        /// <remarks>
        /// This property utilizes App + User authentication. Various operations with the Partner Center
        /// SDK require App + User authorization. More details regarding Partner Center authentication can be 
        /// found at https://msdn.microsoft.com/en-us/library/partnercenter/mt634709.aspx
        /// </remarks>
        public static IAggregatePartner UserPartnerOperations
        {
            get
            {
                if (_userPartnerOperations == null)
                {
                    AuthenticationResult authResult = LoginUserToAAD();

                    // Authenticate by user context with the partner service
                    IPartnerCredentials userCredentials = PartnerCredentials.Instance.GenerateByUserCredentials(
                        Configuration.NativeApplicationId,
                        new AuthenticationToken(
                            authResult.AccessToken,
                            authResult.ExpiresOn),
                        delegate
                        {
                            // token has expired, re-Login to Azure Active Directory
                            var aadToken = LoginUserToAAD();

                            // give the partner SDK the new add token information
                            return Task.FromResult(new AuthenticationToken(aadToken.AccessToken, aadToken.ExpiresOn));
                        });

                    _userPartnerOperations = PartnerService.Instance.CreatePartnerOperations(userCredentials);
                }

                return _userPartnerOperations;
            }
        }

        /// <summary>
        /// Authenticates the specified user against Azure AD. 
        /// </summary>
        /// <returns>
        /// An instnace of <see cref="AuthenticationResult"/> which represent the successful authentication attempt.
        /// </returns>
        private static AuthenticationResult LoginUserToAAD()
        {
            AuthenticationContext authContext;
            UserCredential credentials;

            try
            {
                authContext = new AuthenticationContext(
                    string.Format("{0}/common/oauth2",
                        Configuration.Authority
                    )
                );

                credentials = new UserCredential(
                    Configuration.Username,
                    Configuration.Password
                );

                return authContext.AcquireToken(
                    Configuration.ApiServiceRoot,
                    Configuration.NativeApplicationId,
                    credentials
                );
            }
            finally
            {
                authContext = null;
                credentials = null;
            }
        }
    }
}