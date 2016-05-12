using System;

namespace Microsoft.Samples.Office365.Management.API.Models
{
    /// <summary>
    /// Represents message details obtained from Office 365 Service Communication Service API.  
    /// </summary>
    public class MessageDetails
    {
        /// <summary>
        /// Gets or sets the message text.
        /// </summary>
        /// <value>
        /// The message text of the details.
        /// </value>
        public string MessageText
        { get; set; }

        /// <summary>
        /// Gets or sets the published time.
        /// </summary>
        /// <value>
        /// The time when this detail was published.
        /// </value>
        public DateTime PublishedTime
        { get; set; }
    }
}