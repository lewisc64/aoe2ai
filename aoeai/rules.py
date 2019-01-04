import re

from .defrule import *
from . import interpreter

rules = []

def rule(rule):
    global rules
    rules.append(rule())

class Rule:
    def __init__(self):
        self.name = ""
        self.regex = re.compile("")

    def is_match(self, line):
        return self.regex.match(line)

    def get_data(self, line):
        return self.regex.search(line).groups()

    def parse(self, line, **kwargs):
        pass

class Snippet(Rule):
    def __init__(self, trigger, conditions, actions):
        self.name = trigger
        self.regex = re.compile("^{}$".format(trigger))
        self.conditions = conditions
        self.actions = actions
    
    def parse(self, line, **kwargs):
        return Defrule(self.conditions[:], self.actions[:])

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
                      "set-strategic-number sn-number-explore-groups 1"]))
rules.append(Snippet("set up micro",
                     ["true"],
                     ["set-difficulty-parameter ability-to-maintain-distance 0",
                      "set-difficulty-parameter ability-to-dodge-missiles 0",
                      "set-strategic-number sn-attack-intelligence 1",
                      "set-strategic-number sn-livestock-to-town-center 1",
                      "set-strategic-number sn-enable-patrol-attack 1",
                      "set-strategic-number sn-intelligent-gathering 1",
                      "set-strategic-number sn-local-targeting-mode 1",
                      "set-strategic-number sn-percent-enemy-sighted-response 100"]))
rules.append(Snippet("build lumber camps",
                     ["dropsite-min-distance wood > 2", "resource-found wood", "up-pending-objects c: lumber-camp == 0", "can-build lumber-camp"],
                     ["build lumber-camp"]))
rules.append(Snippet("build mills",
                     ["dropsite-min-distance food > 3", "resource-found food", "up-pending-objects c: mill == 0", "can-build mill"],
                     ["build mill"]))
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

@rule
class Comment(Rule):
    def __init__(self):
        self.name = "comment"
        self.regex = re.compile("^//(.+)$")
    
    def parse(self, line, **kwargs):
        return None

@rule
class Load(Rule):
    def __init__(self):
        self.name = "load"
        self.regex = re.compile("^load \"([^\"]+)\"$")
    
    def parse(self, line, **kwargs):
        path = self.get_data(line)[0]
        file = open(path, "r")
        content = file.read()
        file.close()
              
        output = interpreter.interpret(content, timers=kwargs["timers"], goals=kwargs["goals"], constants=kwargs["constants"])

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
        self.name = "cheat"
        self.regex = re.compile("^cheat ([^ ]+) ([^ ]+)$")

    def parse(self, line, **kwargs):
        amount, resource = self.get_data(line)
        return Defrule(["true"],
                       ["cc-add-resource {} {}".format(resource, amount)])

@rule
class ChatTo(Rule):
    def __init__(self):
        self.name = "chat to"
        self.regex = re.compile("^chat to (all|self|allies) (.+)$")
    
    def parse(self, line, **kwargs):
        recipient, content = self.get_data(line)
        return Defrule(["true"],
                       ["chat-to-{} {}".format(recipient, content).replace("to-self", "local-to-self")])

@rule
class AddCondition(Rule):
    def __init__(self):
        self.name = "add condition"
        self.regex = re.compile("^(?:#add condition (.+)|#remove condition)$")
    
    def parse(self, line, **kwargs):
        condition = self.get_data(line)[0]
        if condition is None:
            kwargs["condition_stack"].pop(-1)
        else:
            kwargs["condition_stack"].append(parse_condition(condition))

@rule
class AddAction(Rule):
    def __init__(self):
        self.name = "add action"
        self.regex = re.compile("^(?:#add action (.+)|#remove action)$")
    
    def parse(self, line, **kwargs):
        action = self.get_data(line)[0]
        if action is None:
            kwargs["action_stack"].pop(-1)
        else:
            kwargs["action_stack"].append(action)

@rule
class If(Rule):
    def __init__(self):
        self.name = "if"
        self.regex = re.compile("^(?:#if (.+)|#else if (.+)|#else|#end if)$")
    
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
        self.name = "when"
        self.regex = re.compile("^(?:#when( once)?|#then|#end when)$")
    
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
        self.name = "do once"
        self.regex = re.compile("^(?:#do once|#end do(?: once)?)$")
    
    def parse(self, line, **kwargs):
        if "end" in line:
            kwargs["action_stack"].pop(-1)
        else:
            kwargs["action_stack"].append("disable-self")

