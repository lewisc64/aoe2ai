class Defrule:
    def __init__(self, conditions, actions, ignore_stacks=False, compressable=True):
        self.format = "(defrule\n{}\n=>\n{}\n)"
        self.conditions = conditions
        self.actions = actions
        self.ignore_stacks = ignore_stacks
        self.compressable = compressable
    
    def format_list(self, items, remove_duplicates=True):
        statement = "    ({})"
        out = []
        for item in items:
            formatted_item = statement.format(item)
            if not remove_duplicates or formatted_item not in out:
                out.append(formatted_item)
        if len(out) > 1:
            if statement.format("true") in out:
                out.remove(statement.format("true"))
            if statement.format("do-nothing") in out:
                out.remove(statement.format("do-nothing"))
        return out
    
    def __str__(self):
        return self.format.format("\n".join(self.format_list(self.conditions)),
                                  "\n".join(self.format_list(self.actions)))

class Defconst:
    def __init__(self, name, value):
        self.format = "(defconst {} {})"
        self.name = name
        self.value = value

    def __str__(self):
        return self.format.format(self.name, self.value)

def parse_condition(condition):
    if " or " in condition:
        sections = condition.split(" or ", 1)
        operator = "or"
    elif " and " in condition:
        sections = condition.split(" and ", 1)
        operator = "and"
    else:
        return condition
    return "{} ({}) ({})".format(operator, parse_condition(sections[0]), parse_condition(sections[1]))
    
    
    

if __name__ == "__main__":
    print(parse_condition("(current-age < castle-age and current-age > dark-age)"))
