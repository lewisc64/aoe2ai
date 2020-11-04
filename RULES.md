# rules
## add action
Adds an action to the action stack.
### Usage
```
#add action ACTION
    RULES
#remove action
```
## add condition
Adds a condition to the condition stack. 'If' is preferred.
### Usage
```
#add condition CONDITION
    RULES
#remove condition
```
## assign builders
Sets the amount of builders that should build a building.
### Usage
```
assign AMOUNT builders to BUILDING_NAME
```
### Example
```
assign 8 builders to castle
```
## attack
Makes use of the attack-now action.
### Usage
```
attack with AMOUNT units
```
### Example
```
attack
attack with 10 units
```
## block respond
When the AI sees the specified amount, the body is allowed to trigger. If building/unit is unspecified, unit is assumed. If amount is unspecified, 1 is assumed.
### Usage
```
#respond to ?AMOUNT NAME ?BUILDING/UNIT
   RULES
#end respond
```
### Example
```
#respond to 3 scout-cavalry-line
    train 4 spearman-line
#end respond
```
## block respond taunt
Body is allowed to trigger when the taunt is detected from the specified player. Taunt is acknowledged regardless of whether the body successfully triggers.
### Usage
```
#reply to ?ENEMY/ALLY taunt TAUNT_NUMBER
   RULES
#end reply
```
### Example
```
#reply to ally taunt 5
    tribute 100 gold to this-any-ally
#end reply
```
## build
Sets up the rule to build the building. If amount is unspecified, the building will be built infinitely.
### Usage
```
build ?forward AMOUNT BUILDING_NAME with RESOURCE_NAME escrow
```
## build dropoffs
Sets up a rule for automatically refreshing dropoff points.
### Usage
```
build BUILDING_TYPE
```
### Example
```

build lumber camps
build gold mining camps
build stone mining camps
build lumber camps maintaining 4 tiles
```
## build farms
Builds farms according to how many food gatherers should exist.
### Usage
```
build farms
```
## build houses
Sets up rule to build houses, default headroom is 5.
### Usage
```
build houses with AMOUNT headroom
```
### Example
```
build houses
build houses with 10 headroom
```
## build safety mill

### Usage
```
build safety mill
```
## build safety mill

### Usage
```
build safety mill
```
## build walls
Wall placement must be enabled on the same perimeter to function.
### Usage
```
build MATERIAL WALLS/GATES on perimeter PERIMETER_NUMBER
```
### Example
```
build stone walls on perimeter 2
build stone gates on perimeter 2
```
## chat to

### Usage
```
chat to PLAYER_TYPE "MESSAGE"
```
## cheat
Gives the AI resources.
### Usage
```
cheat RESOURCE_NAME AMOUNT
```
## create action
Creates a rule with the action contained within.
### Usage
```
@ACTION
```
### Example
```
@chat-to-all "test"
```
## delay
Block body is only allowed to trigger after the time is up.
### Usage
```
#delay by AMOUNT TIME_UNIT
    RULES
#end delay
```
## delete
Creates a rule that deletes the specified object.
### Usage
```
delete unit/building NAME
```
### Example
```
delete unit villager
```
## delete walls

### Usage
```
delete walls
```
## distribute villagers
Percentages must add up to 100. Makes use of the sn-TYPE-gatherer-percentage strategic number.
### Usage
```
distribute villagers WOOD_PERCENT FOOD_PERCENT GOLD_PERCENT STONE_PERCENT
```
### Example
```
distribute villagers 40 45 10 5
```
## distribute villagers to
Modifies the gatherer percentages. Precomputes, so no constants or facts can be used, just numbers.
### Usage
```
distribute PERCENTAGE villagers from RESOURCE_NAME to RESOURCE_NAME
```
### Example
```
distibute 5 villagers from wood to stone
distribute 10 villagers from wood and food to gold
distribute 8 villagers from food to gold and stone using modifiers
```
## do once
Adds 'disable-self' to the action stack. Makes sure each rule in the block individually runs only once.
### Usage
```
#do once
    RULES
#end do once
```
## drop off food

