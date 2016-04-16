using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using R = System.Linq.Expressions.Expression;
using System.Linq.Dynamic;
using System.Reflection;

namespace ExpressionParser
{
    public class Program
    {
        static void Main(string[] args)
        {
            Type pd = Type.GetType("ExpressionParser.Person");
            var fooParameter = R.Parameter(typeof(Person));
            var valueParameter = R.Parameter(typeof(string));
            var propertyInfo = typeof(Person).GetProperty("Name");
            var assignment = R.Assign(R.MakeMemberAccess(fooParameter, propertyInfo), valueParameter);
            var assign = R.Lambda<Action<Person, string>>(assignment, fooParameter, valueParameter);
            Action<Person, string> fnSet = assign.Compile();


            const string exp1 = @"Person.Name="+"Chetan";
            const string exp2 = @"Person.ShowName";
            
            //const string exp = @"(Person.Age > 3 AND Person.Weight > 50) OR Person.Age < 3";
            var variableExpr = R.Variable(typeof(string), "Person.Name");
            var assignExpr = R.Assign(
                variableExpr,
                R.Constant("Chetan")
                );
            var p = R.Parameter(typeof(Person),"Person");
            //assignExpr 

            var bob = new Person
            {
                Name = "Bob",
                Age = 30,
                Weight = 21,
                FavouriteDay = new DateTime(2000, 1, 1)
            };

            var e = R.Lambda<Func<string>>(assignExpr);
            var exp = e.Body as System.Linq.Expressions.MemberExpression;

            (exp.Member as PropertyInfo).SetValue(bob,"Chetan",null);

            var f = DynamicExpression.ParseLambda(new[] { p }, null, exp2);
            
            var result1 = e;
            var result2 = f.Compile().DynamicInvoke(bob);
            //Console.WriteLine(result1);
            Console.WriteLine(result2);
            Console.ReadKey();

          
        }

       
    }

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public int Weight { get; set; }
        public DateTime FavouriteDay { get; set; }
        public string ShowName
        {
            get { return show(); }

            set { Name = value; }
        }

        public string show()
        {
            return Name;
        }
    }
}
