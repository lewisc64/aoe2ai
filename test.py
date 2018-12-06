import aoeai

print(aoeai.interpret("""

goal militant = 0
#add condition town-under-attack
    goal militant = 1
#remove condition

#if goal militant 1

    #when
        build 1 barracks
    #then
        chat to all "I built a barracks. Aren't I cool?"
    #end when

    train militiaman-line

#else if goal militant 0

    build 1 market
    buy food when food < 100

#end if

chat to all \"I'm a cool bean!\"

"""))
