using Language.ScriptItems;

namespace Language.Rules
{
    [ActiveRule]
    public class DistributeVillagers : RuleBase
    {
        public override string Name => "distribute villagers";

        public DistributeVillagers()
            : base(@"^distribute villagers (?<wood>[0-9]+) (?<food>[0-9]+) (?<gold>[0-9]+) (?<stone>[0-9]+)$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var wood = data["wood"].Value;
            var food = data["food"].Value;
            var gold = data["gold"].Value;
            var stone = data["stone"].Value;

            var rule = new Defrule(
                new[]
                {
                    "true",
                },
                new[]
                {
                    $"set-strategic-number sn-wood-gatherer-percentage {wood}",
                    $"set-strategic-number sn-food-gatherer-percentage {food}",
                    $"set-strategic-number sn-gold-gatherer-percentage {gold}",
                    $"set-strategic-number sn-stone-gatherer-percentage {stone}",
                });

            context.AddToScript(context.ApplyStacks(rule));
        }
    }
}
