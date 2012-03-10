using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace TheSprocker.Core.Mapping
{
    /// <summary>
    /// Expression tree to build SqlParameters
    /// </summary>
    internal class ParameterExpressionTree
    {

        /// <summary>
        /// Delete! 
        /// </summary>
        public void usageStub()
        {

             //Action<Person, string> nameSetter = InitializeSet((Person p) => p.Name);
            //Action<Person, int> ageSetter = InitializeSet((Person p) => p.Age);
            //Action<Person, string> nameSetter = InitializeSet((Person p) => p.Name);

            //Person p1 = new Person();
            //ageSetter(p1, 29);
            //nameSetter(p1, "John");


            // bits 

            // SqlParameter collection
            // Type member collection

            // fill the SQL parameters up with 

        }


        public static Action<TContainer, TProperty> InitializeSet<TContainer, TProperty>(Expression<Func<TContainer, TProperty>> getter)
        {
            PropertyInfo propertyInfo = (getter.Body as MemberExpression).Member as PropertyInfo;

            ParameterExpression instance = Expression.Parameter(typeof(TContainer), "instance");
            ParameterExpression parameter = Expression.Parameter(typeof(TProperty), "param");

            
            return Expression.Lambda<Action<TContainer, TProperty>>(
                Expression.Call(instance, propertyInfo.GetSetMethod(), parameter),
                new ParameterExpression[] { instance, parameter }).Compile();

        }
    }
}
