using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Sprocker.Core.FluentInterface;
using Sprocker.Core.FluentInterface.Behaviours;

namespace Sprocker.Core
{
    public static class ExtensionMethods
    {
        public static INodeBuilder OutputMapper<TResult>(this INodeBuilder sprockerBuilder, IRowMapper<TResult> rowMapper)
        {
            return sprockerBuilder;
        }

        public static IChildNodeBuilder MapChildNode<TResult, TMember>(this IChildNodeBuilder sprockerBuilder, Expression<Func<TResult, TMember>> propertySelector)
        {
            return sprockerBuilder;
        }
    }
}
