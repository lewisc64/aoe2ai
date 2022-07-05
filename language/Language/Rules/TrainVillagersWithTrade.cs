using Language.ScriptItems;
using System.Collections.Generic;

namespace Language.Rules
{
    [ActiveRule]
    public class TrainVillagersWithTrade : RuleBase
    {
        public override string Name => "train villagers with trade";

        public override string Help => "Trains the specified amount of villagers, with a portion of that being trade carts if it is a team game.";

        public override string Usage => @"train NUMBER villagers with NUMBER trade";

        public override IEnumerable<string> Examples => new[]
        {
            "train 120 villagers with 30 trade",
            "train 120 villagers with 30 trade using escrow for caravan",
        };

        public TrainVillagersWithTrade()
            : base(@"^train (?<villamount>[^ ]+) villagers? with (?<tradeamount>[^ ]+) trade(?<useescrow> using escrow for caravan)?$")
        {
        }

        public override void Parse(string line, TranspilerContext context)
        {
            var data = GetData(line);
            var civilianTotal = data["villamount"].Value;
            var tradeAmount = data["tradeamount"].Value;
            var useEscrow = data["useescrow"].Success;

            var rules = new List<Defrule>();

            var villagerTargetGoal = context.CreateVolatileGoal();
            var currentVillagersGoal = context.CreateVolatileGoal();
            var currentPopulationGoal = context.CreateVolatileGoal();
            var populationRemainingGoal = context.CreateVolatileGoal();

            rules.Add(new Defrule(
                new[] {
                    "true",
                },
                new[] {
                    $"up-get-fact unit-type-count-total villager {currentVillagersGoal}",
                    $"set-goal {villagerTargetGoal} {civilianTotal}",
                }));

            rules.Add(new Defrule(
                new[] {
                    "players-building-type-count any-ally market >= 1",
                },
                new[] {
                    $"up-modify-goal {villagerTargetGoal} c:- {tradeAmount}",
                }));

            rules.Add(new Defrule(
                new[] {
                    "can-train villager",
                    $"up-compare-goal {currentVillagersGoal} g:< {villagerTargetGoal}",
                },
                new[] {
                    "train villager",
                }));

            rules.Add(new Defrule(
                new[] {
                    "players-building-type-count any-ally market >= 1",
                    "can-build market",
                    "building-type-count-total market < 2",
                },
                new[] {
                    "build market",
                }));

            if (useEscrow)
            {
                rules.Add(new Defrule(
                    new[] {
                        "players-building-type-count any-ally market >= 1",
                        "can-research-with-escrow ri-caravan",
                    },
                    new[] {
                        "release-escrow food",
                        "release-escrow gold",
                        "research ri-caravan",
                    }));
            }
            else
            {
                rules.Add(new Defrule(
                    new[] {
                        "players-building-type-count any-ally market >= 1",
                        "can-research ri-caravan",
                    },
                    new[] {
                        "research ri-caravan",
                    }));
            }

            rules.Add(new Defrule(
                new[] {
                    "players-building-type-count any-ally market >= 1",
                    $"unit-type-count {Game.DeadTradeCartId} == 0",
                    $"unit-type-count {Game.DeadLoadedTradeCartId} == 0",
                    "can-train trade-cart",
                    $"unit-type-count-total trade-cart < {tradeAmount}",
                },
                new[] {
                    "train trade-cart",
                }));

            rules.Add(new Defrule(
                new[] {
                    "true",
                },
                new[] {
                    $"up-get-fact population 0 {currentPopulationGoal}",
                    $"up-get-fact population-cap 0 {populationRemainingGoal}",
                    $"up-modify-goal {populationRemainingGoal} g:- {currentPopulationGoal}",
                }));

            rules.Add(new Defrule(
                new[] {
                    $"up-compare-goal {populationRemainingGoal} c:< 10",
                    $"civilian-population >= {civilianTotal}",
                },
                new[] {
                    "delete-unit villager",
                }));

            context.AddToScript(context.ApplyStacks(rules));

            context.FreeVolatileGoals(new[] { villagerTargetGoal, currentVillagersGoal, currentPopulationGoal, populationRemainingGoal });
        }
    }
}
