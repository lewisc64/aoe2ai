using Language.ScriptItems;

namespace Language.Rules
{
    [ActiveRule]
    public class Repeat : RuleBase
    {
        public override string Name => "do once";

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
