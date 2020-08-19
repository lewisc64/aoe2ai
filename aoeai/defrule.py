import re

class Defrule:
    def __init__(self, conditions, actions, ignore_stacks=False, compressable=True, tag=None):
        self.format = "(defrule\n{}\n=>\n{}\n)"
        self.conditions = conditions
        self.actions = actions
        self.ignore_stacks = ignore_stacks
        self.compressable = compressable
        self.tag = tag
    
    def format_list(self, items, remove_duplicates=True):
        statement = "    ({})"
        out = []
        for item in items:
            formatted_item = statement.format(item)
            if (not remove_duplicates and item != "disable-self") or formatted_item not in out:
                out.append(formatted_item)
        if len(out) > 1:
            if statement.format("true") in out:
                out.remove(statement.format("true"))
            if statement.format("do-nothing") in out:
                out.remove(statement.format("do-nothing"))
        return out
    
    def __str__(self):
        return self.format.format("\n".join(self.format_list(self.conditions)),
                                  "\n".join(self.format_list(self.actions, remove_duplicates=False)))

class Defconst:
    def __init__(self, name, value, tag=None):
        self.format = "(defconst {} {})"
        self.name = name
        self.value = value
        self.tag = tag

    def __str__(self):
        return self.format.format(self.name, self.value)

def unbracket(condition):
    bracket_level = 0
    record = ""
    out = ""
    for char in condition:
        if char == "(":
            bracket_level += 1
            if bracket_level == 1:
                continue
        elif char == ")":
            bracket_level -= 1
            if bracket_level == 0:
                out += f" {parse_condition_internal(record)} "
                record = ""
                continue
        if bracket_level > 0:
            record += char
        else:
            out += char
    return out.strip()

def parse_condition_internal(condition):
    condition = unbracket(condition)
    
    or_regex = r"(?<=\)| )or(?=\(| )(?!.?\[)"
    and_regex = r"(?<=\)| )and(?=\(| )(?!.?\[)"
    not_regex = r"(?: |^)not(?=\(| )"
    
    binary = True
    if re.match(f".+{or_regex}.+", condition):
        sections = re.split(or_regex, condition, 1)
        operator = "or"
    elif re.match(f".+{and_regex}.+", condition):
        sections = re.split(and_regex, condition, 1)
        operator = "and"
    elif re.match(f".*{not_regex}.+", condition):
        sections = re.split(not_regex, condition, 1)
        operator = "not"
        binary = False
    else:
        return condition

    if binary:
        return "{} [{}] [{}]".format(operator, parse_condition_internal(sections[0]), parse_condition_internal(sections[1]))
    else:
        return sections[0] + "{} [{}]".format(operator, parse_condition_internal(sections[1]))

def parse_condition(condition):
    return parse_condition_internal(condition).replace("[", "(").replace("]", ")")

if __name__ == "__main__":
    assert parse_condition("a") == "a"
    assert parse_condition("not a") == "not (a)"
    assert parse_condition("a or b") == "or (a) (b)"
    assert parse_condition("a and b") == "and (a) (b)"
    assert parse_condition("(((((a)or(b)))))") == "or (a) (b)"
    assert parse_condition("a and b or c and d") == "or (and (a) (b)) (and (c) (d))"
    assert parse_condition("a and(b or c) and d") == "and (a) (and (or (b) (c)) (d))"
    assert parse_condition("a and not b") == "and (a) (not (b))"
