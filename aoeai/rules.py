import re
import uuid

from .defrule import *
from .research import *
from . import interpreter

rules = []

def rule(rule):
    global rules
    rules.append(rule())
    return rule

class Rule:
    def __init__(self):
        self.name = ""
        self.regex = None
        self.help = ""
        self.usage = ""
        self.example = ""

    def is_match(self, line):
        return self.regex.match(line)

    def get_data(self, line):
        return self.regex.search(line).groups()

    def parse(self, line, **kwargs):
        pass

class Snippet(Rule):
    def __init__(self, trigger, conditions, actions):
        super().__init__()
        self.name = trigger
        self.regex = re.compile("^{}$".format(trigger))
        self.conditions = conditions
        self.actions = actions
        self.usage = self.name
    
    def parse(self, line, **kwargs):
        return Defrule(self.conditions[:], self.actions[:])

class SnippetCollection(Rule):
    def __init__(self, trigger, snippets):
        self.name = trigger
        self.regex = re.compile("^{}$".format(trigger))
        self.snippets = snippets

    def parse(self, line, **kwargs):
        return [snippet.parse(line, **kwargs) for snippet in self.snippets]

def create_merged_snippet(rules, rule_name, names):
    rules.append(Snippet(rule_name, [], []))
    for rule in rules[:-1]:
        if rule.name in names:
            rules[-1].conditions.extend(rule.conditions)
            rules[-1].actions.extend(rule.actions)

rules.append(Snippet("rule",
                     ["true"],
                     ["do-nothing"]))
rules.append(Snippet("take boar",
                     ["true"],
                     ["set-strategic-number sn-enable-boar-hunting 2",
                      "set-strategic-number sn-minimum-number-hunters 3",
                      "set-strategic-number sn-minimum-boar-lure-group-size 3",
                      "set-strategic-number sn-minimum-boar-hunt-group-size 3"]))
rules.append(Snippet("take boar and deer",
                     ["true"],
                     ["set-strategic-number sn-enable-boar-hunting 1",
                      "set-strategic-number sn-minimum-number-hunters 3",
                      "set-strategic-number sn-minimum-boar-lure-group-size 3",
                      "set-strategic-number sn-minimum-boar-hunt-group-size 3"]))
rules.append(Snippet("set up new building system",
                     ["true"],
                     ["set-strategic-number sn-enable-new-building-system 1",
                      "set-strategic-number sn-percent-building-cancellation 20",
                      "set-strategic-number sn-cap-civilian-builders 200"]))
rules.append(Snippet("set up scouting",
                     ["true"],
                     ["set-strategic-number sn-percent-civilian-explorers 0",
                      "set-strategic-number sn-cap-civilian-explorers 0",
                      "set-strategic-number sn-total-number-explorers 1",
                      "set-strategic-number sn-number-explore-groups 1",
                      "set-strategic-number sn-initial-exploration-required 0"]))
rules.append(Snippet("set up micro",
                     ["true"],
                     ["set-difficulty-parameter ability-to-maintain-distance 0",
                      "set-difficulty-parameter ability-to-dodge-missiles 0",
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
                      "set-strategic-number sn-max-skips-per-attempt 5"]))
rules.append(Snippet("target walls",
                     ["true"],
                     ["set-strategic-number sn-wall-targeting-mode 1"]))
rules.append(Snippet("retreat",
                     ["true"],
                     ["up-retreat-now"]))
rules.append(Snippet("resign",
                     ["true"],
                     ["resign"]))
rules.append(Snippet("drop off food",
                     ["true"],
                     ["up-drop-resources sheep-food c: 5",
                      "up-drop-resources farm-food c: 5",
                      "up-drop-resources forage-food c: 5",
                      "up-drop-resources deer-food c: 20",
                      "up-drop-resources boar-food c: 10"]))
rules.append(Snippet("build safety mill",
                     ["can-build mill",
                      "building-type-count-total mill == 0",
                      "game-time >= 360"],
                     ["build mill"]))

create_merged_snippet(rules, "set up basics", ["set up scouting", "set up new building system", "set up micro"])

@rule
class Comment(Rule):
    def __init__(self):
        super().__init__()
        self.name = "comment"
        self.regex = re.compile("^//(.+)$")
        self.usage = "Place '//' before a line. Cannot be mid-line."
    
    def parse(self, line, **kwargs):
        return None

