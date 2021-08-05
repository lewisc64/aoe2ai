using Language.Rules;
using Language.ScriptItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Language
{
    public class Transpiler
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public List<IRule> Rules { get; } = new List<IRule>();

        public Transpiler()
        {
            Rules.Add(new Snippet("rule",
                new[] { "true" },
                new[] { "do-nothing" }));

            Rules.Add(new Snippet("take boar",
                new[] { "true" },
                new[] { "set-strategic-number sn-enable-boar-hunting 2",
                        "set-strategic-number sn-minimum-number-hunters 3",
                        "set-strategic-number sn-minimum-boar-lure-group-size 3",
                        "set-strategic-number sn-minimum-boar-hunt-group-size 3" }));

            Rules.Add(new Snippet("take boar and deer",
                new[] { "true" },
                new[] { "set-strategic-number sn-enable-boar-hunting 1",
                        "set-strategic-number sn-minimum-number-hunters 3",
                        "set-strategic-number sn-minimum-boar-lure-group-size 3",
                        "set-strategic-number sn-minimum-boar-hunt-group-size 3" }));

            Rules.Add(new Snippet("set up new building system",
                new[] { "true" },
                new[] { "set-strategic-number sn-enable-new-building-system 1",
                        "set-strategic-number sn-percent-building-cancellation 20",
                        "set-strategic-number sn-cap-civilian-builders 200" }));

            Rules.Add(new Snippet("set up scouting",
                new[] { "true" },
                new[] { "set-strategic-number sn-percent-civilian-explorers 0",
                        "set-strategic-number sn-cap-civilian-explorers 0",
                        "set-strategic-number sn-total-number-explorers 1",
                        "set-strategic-number sn-number-explore-groups 1",
                        "set-strategic-number sn-initial-exploration-required 0" }));

            Rules.Add(new Snippet("set up micro",
                new[] { "true" },
                new[] { "set-difficulty-parameter ability-to-maintain-distance 0",
                        "set-difficulty-parameter ability-to-dodge-missiles 0",
                        "set-strategic-number sn-percent-attack-soldiers 100",
                        "set-strategic-number sn-percent-attack-boats 100",
                        "set-strategic-number sn-attack-intelligence 1",
                        "set-strategic-number sn-livestock-to-town-center 1",
                        "set-strategic-number sn-enable-patrol-attack 1",
                        "set-strategic-number sn-intelligent-gathering 1",
                        "set-strategic-number sn-local-targeting-mode 1",
                        "set-strategic-number sn-retask-gather-amount 0",
                        "set-strategic-number sn-target-evaluation-siege-weapon 500",
                        "set-strategic-number sn-ttkfactor-scalar 500",
                        "set-strategic-number sn-percent-enemy-sighted-response 100",
                        "set-strategic-number sn-task-ungrouped-soldiers 0",
                        "set-strategic-number sn-gather-defense-units 1",
                        "set-strategic-number sn-defer-dropsite-update 1",
                        "set-strategic-number sn-do-not-scale-for-difficulty-level 1",
                        "set-strategic-number sn-number-build-attempts-before-skip 5",
                        "set-strategic-number sn-max-skips-per-attempt 5",
                        "set-strategic-number sn-dropsite-separation-distance 8" }));

            Rules.Add(new Snippet("target walls",
                new[] { "true" },
                new[] { "set-strategic-number sn-wall-targeting-mode 1" }));

            Rules.Add(new Snippet("retreat",
                new[] { "true" },
                new[] { "up-retreat-now" }));

            Rules.Add(new Snippet("resign",
                new[] { "true" },
                new[] { "resign" }));
            Rules.Add(new Snippet("drop off food",
                new[] { "true" },
                new[] { "up-drop-resources sheep-food c: 5",
                        "up-drop-resources farm-food c: 5",
                        "up-drop-resources forage-food c: 5",
                        "up-drop-resources deer-food c: 20",
                        "up-drop-resources boar-food c: 10" }));

            Rules.Add(new Snippet("build safety mill",
                new[] { "can-build mill",
                        "building-type-count-total mill == 0",
                        "game-time >= 360" },
                new[] { "build mill" }));

            Rules.Add(new Snippet("build safety mill",
                new[] { "can-build mill",
                        "building-type-count-total mill == 0",
                        "game-time >= 360" },
                new[] { "build mill" }));

            Rules.Add(new Snippet("delete walls",
                new[] { "true" },
                new[] { "delete-building stone-wall-line",
                        "delete-building palisade-wall",
                        "delete-building gate" }));

            Rules.Add(new Snippet("buy wood",
                new[] { "can-buy-commodity wood" },
                new[] { "buy-commodity wood" }));

            Rules.Add(new Snippet("buy food",
                new[] { "can-buy-commodity food" },
                new[] { "buy-commodity food" }));

            Rules.Add(new Snippet("buy stone",
                new[] { "can-buy-commodity stone" },
                new[] { "buy-commodity stone" }));

            Rules.Add(new Snippet("sell wood",
                new[] { "can-sell-commodity wood" },
                new[] { "sell-commodity wood" }));

            Rules.Add(new Snippet("sell food",
                new[] { "can-sell-commodity food" },
                new[] { "sell-commodity food" }));

            Rules.Add(new Snippet("sell stone",
                new[] { "can-sell-commodity stone" },
                new[] { "sell-commodity stone" }));

            Rules.Add(new SnippetCollection(
                "set up basics",
                (Snippet)Rules.First(x => x.Name == "set up new building system"),
                (Snippet)Rules.First(x => x.Name == "set up scouting"),
                (Snippet)Rules.First(x => x.Name == "set up micro")));

            Rules.Add(new SnippetCollection(
                "lure boars",
                new Snippet(
                    null,
                    new[] { "true" },
                    new[] {
                        "set-strategic-number sn-enable-boar-hunting 2",
                        "set-strategic-number sn-minimum-number-hunters 1",
                        "set-strategic-number sn-minimum-boar-lure-group-size 1",
                        "set-strategic-number sn-minimum-boar-hunt-group-size 1",
                        "set-strategic-number sn-maximum-hunt-drop-distance 48",
                        "disable-self",
                    }),
                new Snippet(
                    null,
                    new[] { "dropsite-min-distance live-boar < 4" },
                    new[] {
                        "up-request-hunters c: 8",
                        "set-strategic-number sn-minimum-number-hunters 8",
                    }),
                new Snippet(
                    null,
                    new[] {
                        "strategic-number sn-minimum-number-hunters == 8",
                        "and (dropsite-min-distance live-boar > 4) (or (dropsite-min-distance boar-food > 4) (dropsite-min-distance boar-food == -1))",
                    },
                    new[] {
                        "set-strategic-number sn-minimum-number-hunters 1",
                        "up-retask-gatherers food c: 255",
                    })
                ));

            Rules.AddRange(Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(ActiveRule)))
                .OrderByDescending(x => (int)x.GetCustomAttributesData().First(x => x.AttributeType == typeof(ActiveRule)).ConstructorArguments.First().Value)
                .Select(x => (IRule)Activator.CreateInstance(x)));
        }

        public string Transpile(string source)
        {
            return string.Join("\n", Transpile(source, new TranspilerContext()));
        }

        public IEnumerable<IScriptItem> Transpile(string source, TranspilerContext context, bool suppressStackWarnings = false)
        {
            var withinSubroutine = false;
            string subroutineName = null;
            string subroutine = null;

            var lineNumber = 1;
            foreach (var line in source.Split(new[] { '\r', '\n' }).Select(x => x.Trim()))
            {
                var lineNoComments = line.Split("//").First().Trim();

                if (string.IsNullOrEmpty(lineNoComments))
                {
                    lineNumber++;
                    continue;
                }

                if (line.StartsWith("#subroutine "))
                {
                    withinSubroutine = true;
                    subroutineName = line.Split(' ').Last();
                    subroutine = "";
                }
                else if (withinSubroutine && line.StartsWith("#end subroutine"))
                {
                    context.Subroutines[subroutineName] = subroutine;
                    withinSubroutine = false;
                }
                else if (withinSubroutine)
                {
                    subroutine += line + "\n";
                }
                else
                {
                    var matched = false;
                    foreach (var rule in Rules)
                    {
                        if (rule.Match(lineNoComments))
                        {
                            matched = true;
                            try
                            {
                                rule.Parse(lineNoComments, context);
                            }
                            catch (Exception ex)
                            {
                                Logger.Error($"Exception occurred while parsing line {lineNumber}: {ex.Message}");
                            }
                        }
                    }

                    if (!matched)
                    {
                        Logger.Warn($"Line {lineNumber} did not match: {line}");
                    }
                }

                lineNumber++;
            }

            if (!suppressStackWarnings)
            {
                if (context.ConditionStack.Any())
                {
                    Logger.Error($"Transpiling of '{context.CurrentFileName}' finished with a populated condition stack: {{ {string.Join(", ", context.ConditionStack)} }}");
                }

                if (context.ActionStack.Any())
                {
                    Logger.Error($"Transpiling of '{context.CurrentFileName}' finished with a populated action stack: {{ {string.Join(", ", context.ActionStack)} }}");
                }

                if (context.DataStack.Any())
                {
                    Logger.Error($"Transpiling of '{context.CurrentFileName}' finished with a populated internal data stack: {{ {string.Join(", ", context.DataStack)} }}");
                }
            }

            context.OptimizeScript();

            return new List<IScriptItem>(context.Script);
        }
    }
}
