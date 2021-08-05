using System.Collections.Generic;
using System.Linq;

namespace Language
{
    public static class Game
    {
        public static Dictionary<string, string[]> GetResearches()
        {
            var research = new Dictionary<string, string[]>() {
                { "blacksmith ranged", new[] { "fletching", "bodkin-arrow", "bracer", "padded-archer-armor", "leather-archer-armor", "ring-archer-armor" } },
                { "blacksmith infantry", new[] { "forging", "iron-casting", "blast-furnace", "scale-mail", "chain-mail", "plate-mail" } },
                { "blacksmith cavalry", new[] { "forging", "iron-casting", "blast-furnace", "scale-barding", "chain-barding", "plate-barding" } },
                { "lumber camp", new[] { "double-bit-axe", "bow-saw", "two-man-saw" } },
                { "mill", new[] { "horse-collar", "heavy-plow", "crop-rotation" } },
                { "market", new[] { "coinage", "banking", "guilds", "caravan" } },
                { "mining camp", new[] { "gold-mining", "gold-shaft-mining", "stone-mining", "stone-shaft-mining" } },
                { "mining camp gold", new[] { "gold-mining", "gold-shaft-mining" } },
                { "mining camp stone", new[] { "stone-mining", "stone-shaft-mining" } },
                { "barracks", new[] { "tracking", "squires", "arson", "man-at-arms", "long-swordsman", "two-handed-swordsman", "champion", "pikeman", "halberdier", "eagle-warrior", "elite-eagle-warrior" } },
                { "archery range", new[] { "thumb-ring", "parthian-tactics", "crossbow", "arbalest", "elite-skirmisher", "imperial-skirmisher", "heavy-cavalry-archer" } },
                { "stable", new[] { "bloodlines", "husbandry", "light-cavalry", "hussar", "cavalier", "paladin", "heavy-camel", "elite-battle-elephant" } },
                { "dock", new[] { "gillnets", "careening", "dry-dock", "shipwright", "war-galley", "fast-fire-ship", "heavy-demolition-ship", "cannon-galleon", "galleon" } },
                { "siege workshop", new[] { "capped-ram", "siege-ram", "onager", "siege-onager", "heavy-scorpion" } },
                { "monastery", new[] { "redemption", "atonement", "herbal-medicine", "heresy", "sanctity", "fervor", "faith", "illumination", "block-printing", "theocracy" } },
                { "castle", new[] { "my-unique-unit-upgrade", "my-unique-research", "hoardings", "sappers", "conscription", "spies" } },
                { "town center", new[] { "feudal-age", "castle-age", "imperial-age", "loom", "town-watch", "wheel-barrow", "town-patrol", "hand-cart" } },
                { "university", new[] { "masonry", "fortified-wall", "ballistics", "guard-tower", "heated-shot", "murder-holes", "treadmill-crane", "architecture", "chemistry", "siege-engineers", "keep", "arrowslits", "bombard-tower" } },
            };

            research["blacksmith"] = research["blacksmith ranged"]
                .Concat(research["blacksmith infantry"]
                .Concat(research["blacksmith cavalry"]))
                .Distinct()
                .ToArray();

            // research that is not present in the ai engine.
            research["university"] = research["university"].Where(x => x != "treadmill-crane").ToArray();
            research["monastery"] = research["monastery"].Where(x => x != "herbal-medicine").ToArray();
            research["castle"] = research["castle"].Where(x => x != "spies").ToArray();

            var all = new List<string>();

            foreach (var key in research.Keys)
            {
                research[key] = research[key]
                    .Select(x => x.EndsWith("-age") ? x : $"ri-{x}")
                    .ToArray();

                all.AddRange(research[key]);
            }

            research["all"] = all.Distinct().ToArray();

            // aliases
            research["ranged blacksmith"] = research["blacksmith ranged"];
            research["cavalry blacksmith"] = research["blacksmith cavalry"];
            research["infantry blacksmith"] = research["blacksmith infantry"];
            research["gold mining camp"] = research["mining camp gold"];
            research["stone mining camp"] = research["mining camp stone"];

            return research;
        }
    }
}
