import aoeai

content = """
set up scouting
set up micro
set up new building system

build houses
build lumber camps
build mills
build 1 blacksmith
build 1 university
build forward castle

research ri-loom
research ri-double-bit-axe
research ri-crossbow
research ri-fletching
research ri-bodkin-arrow
research ri-padded-archer-armor
research ri-leather-archer-armor
research ri-ring-archer-armor
research ri-bracer
research ri-ballistics
research ri-chemistry

research feudal-age
research castle-age
research imperial-age

train villager

#add condition current-age == dark-age

    distribute villagers 20 80 0 0
    build 3 farm

#remove condition

#add condition current-age == feudal-age

    distribute villagers 20 60 20 0
    build 7 farm
    #repeat every 30 seconds
        attack with 20 units
    #end repeat

#remove condition

#add condition current-age >= feudal-age

    build 1 barracks
    build 2 archery-range
    build gold mining camps
    train archer-line

#remove condition

#add condition current-age >= castle-age

    distribute villagers 20 60 15 5
    build stone mining camps
    build 3 town-center
    build 18 farm
    #repeat every 30 seconds
        attack with 50 units
    #end repeat

#remove condition
"""

print(aoeai.interpret(content)) 
