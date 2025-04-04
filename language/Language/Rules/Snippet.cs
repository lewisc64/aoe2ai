﻿using Language.ScriptItems;
using System.Collections.Generic;
using System.Linq;

namespace Language.Rules
{
    public class Snippet : RuleBase
    {
        protected virtual IEnumerable<Condition> Conditions { get; }

        protected virtual IEnumerable<Action> Actions { get; }

        public Snippet(string trigger, IEnumerable<string> conditions, IEnumerable<string> actions)
            : this(trigger, conditions.Select(x => new Condition(x)), actions.Select(x => new Action(x)))
        {
        }

        public Snippet(string trigger, IEnumerable<Condition> conditions, IEnumerable<Action> actions)
            : base($@"^{trigger}$")
        {
            Name = trigger;
            Usage = trigger;
            Examples = new[] { trigger };
            Conditions = conditions;
            Actions = actions;
        }

        public override void Parse(string line, TranspilerContext context)
        {
            context.AddToScript(context.ApplyStacks(new Defrule(Conditions, Actions)));
        }

        public static IEnumerable<Snippet> GetSnippets()
        {
            var rules = new List<Snippet>();

            rules.Add(new Snippet("rule",
                new[] { "true" },
                new[] { "do-nothing" }));

            rules.Add(new Snippet("take boar",
                new[] { "true" },
                new[] { "set-strategic-number sn-enable-boar-hunting 2",
                        "set-strategic-number sn-minimum-number-hunters 3",
                        "set-strategic-number sn-minimum-boar-lure-group-size 3",
                        "set-strategic-number sn-minimum-boar-hunt-group-size 3" }));

            rules.Add(new Snippet("take boar and deer",
                new[] { "true" },
                new[] { "set-strategic-number sn-enable-boar-hunting 1",
                        "set-strategic-number sn-minimum-number-hunters 3",
                        "set-strategic-number sn-minimum-boar-lure-group-size 3",
                        "set-strategic-number sn-minimum-boar-hunt-group-size 3" }));

            rules.Add(new Snippet("set up new building system",
                new[] { "true" },
                new[] { "set-strategic-number sn-enable-new-building-system 1",
                        "set-strategic-number sn-percent-building-cancellation 20",
                        "set-strategic-number sn-cap-civilian-builders 200",
                        "disable-self" }));

            rules.Add(new Snippet("set up scouting",
                new[] { "true" },
                new[] { "set-strategic-number sn-percent-civilian-explorers 0",
                        "set-strategic-number sn-cap-civilian-explorers 0",
                        "set-strategic-number sn-total-number-explorers 1",
                        "set-strategic-number sn-number-explore-groups 1",
                        "set-strategic-number sn-initial-exploration-required 0",
                        "set-strategic-number sn-wild-animal-exploration 1",
                        "disable-self" }));

            rules.Add(new Snippet("set up micro",
                new[] { "true" },
                new[] { "set-difficulty-parameter ability-to-maintain-distance 0",
                        "set-difficulty-parameter ability-to-dodge-missiles 100",
                        "set-strategic-number sn-percent-attack-soldiers 100",
                        "set-strategic-number sn-percent-attack-boats 100",
                        "set-strategic-number sn-livestock-to-town-center 1",
                        "set-strategic-number sn-task-ungrouped-soldiers 0",
                        "set-strategic-number sn-gather-defense-units 1",
                        "set-strategic-number sn-defer-dropsite-update 1",
                        "set-strategic-number sn-do-not-scale-for-difficulty-level 1",
                        "set-strategic-number sn-dropsite-separation-distance 8",
                        "set-strategic-number sn-minimum-water-body-size-for-dock 1000",
                        "set-strategic-number sn-consecutive-idle-unit-limit 1",
                        "set-strategic-number sn-zero-priority-distance 255",
                        "set-strategic-number sn-scale-minimum-attack-group-size 0",
                        "set-strategic-number sn-garrison-rams 0",
                        "set-strategic-number sn-disable-villager-garrison 2",
                        "set-strategic-number sn-disable-trade-evasion 1",
                        "set-strategic-number sn-ignore-tower-elevation 1",
                        "set-strategic-number sn-percent-enemy-sighted-response 100",
                        "set-strategic-number sn-attack-intelligence 1",
                        "set-strategic-number sn-wall-targeting-mode 1",
                        "set-strategic-number sn-local-targeting-mode 1",
                        "set-strategic-number sn-enable-patrol-attack 1",
                        "set-strategic-number sn-ttkfactor-scalar 300",
                        "set-strategic-number sn-disable-tower-priority 1",
                        "set-strategic-number sn-enable-offensive-priority 1",
                        "up-reset-target-priorities priority-offense 1",
                        "disable-self" }));

            rules.Add(new Snippet("set up distances",
                new[] { "true" },
                new[] { "set-strategic-number sn-maximum-wood-drop-distance -1",
                        "set-strategic-number sn-maximum-food-drop-distance 8",
                        "set-strategic-number sn-maximum-gold-drop-distance 8",
                        "set-strategic-number sn-maximum-stone-drop-distance 8",
                        "set-strategic-number sn-maximum-hunt-drop-distance 48",
                        "set-strategic-number sn-mill-max-distance 25",
                        "set-strategic-number sn-camp-max-distance 25",
                        "set-strategic-number sn-dropsite-separation-distance 5",
                        "set-strategic-number sn-allow-adjacent-dropsites 1",
                        "disable-self" }));

            rules.Add(new SnippetCollection(
                "set up basics",
                rules.First(x => x.Name == "set up new building system"),
                rules.First(x => x.Name == "set up micro"),
                rules.First(x => x.Name == "set up distances"),
                rules.First(x => x.Name == "set up scouting")));

            rules.Add(new Snippet("target walls",
                new[] { "true" },
                new[] { "set-strategic-number sn-wall-targeting-mode 1" }));

            rules.Add(new Snippet("retreat",
                new[] { "true" },
                new[] { "up-retreat-now" }));

            rules.Add(new Snippet("resign",
                new[] { "true" },
                new[] { "resign" }));

            rules.Add(new Snippet("drop off food",
                new[] { "true" },
                new[] { "up-drop-resources sheep-food c: 5",
                        "up-drop-resources farm-food c: 5",
                        "up-drop-resources forage-food c: 5",
                        "up-drop-resources deer-food c: 20",
                        "up-drop-resources boar-food c: 10" }));

            rules.Add(new Snippet("delete walls",
                new[] { "true" },
                new[] { "delete-building stone-wall-line",
                        "delete-building palisade-wall" }
                    .Concat(Game.AllClosedGateIds.Select(x => $"delete-building {x}"))));

            rules.Add(new Snippet("buy wood",
                new[] { "can-buy-commodity wood" },
                new[] { "buy-commodity wood" }));

            rules.Add(new Snippet("buy food",
                new[] { "can-buy-commodity food" },
                new[] { "buy-commodity food" }));

            rules.Add(new Snippet("buy stone",
                new[] { "can-buy-commodity stone" },
                new[] { "buy-commodity stone" }));

            rules.Add(new Snippet("sell wood",
                new[] { "can-sell-commodity wood" },
                new[] { "sell-commodity wood" }));

            rules.Add(new Snippet("sell food",
                new[] { "can-sell-commodity food" },
                new[] { "sell-commodity food" }));

            rules.Add(new Snippet("sell stone",
                new[] { "can-sell-commodity stone" },
                new[] { "sell-commodity stone" }));

            return rules;
        }
    }
}
