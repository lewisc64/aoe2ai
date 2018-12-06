from .rules import rules
from .defrule import Defrule

def interpret(content):
    timers = []
    goals = []
    constants = []
    condition_stack = []
    action_stack = []
    data_stack = []
    definitions = []

    items = [line.strip() for line in content.split("\n")]
    
    for i, item in enumerate(items):
        
        
        if item == "":
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
                                          items=items,)
                
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
            print("WARNING: Line {} did not match.".format(i + 1))
    
    if condition_stack or action_stack or data_stack:
        print("WARNING: Interpretation finished with populated stacks. Remember to end blocks.")
    
    return "\n".join([str(x) for x in definitions])
