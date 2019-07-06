import aoeai

PATH = "RULES.md"

document = ["# rules"]

for rule in sorted(aoeai.rules, key=lambda x: x.name):
    document.append(f"## {rule.name}")
    document.append(f"{rule.help}")
    if rule.usage != "":
        document.append("### Usage")
        document.append(f"```\n{rule.usage}\n```")
    if rule.example != "":
        document.append("### Example")
        document.append(f"```\n{rule.example}\n```")

file = open(PATH, "w")
file.write("\n".join(document))
file.close()
