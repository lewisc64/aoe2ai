import aoeai

print(aoeai.interpret("""

#if current-age == dark-age
    train militiaman-line
#else if current-age == feudal-age
    train scout-cavalry-line
    cool = 8
#else
    train scorpion-line
#end if
"""))
