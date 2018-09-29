from .rules import rules
from .defrule import Defrule

def interpret(content):
    timers = []
    condition_stack = []
    action_stack = []
    defrules = []
    
    for i, line in enumerate(content.split("\n")):
        
        item = line.strip()
        if item == "":
            continue
            
        match = False
        for rule in rules:
            if rule.is_match(item):
                
                match = True

                p_condition_stack = condition_stack[:]                
                p_action_stack = action_stack[:]
                
                output = rule.parse(item, defrules=defrules,
                                          condition_stack=condition_stack,
                                          action_stack=action_stack,
                                          timers=timers)
                
                if isinstance(output, Defrule):
                    output.conditions.extend(p_condition_stack)
                    output.actions.extend(p_action_stack)
                    defrules.append(output)
                    
                elif isinstance(output, list):
                    defrules.extend(output)
                
                break
        
        if not match:
            print("WARNING: line {} did not match.".format(i + 1))
    
    return "\n".join([str(x) for x in defrules])