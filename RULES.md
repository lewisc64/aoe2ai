# Rules
Below are all the rules and structures recognised by the parser. They are listed in order of priority.
## rule
### Example
```
rule
```
## take boar
### Example
```
take boar
```
## take boar and deer
### Example
```
take boar and deer
```
## set up new building system
### Example
```
set up new building system
```
## set up scouting
### Example
```
set up scouting
```
## set up micro
### Example
```
set up micro
```
## target walls
### Example
```
target walls
```
## retreat
### Example
```
retreat
```
## resign
### Example
```
resign
```
## drop off food
### Example
```
drop off food
```
## build safety mill
### Example
```
build safety mill
```
## build safety mill
### Example
```
build safety mill
```
## delete walls
### Example
```
delete walls
```
## buy wood
### Example
```
buy wood
```
## buy food
### Example
```
buy food
```
## buy stone
### Example
```
buy stone
```
## sell wood
### Example
```
sell wood
```
## sell food
### Example
```
sell food
```
## sell stone
### Example
```
sell stone
```
## set up basics
### Usage
```
set up basics
```
## lure boars
### Usage
```
lure boars
```
## order
Executes statements in order once every rule pass. Loops back to the beginning upon reaching the end.
### Example
```
train archer-line => train skirmisher-line
```
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
## attack
Makes use of the attack-now action.
### Usage
```
attack with AMOUNT units
```
## auto balance
Redistributes villagers at a set interval around a threshold. By default it will balance every 60 seconds with a threshold of 300. When getting the resource amounts, it subtracts the escrowed portion.
### Usage
```
auto balance RESOURCES around THRESHOLD every AMOUNT seconds
```
### Example
```
auto balance wood and food and gold
auto balance all
auto balance wood and food every 30 seconds
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
## chat to
Sets up a rule for automatically refreshing dropoff points.
### Usage
```
build DROPOFF_TYPE
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
## chat to
Sets up rule to build houses, default headroom is 5.
### Usage
```
build houses with AMOUNT headroom
```
## build walls
Wall placement must be enabled on the same perimeter to function.
### Usage
```
build stone/palisade walls/gates on perimeter PERIMETER_NUMBER
```
## call
Inserts all the rules within a subroutine. Can make replacements as a form of pre-processing.
### Usage
```
call SUBROUTINE_NAME
```
### Example
```
#subroutine train-unit
    train {unit}
#end subroutine

call train-unit(unit="archer-line\")

```
## chat to
### Usage
```
chat to PLAYER_TYPE "MESSAGE"
```
## cheat
Gives the AI resources
### Usage
```
cheat AMOUNT RESOURCE_NAME
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
## do once
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
## distribute villagers
Percentages must add up to 100. Makes use of the sn-TYPE-gatherer-percentage strategic number.
### Usage
```
distribute villagers WOOD_PERCENT FOOD_PERCENT GOLD_PERCENT STONE_PERCENT
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
#end do
```
## enable walls
Sets up rule that allows the AI to build walls on the specified perimeter.
### Usage
```
enable walls on perimeter PERIMETER_NUMBER
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
Loads another aoe2ai file. Tries to load relatively from the current file first, then an absolute path.
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
## no merge
Prevents the rules within from being merged together in compilation.
### Usage
```
#nomerge
   RULES
#end nomerge
```
## repeat
Each rule is allowed to be triggered once after the time has elapsed, the process repeats.
### Usage
```
#repeat every AMOUNT TIME_UNIT
    RULES
#end repeat
```
## reply
Body is allowed to trigger when the taunt is detected from the specified player. Taunt is acknowledged regardless of whether the body successfully triggers.
### Usage
```
#reply to ENEMY/ALLY taunt TAUNT_NUMBER
   RULES
#end reply
```
## respond
When the AI sees the specified amount, it reacts with the specified parameters. If building/unit is unspecified, unit is assumed. If amount is unspecified, 1 is assumed.
### Usage
```
respond to ?AMOUNT NAME ?BUILDING/UNIT with NAME ?BUILDING/UNIT
```
## select random
A random block separated by randors will be allowed to execute. Using persistant mode means the randomly chosen one is picked every time, otherwise it will change.
### Usage
```
#select random ?persistant
   RULES
#randor
   RULES
#end select
```
## attack
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
## set const
Sets a constant. Can only be done once for every name.
### Usage
```
const CONST_NAME = VALUE
```
## set escrow
Creates rule that sets the escrow percentage.
### Usage
```
escrow PERCENTAGE RESOURCE_NAME
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
## target player
Sets sn-target-player-number and sn-focus-player-number.
### Usage
```
target winning/closest/attacking enemy/ally
```
## trade excess
Uses the market to rebalance resources around a certain threshold. Ignores escrowed values.
### Usage
```
trade excess RESOURCE_LIST at AMOUNT
```
### Example
```
trade excess food at 2000
trade excess wood and food and gold at 2000
```
## tribute
Gives the specified player resources.
### Usage
```
tribute AMOUNT RESOURCE_NAME to PLAYER_NUMBER
```
## when
Rules in the 'then' block are allowed to trigger when any rule in the main 'when' block is triggered.
### Usage
```
#when
    RULE
#then ?always
    RULES
#end when
```
### Example
```
#when
    build houses
#then
    chat to all "I built a house!"
#end when
```
## build
Sets up the rule to build the building. If amount is unspecified, the building will be built to a maximum of 100.
### Usage
```
build ?forward AMOUNT BUILDING_NAME with RESOURCE_NAME escrow
```
### Example
```
build 1 barracks
build forward castle
build archery-range with wood escrow
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