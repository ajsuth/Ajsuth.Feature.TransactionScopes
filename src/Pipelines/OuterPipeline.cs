// © 2019 Sitecore Corporation A/S. All rights reserved. Sitecore® is a registered trademark of Sitecore Corporation A/S.

using Microsoft.Extensions.Logging;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Ajsuth.Feature.TransactionScopes.Engine.Pipelines
{
    public class OuterPipeline : CommercePipeline<TransactionArgument, TransactionArgument>, IOuterPipeline
    {
        public OuterPipeline(IPipelineConfiguration<IOuterPipeline> configuration, ILoggerFactory loggerFactory)
            : base(configuration, loggerFactory)
        {
        }
    }
}