@rule
class Load(Rule):
    def __init__(self):
        super().__init__()
        self.name = "load"
        self.regex = re.compile("^load \"([^\"]+)\"$")
        self.usage = "load \"PATH\""
        self.help = "Loads another aoe2ai file. Relative to main file if trying to load from a seperate file."
    
    def parse(self, line, **kwargs):
        path = self.get_data(line)[0]
        file = open(path, "r")
        content = file.read()
        file.close()
              
        output = interpreter.interpret(content, timers=kwargs["timers"], goals=kwargs["goals"], constants=kwargs["constants"], userpatch=kwargs["userpatch"])

        rules = []

        for rule in output:
            if isinstance(rule, Defconst):
                kwargs["definitions"].insert(0, rule)
                kwargs["constants"].append(rule.name)
            elif isinstance(rule, Defrule):
                rules.append(rule)

        if not kwargs["condition_stack"]:
            return rules

        for rule in rules:
            rule.compressable = False
            rule.ignore_stacks = True

        goal = len(kwargs["goals"]) + 1
        kwargs["goals"].append(goal)

        jump_amount = len(rules)

        if len(kwargs["condition_stack"]) == 1:
            rules.insert(0, Defrule(["not({})".format(kwargs["condition_stack"][0])], ["up-jump-rule {}".format(jump_amount)], ignore_stacks=True))
        else:
            rules.insert(0, Defrule(["true"], ["set-goal {} {}".format(goal, 1)], ignore_stacks=True))
            rules.insert(1, Defrule(["true"], ["set-goal {} {}".format(goal, 0)]))
            rules.insert(2, Defrule(["goal {} {}".format(goal, 1)], ["up-jump-rule {}".format(jump_amount)], ignore_stacks=True))
        return rules
        
        
@rule
class Cheat(Rule):
    def __init__(self):
        super().__init__()
        self.name = "cheat"
        self.regex = re.compile("^cheat ([^ ]+) ([^ ]+)$")
        self.usage = "cheat RESOURCE_NAME AMOUNT"
        self.help = "Gives the AI resources."

    def parse(self, line, **kwargs):
        amount, resource = self.get_data(line)
        return Defrule(["true"],
                       ["cc-add-resource {} {}".format(resource, amount)])

@rule
class ChatTo(Rule):
    def __init__(self):
        super().__init__()
        self.name = "chat to"
        self.regex = re.compile("^chat to (?:(all|self|allies)|([^ ]+)) (.+)$")
        self.usage = "chat to PLAYER_TYPE \"MESSAGE\""
    
    def parse(self, line, **kwargs):
        group, recipient, content = self.get_data(line)
        if group is not None:
            return Defrule(["true"],
                           ["chat-to-{} {}".format(group, content).replace("to-self", "local-to-self")])
        else:
            return Defrule(["true"],
                           ["chat-to-player {} {}".format(recipient, content)])

@rule
class AddCondition(Rule):
    def __init__(self):
        super().__init__()
        self.name = "add condition"
        self.regex = re.compile("^(?:#add condition (.+)|#remove condition)$")
        self.usage = """#add condition CONDITION
    RULES
#remove condition"""
        self.help = "Adds a condition to the condition stack. 'If' is preferred."
    
    def parse(self, line, **kwargs):
        condition = self.get_data(line)[0]
        if condition is None:
            kwargs["condition_stack"].pop(-1)
        else:
            kwargs["condition_stack"].append(parse_condition(condition))

@rule
class AddAction(Rule):
    def __init__(self):
        super().__init__()
        self.name = "add action"
        self.regex = re.compile("^(?:#add action (.+)|#remove action)$")
        self.usage = """#add action ACTION
    RULES
#remove action"""
        self.help = "Adds an action to the action stack."
    
    def parse(self, line, **kwargs):
        action = self.get_data(line)[0]
        if action is None:
            kwargs["action_stack"].pop(-1)
        else:
            kwargs["action_stack"].append(action)

@rule
class If(Rule):
    def __init__(self):
        super().__init__()
        self.name = "if"
        self.regex = re.compile("^(?:#if (.+)|#else if (.+)|#else|#end if)$")
        self.usage = """#if CONDITION
    RULES
#else if CONDITION
    RULES
#else
    RULES
#end if"""
        self.help = "Adds a condition to the condition stack."
    
    def parse(self, line, **kwargs):
        if line.startswith("#end"):
            amount = kwargs["data_stack"].pop()
            for x in range(amount):
                kwargs["condition_stack"].pop()
            return
        
        condition_if, condition_elseif = self.get_data(line)

        if condition_if is None:

            conditions = [kwargs["condition_stack"].pop() for x in range(kwargs["data_stack"].pop())][::-1]
            conditions[-1] = "not({})".format(conditions[-1])
            
            if condition_elseif is not None:
                conditions.append(parse_condition(condition_elseif))
                
            kwargs["condition_stack"].extend(conditions)
            kwargs["data_stack"].append(len(conditions))
            
        
        else:

            kwargs["condition_stack"].append(parse_condition(condition_if))
            kwargs["data_stack"].append(1)

