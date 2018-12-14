from .rules import rules
from .defrule import *

def compact_rules(rules):
    i = 0
    while i < len(rules) - 1:
        rule1, rule2 = rules[i:i+2]
        if isinstance(rule1, Defrule) and isinstance(rule2, Defrule) and rule1.compressable and rule2.compressable:
            for condition1, condition2 in zip(sorted(rule1.conditions), sorted(rule2.conditions)):
                if condition1 != condition2:
                    i += 1
                    break
            else:
                if len(rule1.actions) + len(rule2.actions) < 32 and ("disable-self" not in rule1.actions and "disable-self" not in rule2.actions or "disable-self" in rule1.actions and "disable-self" in rule2.actions):
                    rule1.actions.extend(rule2.actions)
                    rules.pop(i+1)
                else:
                    i += 1
                    
        else:
            i += 1
    return rules

def interpret(content):
    timers = []
    goals = []
    constants = []
    condition_stack = []
    action_stack = []
    data_stack = []
    definitions = []

    items = [line.strip() for line in content.split("\n")]

    i = 0
    while i < len(items):

        item = items[i]
        
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
                                          current_position=i)
                
                if isinstance(output, Defrule):
                    if not output.ignore_stacks:
                        output.conditions.extend(p_condition_stack)
                        output.actions.extend(p_action_stack)
                    definitions.append(output)
                    
                elif isinstance(output, list):
                    for defrule in [x for x in output if not x.ignore_stacks]:
                        defrule.conditions.extend(p_condition_stack)
                        defrule.actions.extend(p_action_stack)
                    definitions.extend(output)
                
                break
        else:
            print("WARNING: Line {} did not match:".format(i + 1))
            print(item)

        #print(item, condition_stack, action_stack, data_stack)
        
        i += 1
    
    if condition_stack or action_stack or data_stack:
        print("WARNING: Interpretation finished with populated stacks. Remember to end blocks.")
    
    return compact_rules(definitions)

def translate(content):
    return "\n".join(str(x) for x in interpret(content))
