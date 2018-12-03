class Defrule:
    def __init__(self, conditions, actions, ignore_stacks=False):
        self.format = "(defrule\n{}\n=>\n{}\n)"
        self.conditions = conditions
        self.actions = actions
        self.ignore_stacks = ignore_stacks
    
    def format_list(self, items):
        statement = "    ({})"
        out = []
        for item in items:
            out.append(statement.format(item))
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

class Binary:
    def __init__(self, word, *conditions):
        self.format = "(" + word + " {} {})"
        self.conditions = conditions
    
    def __str__(self):
        return self.format.format(*self.conditions)
    