@rule
class When(Rule):
    def __init__(self):
        super().__init__()
        self.name = "when"
        self.regex = re.compile("^(?:#when( once)?|#then|#end when)$")
        self.usage = """#when
    RULE
#then
    RULES
#end when"""
        self.help = "Rules in the 'then' block are allowed to trigger when any rule in the main 'when' block is triggered."
    
    def parse(self, line, **kwargs):
        if line.startswith("#when"):
            goal = len(kwargs["goals"]) + 1
            kwargs["goals"].append(goal)
            kwargs["data_stack"].append(goal)
            kwargs["action_stack"].append("set-goal {} 1".format(goal))
            actions = ["set-goal {} 0".format(goal)]
            if self.get_data(line)[0] is not None:
                actions.append("disable-self")
            return Defrule(["true"], actions, ignore_stacks=True)
        elif line.startswith("#then"):
            goal = kwargs["data_stack"][-1]
            kwargs["action_stack"].pop()
            kwargs["condition_stack"].append("goal {} 1".format(goal))
        elif line.startswith("#end"):
            kwargs["data_stack"].pop()
            kwargs["condition_stack"].pop()

@rule
class DoOnce(Rule):
    def __init__(self):
        super().__init__()
        self.name = "do once"
        self.regex = re.compile("^(?:#do once|#end do(?: once)?)$")
        self.usage = """#do once
    RULES
#end do once"""
        self.help = "Adds 'disable-self' to the action stack. Makes sure each rule in the block individually runs only once."
    
    def parse(self, line, **kwargs):
        if "end" in line:
            kwargs["action_stack"].pop(-1)
        else:
            kwargs["action_stack"].append("disable-self")

@rule
class Delay(Rule):
    def __init__(self):
        super().__init__()
        self.name = "delay"
        self.regex = re.compile("^(?:#delay by ([^ ]+) (seconds?|minutes?|hours?)|#end delay)$")
        self.usage = """#delay by AMOUNT TIME_UNIT
    RULES
#end delay"""
        self.help = "Block body is only allowed to trigger after the time is up."
    
    def parse(self, line, **kwargs):
        if "end" in line:
            kwargs["condition_stack"].pop(-1)
        else:
            data = self.get_data(line)
            unit = data[1]
            amount = int(data[0])
            if "minute" in unit:
                amount *= 60
            elif "hour" in unit:
                amount *= 3600
            timer_number = len(kwargs["timers"]) + 1
            kwargs["timers"].append(timer_number)
            kwargs["condition_stack"].append("timer-triggered {}".format(timer_number))
            return Defrule(["true"], ["enable-timer {} {}".format(timer_number, amount), "disable-self"])

@rule
class Repeat(Rule):
    def __init__(self):
        super().__init__()
        self.name = "repeat"
        self.regex = re.compile("^(?:#repeat every ([^ ]+) (seconds?|minutes?|hours?)|#end repeat)$")
        self.usage = """#repeat every AMOUNT TIME_UNIT
    RULES
#end repeat"""
        self.help = "Each rule is allowed to be triggered once after the time has elapsed, the process repeats."
    
    def parse(self, line, **kwargs):
        if "end" in line:
            timer_number = kwargs["data_stack"].pop()
            amount = kwargs["data_stack"].pop()
            kwargs["condition_stack"].pop()
            return Defrule(["timer-triggered {}".format(timer_number)], ["disable-timer {}".format(timer_number), "enable-timer {} {}".format(timer_number, amount)])
        else:
            data = self.get_data(line)
            unit = data[1]
            amount = int(data[0])
            if "minute" in unit:
                amount *= 60
            elif "hour" in unit:
                amount *= 3600
            timer_number = len(kwargs["timers"]) + 1
            kwargs["data_stack"].append(amount)
            kwargs["data_stack"].append(timer_number)
            kwargs["timers"].append(timer_number)
            kwargs["condition_stack"].append("timer-triggered {}".format(timer_number))
            return Defrule(["true"], ["enable-timer {} {}".format(timer_number, amount), "disable-self"])

@rule
class Train(Rule):
    def __init__(self):
        super().__init__()
        self.name = "train"
        self.regex = re.compile("^train (?:([^ ]+) )?([^ ]+)(?: with ((?:[^ ]+(?: and )?)*) escrow)?$")
        self.usage = """train UNIT_NAME
train UNIT_NAME with RESOURCE_NAME escrow
train AMOUNT UNIT_NAME
train AMOUNT UNIT_NAME with RESOURCE_NAME escrow"""
        self.example = "train 10 militiaman-line with food and gold escrow"
        self.help = "Trains a unit using the specified parameters."

    def parse(self, line, **kwargs):
        amount, unit, escrow = self.get_data(line)

        actions = []
        conditions = []
        
        if escrow is None:
            conditions.append("can-train " + unit)
        else:
            conditions.append("can-train-with-escrow " + unit)
            for resource in escrow.split(" and "):
                actions.append("release-escrow " + resource)
        if amount is not None:
            conditions.append("unit-type-count-total {} < {}".format(unit, amount))
        actions.append("train " + unit)
        
        return Defrule(conditions, actions)

