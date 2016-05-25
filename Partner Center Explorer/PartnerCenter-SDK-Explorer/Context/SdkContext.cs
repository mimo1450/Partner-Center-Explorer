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
        private IAggregatePartner _partnerOperations;

        public SdkContext()
        { }

        public SdkContext(IAggregatePartner partnerOperations)
        {
            _partnerOperations = partnerOperations;
        }

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
        public IAggregatePartner PartnerOperations
        {
            get
            {
                if (_partnerOperations == null)
                {
                    AuthenticationResult authResult = TokenContext.GetAADToken(
                        string.Format("{0}/common/oauth2", Configuration.Authority),
                        Configuration.ApiServiceRoot
                    );

                    // Authenticate by user context with the partner service
                    IPartnerCredentials userCredentials = PartnerCredentials.Instance.GenerateByUserCredentials(
                        Configuration.ApplicationId,
                        new AuthenticationToken(
                            authResult.AccessToken,
                            authResult.ExpiresOn),
                        delegate
                        {
                            // token has expired, re-Login to Azure Active Directory
                            AuthenticationResult aadToken = TokenContext.GetAADToken(
                                string.Format("{0}/common/oauth2", Configuration.Authority),
                                Configuration.ApiServiceRoot
                            );

                            // give the partner SDK the new add token information
                            return Task.FromResult(new AuthenticationToken(aadToken.AccessToken, aadToken.ExpiresOn));
                        });

                    _partnerOperations = PartnerService.Instance.CreatePartnerOperations(userCredentials);
                }

                return _partnerOperations;
            }
        }
    }
}