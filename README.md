# aoe2ai
A fast way to write simple Age of Empires 2 AI.

## Usage

```
parsefile.exe -help
```

## Example AI

An AI that will stay in the dark age and pump out militia forever.

```
#do once

    set up new building system
    set up scouting
    set up micro
    
    #delay by 200 seconds
        take boar
    #end delay
    
    sn-task-ungrouped-soldiers = 0
    
    distribute villagers 20 60 20 0
    
#end do once

train 130 villager
train militiaman-line

attack with 10 units

research ri-loom

build lumber camps
#when
    build gold mining camps
#then
    sn-camp-max-distance += 3
#end when

build houses
build 2 barracks
build 1 mill
build 10 farm
```

Translation:
```
(defrule
    (true)
=>
    (set-strategic-number sn-enable-new-building-system 1)
    (set-strategic-number sn-percent-building-cancellation 20)
    (set-strategic-number sn-cap-civilian-builders 200)
    (disable-self)
    (set-strategic-number sn-percent-civilian-explorers 0)
    (set-strategic-number sn-cap-civilian-explorers 0)
    (set-strategic-number sn-total-number-explorers 1)
    (set-strategic-number sn-number-explore-groups 1)
    (set-strategic-number sn-initial-exploration-required 0)
    (set-difficulty-parameter ability-to-maintain-distance 0)
    (set-difficulty-parameter ability-to-dodge-missiles 0)
    (set-strategic-number sn-attack-intelligence 1)
    (set-strategic-number sn-livestock-to-town-center 1)
    (set-strategic-number sn-enable-patrol-attack 1)
    (set-strategic-number sn-intelligent-gathering 1)
    (set-strategic-number sn-local-targeting-mode 1)
    (set-strategic-number sn-percent-enemy-sighted-response 100)
    (enable-timer 1 200)
)
(defrule
    (timer-triggered 1)
=>
    (set-strategic-number sn-enable-boar-hunting 2)
    (set-strategic-number sn-minimum-number-hunters 3)
    (set-strategic-number sn-minimum-boar-lure-group-size 3)
    (set-strategic-number sn-minimum-boar-hunt-group-size 3)
    (disable-self)
)
(defrule
    (true)
=>
    (set-strategic-number sn-task-ungrouped-soldiers 0)
    (disable-self)
    (set-strategic-number sn-wood-gatherer-percentage 20)
    (set-strategic-number sn-food-gatherer-percentage 60)
    (set-strategic-number sn-gold-gatherer-percentage 20)
    (set-strategic-number sn-stone-gatherer-percentage 0)
)
(defrule
    (can-train villager)
    (unit-type-count-total villager < 130)
=>
    (train villager)
)
(defrule
    (can-train militiaman-line)
=>
    (train militiaman-line)
)
(defrule
    (military-population >= 10)
=>
    (attack-now)
)
(defrule
    (can-research ri-loom)
=>
    (research ri-loom)
)
(defrule
    (dropsite-min-distance wood > 2)
    (resource-found wood)
    (up-pending-objects c: lumber-camp == 0)
    (can-build lumber-camp)
=>
    (build lumber-camp)
)
(defrule
    (true)
=>
    (set-goal 1 0)
)
(defrule
    (dropsite-min-distance gold > 3)
    (resource-found gold)
    (up-pending-objects c: mining-camp == 0)
    (can-build mining-camp)
=>
    (build mining-camp)
    (set-goal 1 1)
)
(defrule
    (goal 1 1)
=>
    (up-modify-sn sn-camp-max-distance c:+ 3)
)
(defrule
    (population-headroom != 0)
    (up-pending-objects c: house == 0)
    (can-build house)
    (housing-headroom < 5)
=>
    (build house)
)
(defrule
    (can-build barracks)
    (building-type-count-total barracks < 2)
=>
    (build barracks)
)
(defrule
    (can-build mill)
    (building-type-count-total mill < 1)
=>
    (build mill)
)
(defrule
    (can-build farm)
    (building-type-count-total farm < 10)
=>
    (build farm)
)
```