@rule
class Respond(Rule):
    def __init__(self):
        super().__init__()
        self.name = "respond"
        self.regex = re.compile("^respond to (?:([^ ]{1,3}) )?([^ ]+)(?: (building|unit))? with (?:([^ ]+) )?([^ ]+)(?: (building|unit))?$")
        self.usage = "respond to ?AMOUNT NAME ?BUILDING/UNIT with NAME ?BUILDING/UNIT"
        self.help = "When the AI sees the specified amount, it reacts with the specified parameters. If building/unit is unspecified, unit is assumed. If amount is unspecified, 1 is assumed."

    def parse(self, line, **kwargs):
        detect_amount, detect_name, detect_type, create_amount, create_name, create_type = self.get_data(line)

        if detect_amount is None:
            detect_amount = 1
        if detect_type is None:
            detect_type = "unit"
        if create_type is None:
            create_type = "unit"
        action = "train" if create_type == "unit" else "build"

        conditions = []

        conditions.append("players-{}-type-count any-enemy {} >= {}".format(detect_type, detect_name, detect_amount))
        if create_amount is not None:
            conditions.append("{}-type-count-total {} < {}".format(create_type, create_name, create_amount))
        conditions.append("can-{} {}".format(action, create_name))
        
        return Defrule(conditions, ["{} {}".format(action, create_name)])

@rule
class BlockRespond(Rule):
    def __init__(self):
        super().__init__()
        self.name = "block respond"
        self.regex = re.compile("^(?:#respond to (?:([^ ]{1,3}) )?([^ ]+)(?: (building|unit))?|#end respon(?:d|se))$")
        self.usage = """#respond to ?AMOUNT NAME ?BUILDING/UNIT
   RULES
#end respond"""
        self.help = "When the AI sees the specified amount, the body is allowed to trigger. If building/unit is unspecified, unit is assumed. If amount is unspecified, 1 is assumed."
        self.example = """#respond to 3 scout-cavalry-line
    train 4 spearman-line
#end respond"""
    
    def parse(self, line, **kwargs):
        if line.startswith("#end"):
            kwargs["condition_stack"].pop()
        else:
            amount, name, detect_type = self.get_data(line)

            if amount is None:
                amount = 1
            if detect_type is None:
                detect_type = "unit"
            
            kwargs["condition_stack"].append("players-{}-type-count any-enemy {} >= {}".format(detect_type, name, amount))

@rule
class Reply(Rule):
    def __init__(self):
        super().__init__()
        self.name = "block respond taunt"
        self.regex = re.compile("^(?:#reply to (enemy|ally) taunt ([^ ]+)|#end reply)$")
        self.usage = """#reply to ?ENEMY/ALLY taunt TAUNT_NUMBER
   RULES
#end reply"""
        self.help = "Body is allowed to trigger when the taunt is detected from the specified player. Taunt is acknowledged regardless of whether the body successfully triggers."
        self.example = """#reply to ally taunt 5
    tribute 100 gold to this-any-ally
#end reply"""
    
    def parse(self, line, **kwargs):
        if line.startswith("#end"):
            kwargs["condition_stack"].pop()
            number = kwargs["data_stack"].pop()
            player_type = kwargs["data_stack"].pop()
            return Defrule(["true"], ["acknowledge-taunt this-any-{} {}".format(player_type, number)])
        else:
            player_type, number = self.get_data(line)
            kwargs["condition_stack"].append("taunt-detected any-{} {}".format(player_type, number))
            kwargs["data_stack"].append(player_type)
            kwargs["data_stack"].append(number)

@rule
class Tribute(Rule):
    def __init__(self):
        super().__init__()
        self.name = "tribute"
        self.regex = re.compile("^tribute ([^ ]+) ([^ ]+) to (?:player )?([^ ]+)$")
        self.usage = "tribute AMOUNT RESOURCE_NAME to PLAYER_NUMBER"
        self.help = "Gives the specified player resources."
        self.example = "tribute 100 wood to any-ally"
    
    def parse(self, line, **kwargs):
        amount, resource, player = self.get_data(line)
        return Defrule(["true"], ["tribute-to-player {} {} {}".format(player, resource, amount)])

