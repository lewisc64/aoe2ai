using Language.ScriptItems;
using System.Collections.Generic;

namespace Language.Rules
{
    [ActiveRule]
    public class Delay : RuleBase
    {
        public override string Name => "do once";

        public override string Help => "Block body is only allowed to trigger after the time is up.";

        public override string Usage => @"#delay by AMOUNT TIME_UNIT
    RULES
#end delay";

        public override IEnumerable<string> Examples => new[]
        {
            @"#delay by 30 seconds
    chat to all ""30 seconds has passed.""
#end delay",
            @"#delay by 30 minutes
    chat to all ""30 minutes has passed.""
#end delay",
        };

        public Delay()
            : base(@"^(?:#delay by (?<amount>[^ ]+) (?<unit>seconds?|minutes?|hours?)|#end delay)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            if (line.StartsWith("#delay"))
            {
                var data = GetData(line);
                var amount = int.Parse(data["amount"].Value);
                var unit = data["unit"].Value.TrimEnd('s');

                switch (unit)
                {
                    case "minute": amount *= 60; break;
                    case "hour": amount *= 3600; break;
                }

                var timerNumber = context.CreateTimer();

                context.AddToScript(context.ApplyStacks(new Defrule(new[] { "true" }, new[] { $"enable-timer {timerNumber} {amount}", "disable-self" })));

                context.ConditionStack.Push(new Condition($"timer-triggered {timerNumber}"));
            }
            else
            {
                context.ConditionStack.Pop();
            }
        }
    }
}
