from .rules import rules
from .defrule import Defrule

def interpret(content):
    timers = []
    goals = []
    condition_stack = []
    action_stack = []
    data_stack = []
    defrules = []
    defconsts = []

    items = [line.strip() for line in content.split("\n")]
    
    for i, item in enumerate(items):
        
        
        if item == "":
            continue
        
        for rule in rules:
            if rule.is_match(item):

                p_condition_stack = condition_stack[:]                
                p_action_stack = action_stack[:]
                
                output = rule.parse(item, defrules=defrules,
                                          defconsts=defconsts,
                                          condition_stack=condition_stack,
                                          action_stack=action_stack,
                                          data_stack=data_stack,
                                          timers=timers,
                                          goals=goals,
                                          items=items,)
                
                if isinstance(output, Defrule):
                    if not output.ignore_stacks:
                        output.conditions.extend(p_condition_stack)
                        output.actions.extend(p_action_stack)
                    defrules.append(output)
                    
                elif isinstance(output, list):
                    for defrule in [x for x in output if not x.ignore_stacks]:
                        defrule.conditions.extend(p_condition_stack)
                        defrule.actions.extend(p_action_stack)
                    defrules.extend(output)
                
                break
        else:
            print("WARNING: Line {} did not match.".format(i + 1))

        print(item)
        print(condition_stack, action_stack, data_stack)

    
    
    if condition_stack or action_stack or data_stack:
        print("WARNING: Interpretation finished with populated stacks. Remember to end blocks.")
    
    return "\n".join([str(x) for x in defconsts + defrules])
