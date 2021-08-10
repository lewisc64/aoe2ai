# aoe2ai
[![CI](https://github.com/lewisc64/aoe2ai/actions/workflows/CI.yml/badge.svg)](https://github.com/lewisc64/aoe2ai/actions/workflows/CI.yml)

A fast way to write simple Age of Empires 2 AI.

## Usage

```
parsefile.exe -help
```

## Example AI

An AI that will stay in the dark age and pump out militia forever.

```
#do once
    set up basics
    distribute villagers 20 60 20 0
#end do

lure boars

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

build farms
build 1 mill
build houses
build 2 barracks
```

Translation:
```
(defrule
    (true)
=>
    (set-strategic-number sn-enable-new-building-system 1)
    (set-strategic-number sn-percent-building-cancellation 20)
    (set-strategic-number sn-cap-civilian-builders 200)
    (set-strategic-number sn-percent-civilian-explorers 0)
    (set-strategic-number sn-cap-civilian-explorers 0)
    (set-strategic-number sn-total-number-explorers 1)
    (set-strategic-number sn-number-explore-groups 1)
    (set-strategic-number sn-initial-exploration-required 0)
    (set-difficulty-parameter ability-to-maintain-distance 0)
    (set-difficulty-parameter ability-to-dodge-missiles 0)
    (set-strategic-number sn-percent-attack-soldiers 100)
    (set-strategic-number sn-percent-attack-boats 100)
    (set-strategic-number sn-attack-intelligence 1)
    (set-strategic-number sn-livestock-to-town-center 1)
    (set-strategic-number sn-enable-patrol-attack 1)
    (set-strategic-number sn-intelligent-gathering 1)
    (set-strategic-number sn-local-targeting-mode 1)
    (set-strategic-number sn-retask-gather-amount 0)
    (set-strategic-number sn-target-evaluation-siege-weapon 500)
    (set-strategic-number sn-ttkfactor-scalar 500)
    (set-strategic-number sn-percent-enemy-sighted-response 100)
    (set-strategic-number sn-task-ungrouped-soldiers 0)
    (set-strategic-number sn-gather-defense-units 1)
    (set-strategic-number sn-defer-dropsite-update 1)
    (set-strategic-number sn-do-not-scale-for-difficulty-level 1)
    (set-strategic-number sn-number-build-attempts-before-skip 5)
    (set-strategic-number sn-max-skips-per-attempt 5)
    (set-strategic-number sn-dropsite-separation-distance 8)
    (disable-self)
)
(defrule
    (true)
=>
    (set-strategic-number sn-wood-gatherer-percentage 20)
    (set-strategic-number sn-food-gatherer-percentage 60)
    (set-strategic-number sn-gold-gatherer-percentage 20)
    (set-strategic-number sn-stone-gatherer-percentage 0)
    (set-strategic-number sn-enable-boar-hunting 2)
    (set-strategic-number sn-minimum-number-hunters 1)
    (set-strategic-number sn-minimum-boar-lure-group-size 1)
    (set-strategic-number sn-minimum-boar-hunt-group-size 1)
    (set-strategic-number sn-maximum-hunt-drop-distance 48)
    (disable-self)
)
(defrule
    (dropsite-min-distance live-boar < 4)
=>
    (up-request-hunters c: 8)
    (set-strategic-number sn-minimum-number-hunters 8)
)
(defrule
    (strategic-number sn-minimum-number-hunters == 8)
    (and (dropsite-min-distance live-boar > 4) (or (dropsite-min-distance boar-food > 4) (dropsite-min-distance boar-food == -1)))
=>
    (set-strategic-number sn-minimum-number-hunters 1)
    (up-retask-gatherers food c: 255)
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
    (dropsite-min-distance wood != -1)
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
    (disable-self)
)
(defrule
    (or (dropsite-min-distance gold > 3) (and (unit-type-count 579 == 0) (and (unit-type-count 581 == 0) (strategic-number sn-gold-gatherer-percentage > 0))))
    (dropsite-min-distance gold != -1)
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
    (true)
=>
    (set-goal 1 0)
    (up-get-fact unit-type-count villager 2)
    (up-modify-goal 2 s:* sn-food-gatherer-percentage)
    (up-modify-goal 2 c:/ 100)
    (up-get-fact building-type-count-total farm 3)
)
(defrule
    (up-compare-goal 3 g:< 2)
    (can-build farm)
=>
    (build farm)
)
(defrule
    (can-build mill)
    (building-type-count-total mill < 1)
=>
    (build mill)
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
```
