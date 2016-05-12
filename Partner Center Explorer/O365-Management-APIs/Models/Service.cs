using System.Collections.Generic;

namespace Microsoft.Samples.Office365.Management.API.Models
{
    /// <summary>
    /// Represents an Office 365 service obtained from the Office 365 Service Communication API.
    /// </summary>
    public class Service
    {
        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>
        /// The display name of the service.
        /// </value>
        public string DisplayName
        { get; set; }

        /// <summary>
        /// Gets or sets the features.
        /// </summary>
        /// <value>
        /// The features associated with the service.
        /// </value>
        public List<Feature> Features
        { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier of the service.
        /// </value>
        public string Id
        { get; set; }
    }
}