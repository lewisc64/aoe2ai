using Language;
using Language.ScriptItems;
using System;

namespace Aoe2AI
{
    class Program
    {
        static void Main(string[] args)
        {
            var t = new Transpiler();
            Console.WriteLine(t.Transpile(@"

#add condition current-age == feudal-age
    #add action research ri-loom
        rule
    #remove action
#remove condition

#add condition current-age == feudal-age
    #add action build town-center
        rule
    #remove action
#remove condition

"));

            var rule = new Defrule(
                new Condition[] {
                    new CombinatoryCondition("or", new[] {
                        new Condition("current-age == dark-age"),
                        new Condition("current-age == feudal-age"),
                    }).Invert(),
                    new Condition("can-build barracks"),
                },
                new[] {
                    new Language.ScriptItems.Action("build barracks"),
                });

            Console.WriteLine(rule);
            // Console.WriteLine(rule.Length);

            Console.WriteLine(Condition.Parse("goal produce-scouts 1 and research-completed ri-light-cavalry and not(research-available ri-hussar) and up-research-status c: ri-hussar < research-pending"));
        }
    }
}
