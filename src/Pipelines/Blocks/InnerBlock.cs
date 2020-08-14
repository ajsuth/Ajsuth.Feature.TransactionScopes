// © 2019 Sitecore Corporation A/S. All rights reserved. Sitecore® is a registered trademark of Sitecore Corporation A/S.

using System;
using System.Threading.Tasks;
using Ajsuth.Feature.TransactionScopes.Engine.Entities;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;

namespace Ajsuth.Feature.TransactionScopes.Engine.Pipelines.Blocks
{
    [PipelineDisplayName("Sample.InnerBlock")]
    public class InnerBlock : PipelineBlock<TransactionArgument, SampleEntity, CommercePipelineExecutionContext>
    {
        protected readonly CommerceCommander Commander;

        /// <summary>
        /// Initializes a new instance of the <see cref="InnerBlock"/> class.
        /// </summary>
        /// <param name="commander">The commander.</param>
        public InnerBlock(CommerceCommander commander)
        {
            this.Commander = commander;
        }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="arg">The SampleArgument argument.</param>
        /// <param name="context">The context.</param>
        /// <returns>The <see cref="SampleEntity"/>.</returns>
        public override async Task<SampleEntity> Run(TransactionArgument arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull($"{Name}: The argument can not be null");

            var result = new SampleEntity()
            {
                Id = $"{CommerceEntity.IdPrefix<SampleEntity>()}Inner-{Guid.NewGuid().ToString()}"
            };

            await Commander.PersistEntity(context.CommerceContext, result).ConfigureAwait(false);

            if (arg.ErrorOnInnerScope)
            {
                context.Abort(
                    await context.CommerceContext.AddMessage(
                        context.GetPolicy<KnownResultCodes>().ValidationError,
                        "ForcedError",
                        new object[] { nameof(arg.ErrorOnInnerScope) },
                        $"Error in {nameof(arg.ErrorOnInnerScope)}.").ConfigureAwait(false),
                    context);

                return null;
            }

            return result;
        }
    }
}
