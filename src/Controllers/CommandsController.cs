// © 2019 Sitecore Corporation A/S. All rights reserved. Sitecore® is a registered trademark of Sitecore Corporation A/S.

using System;
using System.Threading.Tasks;
using System.Web.Http.OData;
using Ajsuth.Feature.TransactionScopes.Engine.Commands;
using Microsoft.AspNetCore.Mvc;
using Sitecore.Commerce.Core;

namespace Ajsuth.Feature.TransactionScopes.Engine.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// Defines a controller
    /// </summary>
    /// <seealso cref="T:Sitecore.Commerce.Core.CommerceController" />
    public class CommandsController : CommerceController
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Ajsuth.Feature.TransactionScopes.Engine.CommandsController" /> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="globalEnvironment">The global environment.</param>
        public CommandsController(IServiceProvider serviceProvider, CommerceEnvironment globalEnvironment)
            : base(serviceProvider, globalEnvironment)
        {
        }

        /// <summary>
        /// Samples the command.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A <see cref="IActionResult"/></returns>
        [HttpPut]
        [Route("TransactionScopeTest()")]
        public async Task<IActionResult> TransactionScopeTest([FromBody] ODataActionParameters value)
        {
            if (!ModelState.IsValid || value == null)
            {
                return new BadRequestObjectResult(ModelState);
            }

            if (!value.ContainsKey("innerError") || string.IsNullOrEmpty(value["innerError"]?.ToString()))
            {
                return new BadRequestObjectResult(value);
            }

            if (!value.ContainsKey("outerError") || string.IsNullOrEmpty(value["outerError"]?.ToString()))
            {
                return new BadRequestObjectResult(value);
            }

            if (!value.ContainsKey("outerErrorBeforeInner") || string.IsNullOrEmpty(value["outerErrorBeforeInner"]?.ToString()))
            {
                return new BadRequestObjectResult(value);
            }

            if (!value.ContainsKey("newContext") || string.IsNullOrEmpty(value["newContext"]?.ToString()))
            {
                return new BadRequestObjectResult(value);
            }

            if (!value.ContainsKey("transactionScopeOption") || string.IsNullOrEmpty(value["transactionScopeOption"]?.ToString()))
            {
                return new BadRequestObjectResult(value);
            }

            bool.TryParse(value["innerError"].ToString(), out var innerError);
            bool.TryParse(value["outerError"].ToString(), out var outerError);
            bool.TryParse(value["outerErrorBeforeInner"].ToString(), out var outerErrorBeforeInner);
            bool.TryParse(value["newContext"].ToString(), out var newContext);
            var command = Command<OuterCommand>();
            var result = await command.Process(CurrentContext, innerError, outerError, outerErrorBeforeInner, newContext, value["transactionScopeOption"].ToString()).ConfigureAwait(false);

            return new ObjectResult(command);
        }
    }
}
