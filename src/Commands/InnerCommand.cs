// © 2019 Sitecore Corporation A/S. All rights reserved. Sitecore® is a registered trademark of Sitecore Corporation A/S.

using System;
using System.Threading.Tasks;
using System.Transactions;
using Ajsuth.Feature.TransactionScopes.Engine.Pipelines;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;

namespace Ajsuth.Feature.TransactionScopes.Engine.Commands
{
    /// <inheritdoc />
    /// <summary>
    /// Defines the SampleCommand command.
    /// </summary>
    public class InnerCommand : CommerceCommand
    {
        private readonly IInnerPipeline _pipeline;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Ajsuth.Feature.TransactionScopes.Engine.SampleCommand" /> class.
        /// </summary>
        /// <param name="pipeline">
        /// The pipeline.
        /// </param>
        /// <param name="serviceProvider">The service provider</param>
        public InnerCommand(IInnerPipeline pipeline, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _pipeline = pipeline;
        }

        /// <summary>
        /// The process of the command
        /// </summary>
        /// <param name="commerceContext">
        /// The commerce context
        /// </param>
        /// <param name="parameter">
        /// The parameter for the command
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<TransactionArgument> Process(CommerceContext commerceContext, TransactionArgument arg)
        {
            TransactionArgument result = null;

            var context = !arg.NewContext ? commerceContext : new CommerceContext(commerceContext.Logger, commerceContext.TelemetryClient)
                {
                    GlobalEnvironment = commerceContext.GlobalEnvironment,
                    Environment = commerceContext.Environment,
                    Headers = commerceContext.Headers
                };

            using (var activity = CommandActivity.Start(context, this))
            {
                await PerformTransaction(
                    context,
                    arg.TransactionScopeOption,
                    async () =>
                    {
                        result = await _pipeline.Run(arg, new CommercePipelineExecutionContextOptions(context)).ConfigureAwait(false);
                    }).ConfigureAwait(false);
                return result;
            }
        }

        public virtual async Task PerformTransaction(CommerceContext commerceContext, TransactionScopeOption transactionScopeOption, Func<Task> action)
        {
            var transactionsPolicy = commerceContext.GetPolicy<TransactionsPolicy>();

            if (transactionsPolicy.TransactionalityEnabled)
            {
                var option = new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadCommitted,
                    Timeout = TimeSpan.FromSeconds(transactionsPolicy.TransactionTimeOut)
                };

                using (var transaction = new TransactionScope(transactionScopeOption, option, transactionsPolicy.AsyncFlowOption))
                {
                    await action().ConfigureAwait(false);
                    await ValidateTransaction(commerceContext, transaction).ConfigureAwait(false);
                }
            }
            else
            {
                await action().ConfigureAwait(false);
            }
        }
    }
}
