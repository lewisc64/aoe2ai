import aoeai

print(aoeai.interpret("""

const wobble = 1

goal research-archery-tech = 0

#respond to archer-line

    build wobble archery-range
    train skirmisher-line

    goal research-archery-tech = 1

#end respond

#respond to watch-tower-line building

    build 1 siege-workshop
    train battering-ram

#end respond

#add condition goal research-archery-tech 1
    research ri-fletching
    research ri-bodkin-arrow
    research ri-padded-archer-armor
#remove condition

"""))
