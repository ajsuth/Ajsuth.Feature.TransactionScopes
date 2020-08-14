// © 2019 Sitecore Corporation A/S. All rights reserved. Sitecore® is a registered trademark of Sitecore Corporation A/S.

using System;
using Sitecore.Commerce.Core;

namespace Ajsuth.Feature.TransactionScopes.Engine.Entities
{
    public class SampleEntity : CommerceEntity
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Sitecore.Commerce.Plugin.Sample.SampleEntity" /> class.
        /// </summary>
        public SampleEntity()
        {
            DateCreated = DateTime.UtcNow;
            DateUpdated = DateCreated;
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Sitecore.Commerce.Plugin.Sample.SampleEntity" /> class. 
        /// Public Constructor
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public SampleEntity(string id) : this()
        {
            Id = id;
        }
    }
}