@rule
class Delay(Rule):
    def __init__(self):
        self.name = "delay"
        self.regex = re.compile("^(?:#delay by ([^ ]+) seconds|#end delay)$")
    
    def parse(self, line, **kwargs):
        if "end" in line:
            kwargs["condition_stack"].pop(-1)
        else:
            amount = self.get_data(line)[0]
            timer_number = len(kwargs["timers"]) + 1
            kwargs["timers"].append(timer_number)
            kwargs["condition_stack"].append("timer-triggered {}".format(timer_number))
            return Defrule(["true"], ["enable-timer {} {}".format(timer_number, amount), "disable-self"])

@rule
class Repeat(Rule):
    def __init__(self):
        self.name = "repeat"
        self.regex = re.compile("^(?:#repeat every ([^ ]+) seconds?|#end repeat)$")
    
    def parse(self, line, **kwargs):
        if "end" in line:
            timer_number = kwargs["data_stack"].pop()
            amount = kwargs["data_stack"].pop()
            kwargs["condition_stack"].pop()
            return Defrule(["timer-triggered {}".format(timer_number)], ["disable-timer {}".format(timer_number), "enable-timer {} {}".format(timer_number, amount)])
        else:
            amount = self.get_data(line)[0]
            timer_number = len(kwargs["timers"]) + 1
            kwargs["data_stack"].append(amount)
            kwargs["data_stack"].append(timer_number)
            kwargs["timers"].append(timer_number)
            kwargs["condition_stack"].append("timer-triggered {}".format(timer_number))
            return Defrule(["true"], ["enable-timer {} {}".format(timer_number, amount), "disable-self"])

@rule
class Train(Rule):
    def __init__(self):
        self.name = "train"
        self.regex = re.compile("^train (?:([^ ]+) )?([^ ]+)(?: with ((?:[^ ]+(?: and )?)*) escrow)?$")

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
        self.name = "respond"
        self.regex = re.compile("^respond to (?:([^ ]{1,3}) )?([^ ]+)(?: (building|unit))? with (?:([^ ]+) )?([^ ]+)(?: (building|unit))?$")

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
        self.name = "block respond"
        self.regex = re.compile("^(?:#respond to (?:([^ ]{1,3}) )?([^ ]+)(?: (building|unit))?|#end respon(?:d|se))$")
    
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
        self.name = "block respond taunt"
        self.regex = re.compile("^(?:#reply to (enemy|ally) taunt ([^ ]+)|#end reply)$")
    
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
        self.name = "tribute"
        self.regex = re.compile("^tribute ([^ ]+) ([^ ]+) to (?:player )?([^ ]+)$")
    
    def parse(self, line, **kwargs):
        amount, resource, player = self.get_data(line)
        return Defrule(["true"], ["tribute-to-player {} {} {}".format(player, resource, amount)])

@rule
class BuildMiningCamps(Rule):
    def __init__(self):
        self.name = "build mining camps"
        self.regex = re.compile("^build (gold|stone) mining camps$")
        
    def parse(self, line, **kwargs):
        resource = self.get_data(line)[0]
        return Defrule(["dropsite-min-distance {} > 3".format(resource),
                        "resource-found {}".format(resource),
                        "up-pending-objects c: mining-camp == 0",
                        "can-build mining-camp"],
                       ["build mining-camp"])


@rule
class BuildHouses(Rule):
    def __init__(self):
        self.name = "build houses"
        self.regex = re.compile("^build houses(?: with ([^ ]+) headroom)?$")

    def parse(self, line, **kwargs):
        headroom = self.get_data(line)[0]
        
        conditions = ["population-headroom != 0", "up-pending-objects c: house == 0", "can-build house"]
        actions = ["build house"]
        
        if headroom is None:
            headroom = 5

        conditions.append("housing-headroom < {}".format(headroom))
        return Defrule(conditions, actions)

@rule
class EnableWalls(Rule):
    def __init__(self):
        self.name = "enable walls"
        self.regex = re.compile("^enable walls (?:with|on) perimeter (1|2)$")

    def parse(self, line, **kwargs):
        return Defrule(["true"], ["enable-wall-placement " + self.get_data(line)[0], "disable-self"])

@rule
class BuildWalls(Rule):
    def __init__(self):
        self.name = "build walls"
        self.regex = re.compile("^build (stone|palisade) (walls|gates) (?:with|on) perimeter (1|2)$")
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
        self.name = "build"
        self.regex = re.compile("^build (forward )?(?:([^ ]+) )?([^ ]+)(?: with ((?:[^ ]+(?: and )?)*) escrow)?$")

    def parse(self, line, **kwargs):
        forward, amount, building, escrow = self.get_data(line)
        
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

        actions.append("build{} {}".format("" if forward is None else "-forward", building))
        
        return Defrule(conditions, actions)