@rule
class BuildDropoffs(Rule):
    def __init__(self):
        super().__init__()
        self.name = "build dropoffs"
        self.regex = re.compile("^build (lumber camps|(?:gold|stone) mining camps|mills)(?: maintaining ([^ ]+) tiles?)?$")
        self.usage = "build BUILDING_TYPE"
        self.help = "Sets up a rule for automatically refreshing dropoff points."
        self.example = """
build lumber camps
build gold mining camps
build stone mining camps
build lumber camps maintaining 4 tiles"""
        
    def parse(self, line, **kwargs):
        data = self.get_data(line)
        dropoff_type = data[0]
        tiles = data[1]

        if "mining camps" in dropoff_type:
            if "gold" in dropoff_type:
                resource = "gold"
            else:
                resource = "stone"
            building = "mining-camp"
            if tiles is None:
                tiles = 3
        
        elif dropoff_type == "lumber camps":
            resource = "wood"
            building = "lumber-camp"
            if tiles is None:
                tiles = 2
        elif dropoff_type == "mills":
            resource = "food"
            building = "mill"
            if tiles is None:
                tiles = 3

        return Defrule(["dropsite-min-distance {} > {}".format(resource, tiles),
                        "resource-found {}".format(resource),
                        "up-pending-objects c: {} == 0".format(building),
                        "can-build {}".format(building)],
                       ["build {}".format(building)])

@rule
class BuildHouses(Rule):
    def __init__(self):
        super().__init__()

        self.default_headroom = 5
        
        self.name = "build houses"
        self.regex = re.compile("^build houses(?: with ([^ ]+) headroom)?$")
        self.usage = "build houses with AMOUNT headroom"
        self.example = """build houses
build houses with 10 headroom"""
        self.help = "Sets up rule to build houses, default headroom is {}.".format(self.default_headroom)

    def parse(self, line, **kwargs):
        headroom = self.get_data(line)[0]
        
        conditions = ["population-headroom != 0", "up-pending-objects c: house == 0", "can-build house"]
        actions = ["build house"]
        
        if headroom is None:
            headroom = self.default_headroom

        conditions.append("housing-headroom < {}".format(headroom))
        return Defrule(conditions, actions)

@rule
class EnableWalls(Rule):
    def __init__(self):
        super().__init__()
        self.name = "enable walls"
        self.regex = re.compile("^enable walls (?:with|on) perimeter (1|2)$")
        self.usage = "enable walls on perimeter PERIMETER_NUMBER"
        self.example = "enable walls on perimeter 2"
        self.help = "Sets up rule that allows the AI to build walls on the specified perimeter."

    def parse(self, line, **kwargs):
        return Defrule(["true"], ["enable-wall-placement " + self.get_data(line)[0], "disable-self"])

@rule
class BuildWalls(Rule):
    def __init__(self):
        super().__init__()
        self.name = "build walls"
        self.regex = re.compile("^build (stone|palisade) (walls|gates) (?:with|on) perimeter (1|2)$")
        self.usage = "build MATERIAL WALLS/GATES on perimeter PERIMETER_NUMBER"
        self.example = """build stone walls on perimeter 2
build stone gates on perimeter 2"""
        self.help = "Wall placement must be enabled on the same perimeter to function."
        
        self.materials = {"stone":"stone-wall-line", "palisade":"palisade-wall"}
    
    def parse(self, line, **kwargs):
        material, type, perimeter = self.get_data(line)
        wall = self.materials[material]
        if type == "walls":
            return Defrule(["can-build-wall {} {}".format(perimeter, wall)],
                           ["build-wall {} {}".format(perimeter, wall)])
        else:
            return Defrule(["building-type-count-total {} > 0".format(wall),
                            "can-build-gate {}".format(perimeter),
                            "building-type-count-total gate < 5"],
                           ["build-gate {}".format(perimeter)])

@rule
class Build(Rule):
    def __init__(self):
        super().__init__()
        self.name = "build"
        self.regex = re.compile("^build (forward )?(?:([^ ]+) )?([^ ]+)(?: ([^ ]+) tiles? away from ([^ ]+))?(?: with ((?:[^ ]+(?: and )?)*) escrow)?$")
        self.usage = "build ?forward AMOUNT BUILDING_NAME with RESOURCE_NAME escrow"
        self.examples = """build 1 barracks
build forward castle
build archery-range with wood escrow"""
        self.help = "Sets up the rule to build the building. If amount is unspecified, the building will be built infinitely."

    def parse(self, line, **kwargs):
        forward, amount, building, near_distance, near, escrow = self.get_data(line)
        
        actions = []
        conditions = []
        
        if escrow is None:
            conditions.append("can-build " + building)
        else:
            conditions.append("can-build-with-escrow " + building)
            for resource in escrow.split(" and "):
                actions.append("release-escrow " + resource)
        
        if amount is not None:
            conditions.append("building-type-count-total {} < {}".format(building, amount))

        if near is None:
            actions.append("build{} {}".format("" if forward is None else "-forward", building))
            compressable = True
        else:
            actions.append("up-set-placement-data {} {} c: {}".format("any-enemy" if forward else "my-player-number", near, near_distance))
            actions.append("up-build place-{} 0 c: {}".format("forward" if forward else "control", building))
            compressable = False

        if building == "castle":
            conditions.append("stone-amount >= 650")
        
        return Defrule(conditions, actions, compressable=compressable)

