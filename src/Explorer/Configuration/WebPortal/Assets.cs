﻿// -----------------------------------------------------------------------
// <copyright file="Assets.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Store.PartnerCenter.Explorer.Configuration.WebPortal
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a collection of client asset files.
    /// </summary>
    public class Assets : ICloneable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Assets"/> class.
        /// </summary>
        public Assets()
        {
            Css = new List<string>();
            JavaScript = new List<string>();
            Templates = new List<string>();
        }

        /// <summary>
        /// Gets or sets the assets version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets a collection of CSS file paths.
        /// </summary>
        public IEnumerable<string> Css { get; set; }

        /// <summary>
        /// Gets or sets a collection of JavaScript file paths.
        /// </summary>
        public IEnumerable<string> JavaScript { get; set; }

        /// <summary>
        /// Gets or sets a collection of HTML template URL routes.
        /// </summary>
        public IEnumerable<string> Templates { get; set; }

        /// <summary>
        /// Adds two <see cref="Assets"/> objects and returns the sum.
        /// </summary>
        /// <param name="left">The left side assets object.</param>
        /// <param name="right">The right side assets object.</param>
        /// <returns>A assets object which has the files of both operands appended.</returns>
        public static Assets operator +(Assets left, Assets right)
        {
            if (left == null)
            {
                return right;
            }

            if (right == null)
            {
                return left;
            }

            // combine the assets together
            List<string> combinedCss = new List<string>(left.Css);
            combinedCss.AddRange(right.Css);

            List<string> combinedJavaScript = new List<string>(left.JavaScript);
            combinedJavaScript.AddRange(right.JavaScript);

            List<string> combinedTemplates = new List<string>(left.Templates);
            combinedTemplates.AddRange(right.Templates);

            return new Assets() { Version = left.Version, Css = combinedCss, JavaScript = combinedJavaScript, Templates = combinedTemplates };
        }

        /// <summary>
        /// Clones an <see cref="Assets"/> instance.
        /// </summary>
        /// <returns>A deep copy of the assets object.</returns>
        public object Clone()
        {
            Assets cloneAssets = new Assets() { Version = Version };

            cloneAssets.Css = Css.Clone();
            cloneAssets.JavaScript = JavaScript.Clone();
            cloneAssets.Templates = Templates.Clone();

            return cloneAssets;
        }

        /// <summary>
        /// Ensures the assets object properties are in a good state.
        /// </summary>
        /// <exception cref="InvalidOperationException">If the asset properties are invalid.</exception>
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Version))
            {
                throw new InvalidOperationException("Asset version is not set.");
            }

            // ensure our asset collections are set to something
            if (Css == null)
            {
                Css = new List<string>();
            }

            if (JavaScript == null)
            {
                JavaScript = new List<string>();
            }

            if (Templates == null)
            {
                Templates = new List<string>();
            }

            // validate asset collections to hold valid values
            ValidateAssetCollections(Css);
            ValidateAssetCollections(JavaScript);
            ValidateAssetCollections(Templates);
        }

        /// <summary>
        /// Ensures asset collections contain non empty strings.
        /// </summary>
        /// <param name="assetsCollection">The asset collection to validate.</param>
        /// <exception cref="InvalidOperationException">If the asset properties are invalid.</exception>
        private void ValidateAssetCollections(IEnumerable<string> assetsCollection)
        {
            if (assetsCollection != null)
            {
                foreach (string asset in assetsCollection)
                {
                    if (string.IsNullOrWhiteSpace(asset))
                    {
                        throw new InvalidOperationException("Can't have an empty asset, please ensure all asset strings are set.");
                    }
                }
            }
        }
    }
}