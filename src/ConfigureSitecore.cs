// © 2019 Sitecore Corporation A/S. All rights reserved. Sitecore® is a registered trademark of Sitecore Corporation A/S.

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.Commerce.Core;
using Sitecore.Framework.Configuration;
using Sitecore.Framework.Pipelines.Definitions.Extensions;

namespace Ajsuth.Feature.TransactionScopes.Engine
{
    /// <summary>
    /// The configure sitecore class.
    /// </summary>
    public class ConfigureSitecore : IConfigureSitecore
    {
        /// <summary>
        /// The configure services.
        /// </summary>
        /// <param name="services">
        /// The services.
        /// </param>
        public void ConfigureServices(IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            services.RegisterAllPipelineBlocks(assembly);
            services.RegisterAllCommands(assembly);

            services.Sitecore().Pipelines(config => config

                .AddPipeline<Pipelines.IInnerPipeline, Pipelines.InnerPipeline>(pipeline => pipeline
                    .Add<Pipelines.Blocks.InnerBlock>()
                )

                .AddPipeline<Pipelines.IOuterPipeline, Pipelines.OuterPipeline>(pipeline => pipeline
                    .Add<Pipelines.Blocks.OuterBlock>()
                )

                .ConfigurePipeline<IConfigureServiceApiPipeline>(configure => configure
                    .Add<ConfigureServiceApiBlock>()
                )
            );
        }
    }
}