@rule
class Research(Rule):
    def __init__(self):
        super().__init__()
        self.name = "research"
        self.regex = re.compile("^research (?:([^ ]+)|({}) upgrades)(?: with ((?:[^ ]+(?: and )?)*) escrow)?$".format("|".join(research.keys())))
        self.usage = "research TECH_NAME with RESOURCE_NAME escrow"
        self.example = """research ri-loom
research feudal-age with food and gold escrow
research blacksmith infantry upgrades"""
        self.help = "Sets up the rule to research the specified research."
    
    def parse(self, line, **kwargs):
        tech, tech_group, escrow = self.get_data(line)
        
        actions = []
        conditions = []

        if escrow is None:
            conditions.append("can-research {}")
        else:
            conditions.append("can-research-with-escrow {}")
            for resource in escrow.split(" and "):
                actions.append("release-escrow {}".format(resource))
        actions.append("research {}")

        out = []
        
        for name in research[tech_group] if tech_group in research else [tech]:
            out.append(Defrule([x.format(name) for x in conditions], [x.format(name) for x in actions]))
        
        return out

@rule
class Attack(Rule):
    def __init__(self):
        super().__init__()
        self.name = "attack"
        self.regex = re.compile("^attack(?: with ([^ ]+) units)?$")
        self.usage = "attack with AMOUNT units"
        self.example = """attack
attack with 10 units"""
        self.help = "Makes use of the attack-now action."
    
    def parse(self, line, **kwargs):
        number = self.get_data(line)[0]
        if number is None:
            return Defrule(["true"], ["attack-now"])
        else:
            return Defrule(["military-population >= {}".format(number)], ["attack-now"])

@rule
class Market(Rule):
    def __init__(self):
        super().__init__()
        self.name = "market"
        self.regex = re.compile("^(buy|sell) ([^ ]+) when ([^ +]+) ([<>!=]+) ([^ ]+)$")
        self.usage = "buy/sell RESOURCE_NAME when RESOURCE_NAME COMPARISON AMOUNT"
        self.example = """buy food when gold > 100
sell wood when wood > 1500"""
        self.help = "Buys/sells based on a condition."

    def parse(self, line, **kwargs):
        action, commodity, conditional, comparison, amount = self.get_data(line)
        return Defrule(["{}-amount {} {}".format(conditional, comparison, amount),
                        "can-{}-commodity {}".format(action, commodity)],
                       ["{}-commodity {}".format(action, commodity)])

@rule
class DistributeVillagers(Rule):
    def __init__(self):
        super().__init__()
        self.name = "distribute villagers"
        self.regex = re.compile("^distribute villagers ([0-9]+) ([0-9]+) ([0-9]+) ([0-9]+)$")
        self.usage = "distribute villagers WOOD_PERCENT FOOD_PERCENT GOLD_PERCENT STONE_PERCENT"
        self.example = "distribute villagers 40 45 10 5"
        self.help = "Percentages must add up to 100. Makes use of the sn-TYPE-gatherer-percentage strategic number."

    def parse(self, line, **kwargs):
        data = self.get_data(line)
        
        return Defrule(["true"], ["set-strategic-number sn-wood-gatherer-percentage {}".format(data[0]),
                                  "set-strategic-number sn-food-gatherer-percentage {}".format(data[1]),
                                  "set-strategic-number sn-gold-gatherer-percentage {}".format(data[2]),
                                  "set-strategic-number sn-stone-gatherer-percentage {}".format(data[3])])

