// © 2019 Sitecore Corporation A/S. All rights reserved. Sitecore® is a registered trademark of Sitecore Corporation A/S.

using Sitecore.Commerce.Core;
using Sitecore.Framework.Pipelines;

namespace Ajsuth.Feature.TransactionScopes.Engine.Pipelines
{
    [PipelineDisplayName("SamplePipeline")]
    public interface IOuterPipeline : IPipeline<TransactionArgument, TransactionArgument, CommercePipelineExecutionContext>
    {
    }
}
