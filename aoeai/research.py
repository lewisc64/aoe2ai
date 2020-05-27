
def create_merged_research(research, new_key, keys):
    research[new_key] = []
    for key in keys:
        research[new_key].extend(research[key])

def research_exists(name):
    return name in research["all"]

# will only contain basic non-unique research. used for making rules, not verifying them.
research = {
    "blacksmith ranged": ["fletching", "bodkin-arrow", "bracer", "padded-archer-armor", "leather-archer-armor", "ring-archer-armor"],
    "blacksmith infantry": ["scale-mail", "chain-mail", "plate-mail"],
    "blacksmith cavalry": ["scale-barding", "chain-barding", "plate-barding"],
    "lumber camp": ["double-bit-axe", "bow-saw", "two-man-saw"],
    "mill": ["horse-collar", "heavy-plow", "crop-rotation"],
    "market": ["coinage", "banking", "guilds", "caravan"],
    "mining camp gold": ["gold-mining", "gold-shaft-mining"],
    "mining camp stone": ["stone-mining", "stone-shaft-mining"],
    "barracks": ["tracking", "squires", "arson", "man-at-arms", "long-swordsman", "two-handed-swordsman", "champion", "pikeman", "halberdier", "eagle-warrior", "elite-eagle-warrior"],
    "archery range": ["thumb-ring", "parthian-tactics", "crossbow", "arbalest", "elite-skirmisher", "imperial-skirmisher", "heavy-cavalry-archer"],
    "stable": ["bloodlines", "husbandry", "light-cavalry", "hussar", "cavalier", "paladin", "heavy-camel", "elite-battle-elephant"],
    "dock": ["gillnets", "careening", "dry-dock", "shipwright", "war-galley", "fast-fire-ship", "heavy-demolition-ship", "cannon-galleon", "galleon"],
    "siege workshop": ["capped-ram", "siege-ram", "onager", "siege-onager", "heavy-scorpion"],
    "monastery": ["redemption", "atonement", "herbal-medicine", "heresy", "sanctity", "fervor", "faith", "illumination", "block-printing", "theocracy"],
    "castle": ["my-unique-unit-upgrade", "my-unique-research", "hoardings", "sappers", "conscription", "spies"],
    "town center": ["feudal-age", "castle-age", "imperial-age", "loom", "town-watch", "wheel-barrow", "town-patrol", "hand-cart"],
    "university": ["masonry", "fortified-wall", "ballistics", "guard-tower", "heated-shot", "murder-holes", "treadmill-crane", "architecture", "chemistry", "siege-engineers", "keep", "arrowslits", "bombard-tower"]
}
common_inf_cav = ["forging", "iron-casting", "blast-furnace"]
research["blacksmith"] = sum([research["blacksmith " + name] for name in ["ranged", "infantry", "cavalry"]], [])
research["blacksmith"].extend(common_inf_cav)
research["blacksmith infantry"].extend(common_inf_cav)
research["blacksmith cavalry"].extend(common_inf_cav)

# stuff that isn't present in the ai engine for some reason
research["university"].remove("treadmill-crane")
research["monastery"].remove("herbal-medicine")
research["castle"].remove("spies")

for key in research:
    for i, value in enumerate(research[key]):
        if not value.startswith("my-") and not value.endswith("-age"):
            research[key][i] = "ri-{}".format(value)

create_merged_research(research, "all", research.keys())
create_merged_research(research, "mining camp", ["mining camp gold", "mining camp stone"])
create_merged_research(research, "gold mining camp", ["mining camp gold"])
create_merged_research(research, "stone mining camp", ["mining camp stone"])