@rule
class DistributeVillagersTo(Rule):
    def __init__(self):
        super().__init__()
        self.name = "distribute villagers to"
        self.regex = re.compile("^distribute ([0-9]+) villagers? from ((?:[^ ]+(?: and )?)*) to (.+)$")
        self.usage = "distribute PERCENTAGE villagers from RESOURCE_NAME to RESOURCE_NAME"
        self.example = """distibute 5 villagers from wood to stone
distribute 10 villagers from wood and food to gold
distribute 8 villagers from food to gold and stone"""
        self.help = "Modifies the gatherer percentages. Precomputes, so no constants or facts can be used, just numbers."

    def parse(self, line, **kwargs):
        amount, sources, destinations = self.get_data(line)
        amount = int(amount)
        sources = sources.split(" and ")
        destinations = destinations.split(" and ")
        
        conditions = []
        actions = []
        for resource in sources:
            conditions.append("strategic-number sn-{}-gatherer-percentage >= {}".format(resource, amount // len(sources)))
            actions.append("up-modify-sn sn-{}-gatherer-percentage c:- {}".format(resource, amount // len(sources)))
        for resource in destinations:
            conditions.append("strategic-number sn-{}-gatherer-percentage <= {}".format(resource, 100 - amount // len(destinations)))
            actions.append("up-modify-sn sn-{}-gatherer-percentage c:+ {}".format(resource, amount // len(destinations)))

        return Defrule(conditions, actions)

@rule
class AssignBuilders(Rule):
    def __init__(self):
        super().__init__()
        self.name = "assign builders"
        self.regex = re.compile("^assign ([^ ]+) builders? to ([^ ]+)$")
        self.usage = "assign AMOUNT builders to BUILDING_NAME"
        self.example = "assign 8 builders to castle"
        self.help = "Sets the amount of builders that should build a building."

    def parse(self, line, **kwargs):
        amount, building = self.get_data(line)
        return Defrule(["true"], ["up-assign-builders c: {} c: {}".format(building, amount)])

@rule
class SetGoal(Rule):
    def __init__(self):
        super().__init__()
        self.name = "set goal"
        self.regex = re.compile("^goal ([^ ]+) ?(\+|\-|\*|\/)?= ?(.+)$")
        self.usage = "goal GOAL_NAME = VALUE"
        self.example = """goal advance = 1
goal count += 1"""
        self.help = "Sets a goal, and sets up the constant if it does not already exist."

    def parse(self, line, **kwargs):
        name, operator, value = self.get_data(line)
        if operator is None:
            if name not in kwargs["constants"]:
                goal = len(kwargs["goals"]) + 1
                kwargs["goals"].append(goal)
                kwargs["definitions"].insert(0, Defconst(name, goal))
                kwargs["constants"].append(name)
            return Defrule(["true"], ["set-goal {} {}".format(name, value)])
        return Defrule(["true"], ["up-modify-goal {} c:{} {}".format(name, operator, value)])

@rule
class SetConst(Rule):
    def __init__(self):
        super().__init__()
        self.name = "set const"
        self.regex = re.compile("^const ([^ ]+) ?= ?(.+)$")
        self.usage = "const CONST_NAME = VALUE"
        self.example = "const max-villagers = 120"
        self.help = "Sets a constant. Can only be done once for every name."

    def parse(self, line, **kwargs):
        name, value = self.get_data(line)
        if name in kwargs["constants"]:
            print("WARNING: Do not declare a constant with the same name more than once. Ignored.")
            return
        kwargs["constants"].append(name)
        kwargs["definitions"].insert(0, Defconst(name, value))
        if kwargs["condition_stack"] or kwargs["action_stack"]:
            print("WARNING: Consts should only be set at the top of the script. This is where they will appear.")

@rule
class SetStrategicNumber(Rule):
    def __init__(self):
        super().__init__()
        self.name = "set strategic number"
        self.regex = re.compile("^(sn-[^ ]+) ?(\+|\-|\*|\/)?= ?(.+)$")
        self.usage = "STRATEGIC_NUMBER_NAME = VALUE"
        self.example = """sn-maximum-gold-drop-distance = 8
sn-maximum-town-size += 5"""
        self.help = "Sets a strategic number. Uses the 'sn-' prefix for recognition of the rule."

    def parse(self, line, **kwargs):
        name, operator, value = self.get_data(line)
        if operator is None:
            return Defrule(["true"], ["set-strategic-number {} {}".format(name, value)])
        return Defrule(["true"], ["up-modify-sn {} c:{} {}".format(name, operator, value)])

@rule
class SendScout(Rule):
    def __init__(self):
        super().__init__()
        self.name = "scout"
        self.regex = re.compile("^scout (.+)$")
        self.usage = "scout AREA_NAME"
        self.example = """scout opposite
scout enemy"""
        self.help = "Sets up the userpatch rule to send the scout somewhere else."

    def parse(self, line, **kwargs):
        direction = self.get_data(line)[0]
        return Defrule(["true"], ["up-send-scout group-type-land-explore scout-{}".format(direction)])

@rule
class ModifyTownSize(Rule):
    def __init__(self):
        super().__init__()
        self.name = "modify town size"
        self.regex = re.compile("^(set|increase|decrease) town size (?:to|by) (.+)$")
        self.usage = "set/increase/decrease town size by/to AMOUNT"
        self.example = """set town size to 32
increase town size by 5
decrease town size by 1"""
        self.help = "Modifies the sn-maximum-town-size strategic number."

    def parse(self, line, **kwargs):
        operation, amount = self.get_data(line)
        if operation == "set":
            return Defrule(["true"], ["set-strategic-number sn-maximum-town-size {}".format(amount)])
        return Defrule(["true"], ["up-modify-sn sn-maximum-town-size c:{} {}".format("+" if operation == "increase" else "-", amount)])

@rule
class SetEscrow(Rule):
    def __init__(self):
        super().__init__()
        self.name = "set escrow"
        self.regex = re.compile("^escrow ([^ ]+) (.+)$")
        self.usage = "escrow PERCENTAGE RESOURCE_NAME"
        self.example = "escrow 50 gold"
        self.help = "Creates rule that sets the escrow percentage."

    def parse(self, line, **kwargs):
        amount, resource = self.get_data(line)
        return Defrule(["true"], ["set-escrow-percentage {} {}".format(resource, amount)])

@rule
class Delete(Rule):
    def __init__(self):
        super().__init__()
        self.name = "delete"
        self.regex = re.compile("^delete (unit|building) (.+)$")
        self.usage = "delete unit/building NAME"
        self.example = "delete unit villager"
        self.help = "Creates a rule that deletes the specified object."

    def parse(self, line, **kwargs):
        form, name = self.get_data(line)
        return Defrule(["true"], ["delete-{} {}".format(form, name)])

@rule
class SelectRandom(Rule):
    def __init__(self):
        super().__init__()
        self.name = "select random"
        self.regex = re.compile("^#select random( persistant)?|#randor|#end select(?: random)?$")
        self.usage = """#select random ?persistant
   chat to all \"option 1!\"
#randor
   chat to all \"option 2!\"
#end select random"""
        self.help = "A random block separated by randors will be allowed to execute. Using persistant mode means the randomly chosen one is picked every time, otherwise it will change."
        self.example = """#select random
   chat to all \"option 1!\"
#randor
   chat to all \"option 2!\"
#end select random"""
    
    def parse(self, line, **kwargs):
        persistant = self.get_data(line)[0]
        
        if line.startswith("#select"):
            identifier = uuid.uuid4()
            const_name = f"select-random-{identifier}"
            goal_number = len(kwargs["goals"]) + 1

            kwargs["goals"].append(goal_number)

            kwargs["data_stack"].append(persistant)
            kwargs["data_stack"].append(2)
            kwargs["data_stack"].append(identifier)
            kwargs["data_stack"].append(goal_number)
            
            kwargs["definitions"].insert(0, Defconst(const_name, -1))
            kwargs["constants"].append(const_name)

            kwargs["condition_stack"].append(f"goal {goal_number} 1")
            
            generate_rule = Defrule(["true"], [f"generate-random-number {const_name}", f"set-goal {goal_number} 0"])
            
            if persistant:
                generate_rule.actions.append("disable-self")
                
            return [generate_rule, Defconst(identifier, -1)]
        
        elif line.startswith("#end"):
            kwargs["condition_stack"].pop()

            goal_number = kwargs["data_stack"].pop()
            identifier = kwargs["data_stack"].pop()
            number_of_blocks = kwargs["data_stack"].pop() - 1
            persistant = kwargs["data_stack"].pop()
            
            const_name = f"select-random-{identifier}"
            
            for const in kwargs["definitions"]:
                if isinstance(const, Defconst) and const.name == const_name:
                    const.value = number_of_blocks
                    break
            else:
                raise Exception(f"select random failed to find const name '{const_name}'")

            for i, const in enumerate(kwargs["definitions"]):
                if isinstance(const, Defconst) and const.name == identifier:
                    kwargs["definitions"].pop(i)
                    for goal_value in range(1, number_of_blocks + 1):
                        actions = [f"set-goal {goal_number} {goal_value}"]
                        if persistant:
                            actions.append("disable-self")
                        goal_set_rule = Defrule([f"random-number == {goal_value}", f"goal {goal_number} 0"], actions)
                        goal_set_rule.conditions.extend(kwargs["condition_stack"])
                        goal_set_rule.actions.extend(kwargs["action_stack"])
                        kwargs["definitions"].insert(i + goal_value - 1, goal_set_rule)
                    break
            else:
                raise Exception(f"failed to find marker '{identifier}'")
        
        else:
            goal_number = kwargs["data_stack"].pop()
            identifier = kwargs["data_stack"].pop()
            number = kwargs["data_stack"].pop()
            kwargs["condition_stack"].pop()
            kwargs["condition_stack"].append(f"goal {goal_number} {number}")
            kwargs["data_stack"].append(number + 1)
            kwargs["data_stack"].append(identifier)
            kwargs["data_stack"].append(goal_number)
