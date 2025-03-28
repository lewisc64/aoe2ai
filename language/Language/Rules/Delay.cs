using Language.ScriptItems;
using System.Collections.Generic;

namespace Language.Rules
{
    [ActiveRule]
    public class Delay : RuleBase
    {
        public override string Name => "delay";

        public override string Help => "Block body is only allowed to trigger after the time is up.";

        public override string Usage => @"#delay by AMOUNT TIME_UNIT
    RULES
#end delay";

        public override IEnumerable<string> Examples => new[]
        {
            @"#delay by 30 seconds
    chat to all ""30 in-game seconds has passed.""
#end delay",
            @"#delay by 30 minutes
    chat to all ""30 in-game minutes has passed.""
#end delay",
            @"#delay by 5 real seconds
    chat to all ""5 real seconds have passed.""
#end delay",
        };

        public Delay()
            : base(@"^(?:#delay by (?<amount>[^ ]+) (?<real>real )?(?<unit>seconds?|minutes?|hours?)|#end delay)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            if (line.StartsWith("#delay"))
            {
                var data = GetData(line);
                var amount = int.Parse(data["amount"].Value);
                var real = data["real"].Success;
                var unit = data["unit"].Value.TrimEnd('s');

                switch (unit)
                {
                    case "minute": amount *= 60; break;
                    case "hour": amount *= 3600; break;
                }

                if (real)
                {
                    var futureStampGoal = context.CreateGoal();
                    var currentTimeGoal = context.CreateVolatileGoal();

                    var rules = new[]
                    {
                        new Defrule(
                            [
                                "true",
                            ],
                            [
                                $"up-get-precise-time 0 {futureStampGoal}",
                                $"up-modify-goal {futureStampGoal} c:+ {amount * 1000}",
                                $"set-goal {currentTimeGoal} 0",
                                "disable-self",
                            ]),
                        new Defrule(
                            [
                                "true",
                            ],
                            [
                                $"up-get-precise-time 0 {currentTimeGoal}",
                            ]),
                    };
                    
                    context.AddToScript(context.ApplyStacks(rules, ignoreActionStack: true));
                    context.ConditionStack.Push(new Condition($"up-compare-goal {currentTimeGoal} g:>= {futureStampGoal}"));
                    context.DataStack.Push(currentTimeGoal);
                }
                else
                {
                    var timerNumber = context.CreateTimer();
                    context.AddToScript(context.ApplyStacks(new Defrule(["true"], [$"enable-timer {timerNumber} {amount}", "disable-self"])));
                    context.ConditionStack.Push(new Condition($"timer-triggered {timerNumber}"));
                    context.DataStack.Push(null);
                }
            }
            else
            {
                context.ConditionStack.Pop();
                var usedVolatileGoal = context.DataStack.Pop();
                if (usedVolatileGoal is not null)
                {
                    context.FreeVolatileGoal((int)usedVolatileGoal);
                }
            }
        }
    }
}
