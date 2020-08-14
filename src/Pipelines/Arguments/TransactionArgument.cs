// © 2019 Sitecore Corporation A/S. All rights reserved. Sitecore® is a registered trademark of Sitecore Corporation A/S.

using Sitecore.Commerce.Core;
using System.Transactions;

namespace Ajsuth.Feature.TransactionScopes.Engine
{
    public class TransactionArgument : PipelineArgument
    {
        public TransactionArgument(bool errorOnInnerScope, bool errorOnOuterScope, bool errorBeforeInnerScope, bool newContext, TransactionScopeOption transactionScopeOption)
        {
            ErrorOnInnerScope = errorOnInnerScope;
            ErrorOnOuterScope = errorOnOuterScope;
            ErrorBeforeInnerScope = errorBeforeInnerScope;
            NewContext = newContext;
            TransactionScopeOption = transactionScopeOption;
        }
        
        public bool ErrorOnInnerScope { get; set; }

        public bool ErrorOnOuterScope { get; set; }

        public bool ErrorBeforeInnerScope { get; set; }

        public bool NewContext { get; set; }

        public TransactionScopeOption TransactionScopeOption { get; set; }
    }
}
