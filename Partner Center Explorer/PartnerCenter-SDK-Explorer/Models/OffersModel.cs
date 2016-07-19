using System.Collections.Generic;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models
{
    /// <summary>
    /// Model for available offers from Partner Center.
    /// </summary>
    public class OffersModel
    {
        /// <summary>
        /// Gets or sets the available offers from Partner Center.
        /// </summary>
        /// <value>
        /// The available offers from Partner Center.
        /// </value>
        public List<OfferModel> AvailableOffers
        { get; set; }

        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        /// <value>
        /// The customer identifier for which available offers have been obtained.
        /// </value>
        public string CustomerId
        { get; set; }
    }
}