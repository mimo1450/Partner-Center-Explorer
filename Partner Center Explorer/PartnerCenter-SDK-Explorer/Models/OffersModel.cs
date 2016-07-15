using System.Collections.Generic;

namespace Microsoft.Store.PartnerCenter.Samples.SDK.Explorer.Models
{
    public class OffersModel
    {
        public List<OfferModel> AvailableOffers
        { get; set; }

        public string CustomerId
        { get; set; }
    }
}