### Usage
```
drop off food
```
## enable walls
Sets up rule that allows the AI to build walls on the specified perimeter.
### Usage
```
enable walls on perimeter PERIMETER_NUMBER
```
### Example
```
enable walls on perimeter 2
```
## if
Adds a condition to the condition stack.
### Usage
```
#if CONDITION
    RULES
#else if CONDITION
    RULES
#else
    RULES
#end if
```
## load
Loads another aoe2ai file. Relative to main file if trying to load from a seperate file.
### Usage
```
load "PATH"
```
## market
Buys/sells based on a condition.
### Usage
```
buy/sell RESOURCE_NAME when RESOURCE_NAME COMPARISON AMOUNT
```
### Example
```
buy food when gold > 100
sell wood when wood > 1500
```
## modify town size
Modifies the sn-maximum-town-size strategic number.
### Usage
```
set/increase/decrease town size by/to AMOUNT
```
### Example
```
set town size to 32
increase town size by 5
decrease town size by 1
```
## order
Loads another aoe2ai file. Relative to main file if trying to load from a seperate file.
### Usage
```
load "PATH"
```
## repeat
Each rule is allowed to be triggered once after the time has elapsed, the process repeats.
### Usage
```
#repeat every AMOUNT TIME_UNIT
    RULES
#end repeat
```
## research
Sets up the rule to research the specified research.
### Usage
```
research TECH_NAME with RESOURCE_NAME escrow
```
### Example
```
research ri-loom
research feudal-age with food and gold escrow
research blacksmith infantry upgrades
```
## resign

### Usage
```
resign
```
## respond
When the AI sees the specified amount, it reacts with the specified parameters. If building/unit is unspecified, unit is assumed. If amount is unspecified, 1 is assumed.
### Usage
```
respond to ?AMOUNT NAME ?BUILDING/UNIT with NAME ?BUILDING/UNIT
```
## retreat

### Usage
```
retreat
```
## rule

### Usage
```
rule
```
## scout
Sets up the userpatch rule to send the scout somewhere else.
### Usage
```
scout AREA_NAME
```
### Example
```
scout opposite
scout enemy
```
## select random
A random block separated by randors will be allowed to execute. Using persistant mode means the randomly chosen one is picked every time, otherwise it will change.
### Usage
```
#select random ?persistant
   chat to all "option 1!"
#randor
   chat to all "option 2!"
#end select random
```
### Example
```
#select random
   chat to all "option 1!"
#randor
   chat to all "option 2!"
#end select random
```
## set const
Sets a constant. Can only be done once for every name.
### Usage
```
const CONST_NAME = VALUE
```
### Example
```
const max-villagers = 120
```
## set escrow
Creates rule that sets the escrow percentage.
### Usage
```
escrow PERCENTAGE RESOURCE_NAME
```
### Example
```
escrow 50 gold
```
## set goal
Sets a goal, and sets up the constant if it does not already exist.
### Usage
```
goal GOAL_NAME = VALUE
```
### Example
```
goal advance = 1
goal count += 1
```
## set strategic number
Sets a strategic number. Uses the 'sn-' prefix for recognition of the rule.
### Usage
```
STRATEGIC_NUMBER_NAME = VALUE
```
### Example
```
sn-maximum-gold-drop-distance = 8
sn-maximum-town-size += 5
```
## set up basics

### Usage
```
set up basics
```
## set up micro

### Usage
```
set up micro
```
## set up new building system

### Usage
```
set up new building system
```
## set up scouting

### Usage
```
set up scouting
```
## take boar

### Usage
```
take boar
```
## take boar and deer

### Usage
```
take boar and deer
```
## target player
Sets sn-target-player-number and sn-focus-player-number.
### Usage
```
target winning/closest/attacking/random enemy/ally
```
### Example
```
target closest enemy
```
## target walls

### Usage
```
target walls
```
## train
Trains a unit using the specified parameters.
### Usage
```
train UNIT_NAME
train UNIT_NAME with RESOURCE_NAME escrow
train AMOUNT UNIT_NAME
train AMOUNT UNIT_NAME with RESOURCE_NAME escrow
```
### Example
```
train 10 militiaman-line with food and gold escrow
```
## tribute
Gives the specified player resources.
### Usage
```
tribute AMOUNT RESOURCE_NAME to PLAYER_NUMBER
```
### Example
```
tribute 100 wood to any-ally
```
## when
Rules in the 'then' block are allowed to trigger when any rule in the main 'when' block is triggered.
### Usage
```
#when
    RULE
#then
    RULES
#end when
```
## while
Repeats a set of rules until the condition is false.
### Usage
```
#while CONDITION
    RULES
#end WHILE
```