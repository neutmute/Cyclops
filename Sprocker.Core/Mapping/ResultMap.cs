using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Sprocker.Core.Mapping
{
    public class ResultMap<TEntity>
    {
        public List<Expression<Func<TEntity, object>>> CriteriaExpressions { get; set; }

        public ResultMap()
        {
            CriteriaExpressions = new List<Expression<Func<TEntity, object>>>();
        }
    }
}
