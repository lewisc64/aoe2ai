import re
from .defrule import *

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
        return Defrule(self.conditions, self.actions)

rules.append(Snippet("rule",
                     ["true"],
                     ["do-nothing"]))
rules.append(Snippet("take boar",
                     ["true"],
                     ["set-strategic-number sn-enable-boar-hunting 1",
                      "set-strategic-number sn-minimum-number-hunters 3"]))
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
                      "set-strategic-number sn-enable-patrol-attack 1"]))
rules.append(Snippet("build houses",
                     ["housing-headroom < 5", "population-headroom != 0", "up-pending-objects c: house == 0", "can-build house"],
                     ["build house"]))
rules.append(Snippet("build lumber camps",
                     ["dropsite-min-distance wood > 2", "resource-found wood", "up-pending-objects c: lumber-camp == 0", "can-build lumber-camp"],
                     ["build lumber-camp"]))
rules.append(Snippet("build mills",
                     ["dropsite-min-distance food > 3", "resource-found food", "up-pending-objects c: mill == 0", "can-build mill"],
                     ["build mill"]))
rules.append(Snippet("target walls",
                     ["true"],
                     ["set-strategic-number sn-wall-targeting-mode 1"]))

@rule
class Comment(Rule):
    def __init__(self):
        self.name = "comment"
        self.regex = re.compile("^//(.+)$")
    
    def parse(self, line, **kwargs):
        return None

@rule
class Cheat(Rule):
    def __init__(self):
        self.name = "cheat"
        self.regex = re.compile("^cheat ([0-9]+) ([^ ]+)$")

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
            kwargs["condition_stack"].append(condition)

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

#@rule
class If(Rule):
    def __init__(self):
        self.name = "if"
        self.regex = re.compile("^(?:#if (.+)|#elseif (.+)|#else|#end if)$")
    
    def parse(self, line, **kwargs):
        condition = self.get_data(line)[0]
        if condition is None:
            
            text = kwargs["condition_stack"].pop()
            
            if line == "#else":
                kwargs["condition_stack"].append("not ({})".format(text))
                
            elif line.startswith("#elseif"):
                condition = self.get_data(line)[1]
                kwargs["condition_stack"].append("and (not ({})) {}".format(text, condition))
        else:
            kwargs["condition_stack"].append(condition)

@rule
class If(Rule):
    def __init__(self):
        self.name = "if"
        self.regex = re.compile("^(?:#if (.+)|#else if (.+)|#else|#end if)$")
    
    def parse(self, line, **kwargs):
        if line.startswith("#end"):
            if not kwargs["condition_stack"][-1].startswith("goal"):
                kwargs["condition_stack"].pop()
            kwargs["condition_stack"].pop()
            kwargs["data_stack"].pop()
            return
        
        condition_if, condition_elseif = self.get_data(line)

        if condition_if is None:
            old_condition = kwargs["condition_stack"].pop()
            if condition_elseif is not None:
                kwargs["condition_stack"].append(condition_elseif)
            return Defrule([old_condition], ["set-goal {} 0".format(kwargs["data_stack"][-1])], ignore_stacks=True)
        
        else:
            goal = len(kwargs["goals"]) + 1
            kwargs["goals"].append(goal)
            kwargs["data_stack"].append(goal)
            kwargs["condition_stack"].append("goal {} 1".format(goal))
            kwargs["condition_stack"].append(condition_if)
            return Defrule(["true"], ["set-goal {} 1".format(goal)])

@rule
class When(Rule):
    def __init__(self):
        self.name = "when"
        self.regex = re.compile("^(?:#when|#then|#end when)$")
    
    def parse(self, line, **kwargs):
        condition = self.get_data(line)[0]
        if condition is None:
            text = kwargs["condition_stack"].pop(-1)
            if line.startswith("#else"):
                kwargs["condition_stack"].append("not ({})".format(text))
        else:
            kwargs["condition_stack"].append(condition)

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
        self.regex = re.compile("^(?:#delay by ([0-9]+) seconds|#end delay)$")
    
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
        self.regex = re.compile("^(?:#repeat every ([0-9]+) seconds?|#end repeat)$")
    
    def parse(self, line, **kwargs):
        if "end" in line:
            kwargs["condition_stack"].pop(-1)
        else:
            amount = self.get_data(line)[0]
            timer_number = len(kwargs["timers"]) + 1
            kwargs["timers"].append(timer_number)
            kwargs["condition_stack"].append("timer-triggered {}".format(timer_number))
            return [Defrule(["timer-triggered {}".format(timer_number)], ["disable-timer {}".format(timer_number), "enable-timer {} {}".format(timer_number, amount), "disable-self"]),
                    Defrule(["true"], ["enable-timer {} {}".format(timer_number, amount), "disable-self"])]

@rule
class Train(Rule):
    def __init__(self):
        self.name = "train"
        self.regex = re.compile("^train (?:([0-9]+) )?([^ ]+)$")

    def parse(self, line, **kwargs):
        data = self.get_data(line)
        if data[0] is None:
            return Defrule(["can-train " + data[1]], ["train " + data[1]])
        else:
            return Defrule(["unit-type-count-total {} < {}".format(data[1], data[0]), "can-train " + data[1]], 
                           ["train " + data[1]])

@rule
class Respond(Rule):
    def __init__(self):
        self.name = "respond"
        self.regex = re.compile("^respond to (?:([0-9]+) )?([^ ]+) with ([^ ]+)$")

    def parse(self, line, **kwargs):
        data = self.get_data(line)
        return Defrule(["players-unit-type-count any-enemy {} >= {}".format(data[1], 0 if data[0] is None else data[0]),
                        "can-train {}".format(data[2])],
                       ["train {}".format(data[2])])

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
        self.regex = re.compile("^build (forward )?(?:([0-9]+) )?([^ ]+)$")

    def parse(self, line, **kwargs):
        forward, amount, building = self.get_data(line)
        
        mod = "" if forward is None else "-forward"
        conditions = ["can-build " + building]
        
        if amount is not None:
            conditions.append("building-type-count-total {} < {}".format(building, amount))
        
        return Defrule(conditions, ["build{} {}".format(mod, building)])

@rule
class Research(Rule):
    def __init__(self):
        self.name = "research"
        self.regex = re.compile("^research ([^ ]+)$")
    
    def parse(self, line, **kwargs):
        tech = self.get_data(line)[0]
        return Defrule(["can-research " + tech], ["research " + tech])

@rule
class Attack(Rule):
    def __init__(self):
        self.name = "attack"
        self.regex = re.compile("^attack(?: with ([0-9]+) units)?$")
    
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
        self.regex = re.compile("^(buy|sell) ([^ ]+) when ([^ +]+) ([<>!=]+) ([0-9]+)$")

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
class SetGoal(Rule):
    def __init__(self):
        self.name = "set goal"
        self.regex = re.compile("^([^ ]+) = (.+)$")
        self.replacements = {"true":1, "false":0}

    def parse(self, line, **kwargs):
        name, value = self.get_data(line)
        if value in self.replacements:
            value = self.replacements[value]
        for defconst in kwargs["defconsts"]:
            if defconst.name == name:
                break
        else:
            goal = len(kwargs["goals"]) + 1
            kwargs["goals"].append(goal)
            kwargs["defconsts"].append(Defconst(name, goal))
        return Defrule(["true"], ["set-goal {} {}".format(name, value)])
        