@rule
class Research(Rule):
    def __init__(self):
        self.name = "research"
        self.regex = re.compile("^research ([^ ]+)(?: with ((?:[^ ]+(?: and )?)*) escrow)?$")
    
    def parse(self, line, **kwargs):
        tech, escrow = self.get_data(line)

        actions = []
        conditions = []

        if escrow is None:
            conditions.append("can-research " + tech)
        else:
            conditions.append("can-research-with-escrow " + tech)
            for resource in escrow.split(" and "):
                actions.append("release-escrow " + resource)
        actions.append("research " + tech)
        
        return Defrule(conditions, actions)

@rule
class Attack(Rule):
    def __init__(self):
        self.name = "attack"
        self.regex = re.compile("^attack(?: with ([^ ]+) units)?$")
    
    def parse(self, line, **kwargs):
        number = self.get_data(line)[0]
        if number is None:
            return Defrule(["true"], ["attack-now"])
        else:
            return Defrule(["military-population >= {}".format(number)], ["attack-now"])

@rule
class Market(Rule):
    def __init__(self):
        self.name = "market"
        self.regex = re.compile("^(buy|sell) ([^ ]+) when ([^ +]+) ([<>!=]+) ([^ ]+)$")

    def parse(self, line, **kwargs):
        action, commodity, conditional, comparison, amount = self.get_data(line)
        return Defrule(["{}-amount {} {}".format(conditional, comparison, amount),
                        "can-{}-commodity {}".format(action, commodity)],
                       ["{}-commodity {}".format(action, commodity)])

@rule
class DistributeVillagers(Rule):
    def __init__(self):
        self.name = "distribute villagers"
        self.regex = re.compile("^distribute villagers ([0-9]+) ([0-9]+) ([0-9]+) ([0-9]+)$")

    def parse(self, line, **kwargs):
        data = self.get_data(line)
        
        return Defrule(["true"], ["set-strategic-number sn-wood-gatherer-percentage {}".format(data[0]),
                                  "set-strategic-number sn-food-gatherer-percentage {}".format(data[1]),
                                  "set-strategic-number sn-gold-gatherer-percentage {}".format(data[2]),
                                  "set-strategic-number sn-stone-gatherer-percentage {}".format(data[3])])

@rule
class DistributeVillagersTo(Rule):
    def __init__(self):
        self.name = "distribute villagers to"
        self.regex = re.compile("^distribute ([0-9]+) villagers? from ((?:[^ ]+(?: and )?)*) to (.+)$")

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
        self.name = "assign builders"
        self.regex = re.compile("^assign ([^ ]+) builders? to ([^ ]+)$")

    def parse(self, line, **kwargs):
        amount, building = self.get_data(line)
        return Defrule(["true"], ["up-assign-builders c: {} c: {}".format(building, amount)])

@rule
class SetGoal(Rule):
    def __init__(self):
        self.name = "set goal"
        self.regex = re.compile("^goal ([^ ]+) ?(\+|\-|\*|\/)?= ?(.+)$")

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
        self.name = "set const"
        self.regex = re.compile("^const ([^ ]+) ?= ?(.+)$")

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
        self.name = "set strategic number"
        self.regex = re.compile("^(sn-[^ ]+) ?(\+|\-|\*|\/)?= ?(.+)$")

    def parse(self, line, **kwargs):
        name, operator, value = self.get_data(line)
        if operator is None:
            return Defrule(["true"], ["set-strategic-number {} {}".format(name, value)])
        return Defrule(["true"], ["up-modify-sn {} c:{} {}".format(name, operator, value)])

@rule
class SendScout(Rule):
    def __init__(self):
        self.name = "scout"
        self.regex = re.compile("^scout (.+)$")

    def parse(self, line, **kwargs):
        direction = self.get_data(line)[0]
        return Defrule(["true"], ["up-send-scout group-type-land-explore scout-{}".format(direction)])

@rule
class ModifyTownSize(Rule):
    def __init__(self):
        self.name = "modify town size"
        self.regex = re.compile("^(set|increase|decrease) town size (?:to|by) (.+)$")

    def parse(self, line, **kwargs):
        operation, amount = self.get_data(line)
        if operation == "set":
            return Defrule(["true"], ["set-strategic-number sn-maximum-town-size {}".format(amount)])
        return Defrule(["true"], ["up-modify-sn sn-maximum-town-size c:{} {}".format("+" if operation == "increase" else "-", amount)])

@rule
class SetEscrow(Rule):
    def __init__(self):
        self.name = "set escrow"
        self.regex = re.compile("^escrow ([^ ]+) (.+)$")

    def parse(self, line, **kwargs):
        amount, resource = self.get_data(line)
        return Defrule(["true"], ["set-escrow-percentage {} {}".format(resource, amount)])

@rule
class Delete(Rule):
    def __init__(self):
        self.name = "delete"
        self.regex = re.compile("^delete (unit|building) (.+)$")

    def parse(self, line, **kwargs):
        form, name = self.get_data(line)
        return Defrule(["true"], ["delete-{} {}".format(form, name)])

