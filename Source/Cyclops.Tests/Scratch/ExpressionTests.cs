using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PetStore.Domain;

namespace Cyclops.UnitTest
{
    [TestClass]
    public class ExpressionTests
    {
        [TestMethod]
        public void CreateSetterFromGetter()
        {
            Action<Person, int> ageSetter = InitializeSet((Person p) => p.Age);
            Action<Person, string> nameSetter = InitializeSet((Person p) => p.Name);

            Person p1 = new Person();
            ageSetter(p1, 29);
            nameSetter(p1, "John");

            Assert.IsTrue(p1.Name == "John");
            Assert.IsTrue(p1.Age == 29);
        }

        [TestMethod]
        public void CreateSetterFromGetter2()
        {
            var p1 = new Person();
            SetIfNotNull(p1, p => p.Name, "magic");
            Assert.AreEqual("tada!", p1.Name);

            SetIfNotNull(p1, p => p.Name, "foo3333");
            Assert.AreEqual("foo3333", p1.Name);

            
        }

        public class Person { public int Age { get; set; } public string Name { get; set; } }

        public static Action<TContainer, TProperty> InitializeSet<TContainer, TProperty>(Expression<Func<TContainer, TProperty>> getter)
        {
            PropertyInfo propertyInfo = (getter.Body as MemberExpression).Member as PropertyInfo;

            ParameterExpression instance = Expression.Parameter(typeof(TContainer), "instance");
            ParameterExpression parameter = Expression.Parameter(typeof(TProperty), "param");

            var action = Expression.Lambda<Action<TContainer, TProperty>>(
                Expression.Call(instance, propertyInfo.GetSetMethod(), parameter),
                new [] { instance, parameter }).Compile();

            return action;
        }

        public static void SetIfNotNull<Person, TProperty>(Person target, Expression<Func<Person, TProperty>> getter, string settingKey)
        {
            var propertyInfo = (getter.Body as MemberExpression).Member as PropertyInfo;

            ParameterExpression instance = Expression.Parameter(typeof(Person), "instance");
            ParameterExpression parameter = Expression.Parameter(typeof(TProperty), "param");

            Action<Person, string> action = Expression.Lambda<Action<Person, string>>(
                Expression.Call(instance, propertyInfo.GetSetMethod(), parameter),
                new[] { instance, parameter }).Compile();

            if (settingKey == "magic")
            {
                action(target, "tada!");
            }
            else
            {
                action(target, settingKey);
            }
        }


    }
}
