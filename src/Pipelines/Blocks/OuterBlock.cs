// © 2019 Sitecore Corporation A/S. All rights reserved. Sitecore® is a registered trademark of Sitecore Corporation A/S.

using System;
using System.Threading.Tasks;
using Ajsuth.Feature.TransactionScopes.Engine.Commands;
using Ajsuth.Feature.TransactionScopes.Engine.Entities;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;

namespace Ajsuth.Feature.TransactionScopes.Engine.Pipelines.Blocks
{
    [PipelineDisplayName("Sample.OuterBlock")]
    public class OuterBlock : PipelineBlock<TransactionArgument, SampleEntity, CommercePipelineExecutionContext>
    {
        protected readonly CommerceCommander Commander;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateReservationBlock"/> class.
        /// </summary>
        /// <param name="commander">The commander.</param>
        public OuterBlock(CommerceCommander commander)
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

            if (arg.ErrorBeforeInnerScope)
            {
                await context.CommerceContext.AddMessage(
                    context.GetPolicy<KnownResultCodes>().Error,
                    "ForcedError",
                    new object[] { nameof(arg.ErrorBeforeInnerScope) },
                    $"Error in {nameof(arg.ErrorBeforeInnerScope)}.").ConfigureAwait(false);
            }

            await Commander.Command<InnerCommand>().Process(context.CommerceContext, arg).ConfigureAwait(false);

            var result = new SampleEntity()
            {
                Id = $"{CommerceEntity.IdPrefix<SampleEntity>()}Outer-{Guid.NewGuid().ToString()}"
            };

            await Commander.PersistEntity(context.CommerceContext, result).ConfigureAwait(false);

            if (arg.ErrorOnOuterScope)
            {
                context.Abort(
                    await context.CommerceContext.AddMessage(
                        context.GetPolicy<KnownResultCodes>().ValidationError,
                        "ForcedError",
                        new object[] { nameof(arg.ErrorOnOuterScope) },
                        $"Error in {nameof(arg.ErrorOnOuterScope)}.").ConfigureAwait(false),
                    context);

                return null;
            }

            return result;
        }
    }
}
