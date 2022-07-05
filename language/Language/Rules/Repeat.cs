using Language.ScriptItems;
using System.Collections.Generic;

namespace Language.Rules
{
    [ActiveRule]
    public class Repeat : RuleBase
    {
        public override string Name => "repeat";

        public override string Help => "Each rule is allowed to be triggered once after the time has elapsed, the process repeats.";

        public override string Usage => @"#repeat every AMOUNT TIME_UNIT
    RULES
#end repeat";

        public override IEnumerable<string> Examples => new[]
        {
            @"#repeat every 30 seconds
    chat to all ""hello""
#end repeat",
        };

        public Repeat()
            : base(@"^(?:#repeat every (?<amount>[^ ]+) (?<unit>seconds?|minutes?|hours?)|#end repeat)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            if (line.StartsWith("#repeat"))
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
                context.DataStack.Push(timerNumber);
                context.DataStack.Push(amount);
            }
            else
            {
                var amount = context.DataStack.Pop();
                var timerNumber = context.DataStack.Pop();
                context.ConditionStack.Pop();

                context.AddToScript(context.ApplyStacks(new Defrule(new[] { $"timer-triggered {timerNumber}" }, new[] { $"disable-timer {timerNumber}", $"enable-timer {timerNumber} {amount}" })));
            }
        }
    }
}
