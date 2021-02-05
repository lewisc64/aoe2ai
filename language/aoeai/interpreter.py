from .rules import rules
from .defrule import *

def ensure_rule_length(rules, rule_length=32):

    for i in range(len(rules) - 1, -1, -1):
        
        rule = rules[i]
        if not isinstance(rule, Defrule):
            continue
        
        if len(rule.actions) + len(rule.conditions) > rule_length:
            if not rule.compressable:
                print("WARNING: An incompressable rule is being split.")
            
            rules.remove(rule)
            
            disable_self = "disable-self" in rule.actions
            if disable_self:
                rule.actions.remove("disable-self")

            step = rule_length - (2 if disable_self else 0)
            
            for j in range(0, len(rule.actions), step):
                
                rules.insert(i, Defrule(rule.conditions, rule.actions[j:j+step]))
                if disable_self and not "disable-self" in rules[i].actions:
                    rules[i].actions.append("disable-self")
    
    i = 0
    while i < len(rules) - 1:
        rule1, rule2 = rules[i:i+2]
        if isinstance(rule1, Defrule) and isinstance(rule2, Defrule) and rule1.compressable and rule2.compressable:
            rule1_conditions = [x for x in rule1.conditions if x != "true"]
            rule2_conditions = [x for x in rule2.conditions if x != "true"]
            if len(rule1_conditions) == len(rule2_conditions):
                for condition1, condition2 in zip(sorted(rule1_conditions), sorted(rule2_conditions)):
                    if condition1 != condition2:
                        break
                else:
                    if "disable-self" not in rule1.actions and "disable-self" not in rule2.actions or "disable-self" in rule1.actions and "disable-self" in rule2.actions:
                        if len(rule1.actions) + len(rule2.actions) + len(rule2.conditions) <= rule_length:
                            rule1.actions.extend(rule2.actions)
                            rules.pop(i+1)
                            continue
        i += 1

    for rule in [x for x in rules if isinstance(x, Defrule)]:
        if "false" in rule.conditions and rule.compressable:
            rules.remove(rule)
    
    return rules

def interpret(content, timers=None, goals=None, constants=None, userpatch=False, content_identifier=None):
    if timers is None:
        timers = []
    if goals is None:
        goals = []
    if constants is None:
        constants = []
    condition_stack = []
    action_stack = []
    data_stack = []
    definitions = []

    items = [line.strip() for line in content.split("\n")]

    i = 0
    while i < len(items):

        item = items[i].split("//")[0].strip()
        
        if item == "":
            i += 1
            continue
        
        for rule in rules:
            if rule.is_match(item):

                p_condition_stack = condition_stack[:]                
                p_action_stack = action_stack[:]
                
                output = rule.parse(item, definitions=definitions,
                                          condition_stack=condition_stack,
                                          action_stack=action_stack,
                                          data_stack=data_stack,
                                          timers=timers,
                                          goals=goals,
                                          constants=constants,
                                          items=items,
                                          current_position=i,
                                          userpatch=userpatch)
                
                if isinstance(output, Defrule):
                    if not output.ignore_stacks:
                        output.conditions.extend(p_condition_stack)
                        output.actions.extend(p_action_stack)
                    definitions.append(output)
                    
                elif isinstance(output, list):
                    for defrule in [x for x in output if isinstance(x, Defconst) or not x.ignore_stacks]:
                        if not isinstance(defrule, Defconst):
                            defrule.conditions.extend(p_condition_stack)
                            defrule.actions.extend(p_action_stack)
                    definitions.extend(output)
                
                break
        else:
            print(f"WARNING: Line {i + 1} did not match: {item}")
            if content_identifier is not None:
                print(f"    in file {content_identifier}")
        
        i += 1
    
    if condition_stack or action_stack or data_stack:
        print("WARNING: Interpretation finished with populated stacks. Remember to end blocks.")
        if content_identifier is not None:
            print(f"    in file {content_identifier}")
    
    return ensure_rule_length(definitions, rule_length=(16 if userpatch else 32))

def translate(content, stamp=False, userpatch=False):
    return (";Translated by https://github.com/lewisc64/aoe2ai\n" if stamp else "") + "\n".join(str(x) for x in interpret(content, userpatch=userpatch))
