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

manage scouting
lure boars
push deer
micro sheep

train 130 villager
train militiaman-line

#repeat every 60 seconds
  attack with 10 units
#end repeat

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
    (set-difficulty-parameter ability-to-maintain-distance 0)
    (set-difficulty-parameter ability-to-dodge-missiles 100)
    (set-strategic-number sn-percent-attack-soldiers 100)
    (set-strategic-number sn-percent-attack-boats 100)
    (set-strategic-number sn-attack-intelligence 1)
    (set-strategic-number sn-livestock-to-town-center 1)
    (set-strategic-number sn-enable-patrol-attack 1)
    (set-strategic-number sn-local-targeting-mode 1)
    (set-strategic-number sn-percent-enemy-sighted-response 100)
    (set-strategic-number sn-task-ungrouped-soldiers 0)
    (set-strategic-number sn-gather-defense-units 1)
    (set-strategic-number sn-defer-dropsite-update 1)
    (set-strategic-number sn-do-not-scale-for-difficulty-level 1)
    (set-strategic-number sn-dropsite-separation-distance 8)
    (set-strategic-number sn-wall-targeting-mode 1)
    (set-strategic-number sn-minimum-water-body-size-for-dock 999)
    (set-strategic-number sn-consecutive-idle-unit-limit 1)
    (set-strategic-number sn-enable-offensive-priority 1)
    (set-strategic-number sn-zero-priority-distance 255)
    (set-strategic-number sn-scale-minimum-attack-group-size 0)
    (set-strategic-number sn-garrison-rams 0)
    (disable-self)
)
(defrule
    (true)
=>
    (set-strategic-number sn-maximum-wood-drop-distance -1)
    (set-strategic-number sn-maximum-food-drop-distance 8)
    (set-strategic-number sn-maximum-gold-drop-distance 8)
    (set-strategic-number sn-maximum-stone-drop-distance 8)
    (set-strategic-number sn-maximum-hunt-drop-distance 48)
    (set-strategic-number sn-mill-max-distance 25)
    (set-strategic-number sn-camp-max-distance 25)
    (set-strategic-number sn-dropsite-separation-distance 5)
    (set-strategic-number sn-allow-adjacent-dropsites 1)
    (set-strategic-number sn-percent-civilian-explorers 0)
    (set-strategic-number sn-cap-civilian-explorers 0)
    (set-strategic-number sn-total-number-explorers 1)
    (set-strategic-number sn-number-explore-groups 1)
    (set-strategic-number sn-initial-exploration-required 0)
    (set-strategic-number sn-wood-gatherer-percentage 20)
    (set-strategic-number sn-food-gatherer-percentage 60)
    (set-strategic-number sn-gold-gatherer-percentage 20)
    (set-strategic-number sn-stone-gatherer-percentage 0)
    (set-strategic-number sn-percent-civilian-explorers 0)
    (set-strategic-number sn-cap-civilian-explorers 0)
    (set-strategic-number sn-total-number-explorers 1)
    (set-strategic-number sn-number-explore-groups 1)
    (set-strategic-number sn-initial-exploration-required 0)
    (disable-self)
)
(defrule
    (soldier-count == 0)
    (or
      (game-time < 600)
      (nand
        (resource-found gold)
        (resource-found stone)
      )
    )
    (strategic-number sn-cap-civilian-explorers == 0)
=>
    (set-strategic-number sn-percent-civilian-explorers 100)
    (set-strategic-number sn-cap-civilian-explorers 1)
)
(defrule
    (or
      (soldier-count >= 1)
      (and
        (game-time >= 600)
        (and
          (resource-found gold)
          (resource-found stone)
        )
      )
    )
    (strategic-number sn-cap-civilian-explorers == 1)
=>
    (set-strategic-number sn-percent-civilian-explorers 0)
    (set-strategic-number sn-cap-civilian-explorers 0)
)
(defrule
    (game-time < 600)
=>
    (up-full-reset-search)
    (up-find-local c: scout-cavalry-line c: 1)
    (up-find-local c: eagle-warrior-line c: 1)
    (up-find-local c: camel-line c: 1)
    (up-remove-objects search-local object-data-action == actionid-move)
    (up-modify-sn sn-focus-player-number c:= 0)
    (up-set-target-object search-local c: 0)
    (up-get-point position-object 41)
    (up-set-target-point 41)
    (up-filter-distance c: -1 c: 20)
    (up-find-remote c: livestock-class c: 1)
    (up-target-objects 0 action-move -1 -1)
)
(defrule
    (dropsite-min-distance live-boar != -1)
=>
    (set-strategic-number sn-enable-boar-hunting 2)
    (set-strategic-number sn-minimum-number-hunters 1)
    (set-strategic-number sn-minimum-boar-lure-group-size 1)
    (set-strategic-number sn-minimum-boar-hunt-group-size 1)
    (set-strategic-number sn-maximum-hunt-drop-distance 48)
)
(defrule
    (dropsite-min-distance live-boar == -1)
=>
    (set-strategic-number sn-enable-boar-hunting 1)
    (set-strategic-number sn-minimum-number-hunters 0)
    (set-strategic-number sn-minimum-boar-lure-group-size 0)
    (set-strategic-number sn-minimum-boar-hunt-group-size 0)
    (set-strategic-number sn-maximum-hunt-drop-distance 8)
)
(defrule
    (dropsite-min-distance live-boar < 4)
    (dropsite-min-distance live-boar >= 0)
=>
    (up-request-hunters c: 8)
)
(defrule
    (food-amount < 50)
    (up-pending-objects c: villager <= 1)
=>
    (up-drop-resources c: boar-food 10)
)
(defrule
    (dropsite-min-distance deer-hunting != 255)
    (dropsite-min-distance deer-hunting != -1)
=>
    (up-full-reset-search)
    (up-get-point position-self 43)
    (up-set-target-point 43)
    (up-modify-goal 43 c:* 100)
    (up-modify-goal 44 c:* 100)
    (set-strategic-number sn-focus-player-number 0)
    (up-filter-distance c: -1 c: 30)
    (up-find-remote c: 909 c: 100)
    (up-get-search-state 1)
)
(defrule
    (dropsite-min-distance deer-hunting != 255)
    (dropsite-min-distance deer-hunting != -1)
    (up-compare-goal 3 c:>= 1)
=>
    (up-clean-search search-remote 44 1)
    (up-set-target-object search-remote c: 0)
    (up-get-object-data 38 45)
    (up-get-object-data 39 46)
    (up-lerp-tiles 45 43 c: -150)
    (up-reset-filters)
    (up-find-local c: scout-cavalry-line c: 1)
    (up-find-local c: eagle-warrior-line c: 1)
    (up-find-local c: camel-line c: 1)
    (set-strategic-number sn-target-point-adjustment 6)
    (up-target-point 45 action-default -1 -1)
    (set-strategic-number sn-target-point-adjustment 0)
)
(defrule
    (dropsite-min-distance deer-hunting <= 4)
    (dropsite-min-distance deer-hunting != -1)
=>
    (up-full-reset-search)
    (up-find-local c: town-center c: 1)
    (up-set-target-object search-local c: 0)
    (up-get-point position-object 43)
    (up-set-target-point 43)
    (up-reset-search 1 1 0 0)
    (up-filter-distance c: -1 c: 4)
    (up-find-local c: villager-class c: 4)
    (up-remove-objects search-local object-data-target == 909)
    (up-find-remote c: 909 c: 1)
    (up-get-search-state 1)
)
(defrule
    (up-compare-goal 1 >= 1)
    (up-compare-goal 3 >= 1)
=>
    (up-target-objects 0 action-default -1 -1)
    (up-send-scout group-type-land-explore scout-border)
)
(defrule
    (civ-selected gurjaras)
=>
    (up-jump-rule 7)
)
(defrule
    (true)
=>
    (up-full-reset-search)
    (up-get-point position-self 51)
    (up-set-target-point 51)
    (up-modify-sn sn-focus-player-number c:= 0)
    (up-filter-distance c: -1 c: 10)
    (up-filter-status c: 5 c: 0)
    (up-find-status-remote c: livestock-class c: 1)
    (up-get-search-state 47)
)
(defrule
    (goal 49 0)
=>
    (up-filter-status c: 4 c: 0)
    (up-find-status-remote c: livestock-class c: 1)
    (up-get-search-state 47)
)
(defrule
    (up-compare-goal 49 c:>= 1)
=>
    (up-reset-filters)
    (up-find-local c: 592 c: 255)
    (up-find-local c: 590 c: 255)
    (up-target-objects 0 action-default -1 -1)
)
(defrule
    (goal 49 0)
=>
    (up-full-reset-search)
    (up-get-fact player-number 0 5)
    (up-modify-sn sn-focus-player-number g:= 5)
    (up-find-remote c: livestock-class c: 1)
    (up-find-local c: 592 c: 255)
    (up-find-local c: 590 c: 255)
    (up-target-objects 0 action-default -1 -1)
)
(defrule
    (true)
=>
    (up-full-reset-search)
    (up-get-fact player-number 0 5)
    (up-modify-sn sn-focus-player-number g:= 5)
    (up-find-local c: livestock-class c: 255)
    (up-remove-objects search-local -1 == 0)
    (up-find-remote c: town-center c: 1)
    (up-set-target-object search-remote c: 0)
    (up-get-point position-object 51)
    (up-get-point position-center 53)
    (up-lerp-tiles 51 53 c: -3)
    (up-target-point 51 action-default -1 -1)
)
(defrule
    (true)
=>
    (up-full-reset-search)
    (up-modify-sn sn-focus-player-number c:= 0)
    (up-filter-distance c: -1 c: 10)
    (up-filter-status c: 5 c: 0)
    (up-find-status-remote c: livestock-class c: 1)
    (up-find-local c: livestock-class c: 1)
    (up-remove-objects search-remote object-data-carry > 30)
    (up-get-search-state 47)
)
(defrule
    (up-compare-goal 49 c:>= 1)
=>
    (up-full-reset-search)
    (up-get-fact player-number 0 5)
    (up-modify-sn sn-focus-player-number g:= 5)
    (up-find-local c: livestock-class c: 1)
    (up-find-remote c: town-center c: 1)
    (set-strategic-number sn-target-point-adjustment 5)
    (up-target-objects 0 action-move -1 -1)
    (set-strategic-number sn-target-point-adjustment 0)
)
(defrule
    (not
      (civ-selected gurjaras)
    )
=>
    (up-jump-rule 1)
)
(defrule
    (true)
=>
    (up-full-reset-search)
    (up-modify-sn sn-focus-player-number c:= 0)
    (up-find-local c: town-center c: 1)
    (up-set-target-object search-local c: 0)
    (up-get-point position-object 51)
    (up-set-target-point 51)
    (up-reset-search 1 1 0 0)
    (up-find-local c: 592 c: 255)
    (up-find-local c: 590 c: 255)
    (up-filter-distance c: -1 c: 30)
    (up-find-remote c: 907 c: 255)
    (up-target-objects 0 action-default -1 -1)
    (up-reset-search 1 1 1 1)
    (up-get-fact player-number 0 5)
    (up-modify-sn sn-focus-player-number g:= 5)
    (up-reset-filters)
    (up-find-local c: livestock-class c: 255)
    (up-find-remote c: mill c: 1)
    (up-target-objects 0 action-garrison -1 -1)
    (up-reset-search 1 1 0 0)
    (up-find-local c: mill c: 1)
    (up-target-objects 0 action-gather -1 -1)
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
    (true)
=>
    (enable-timer 1 60)
    (disable-self)
)
(defrule
    (military-population >= 10)
    (timer-triggered 1)
=>
    (attack-now)
)
(defrule
    (timer-triggered 1)
=>
    (disable-timer 1)
    (enable-timer 1 60)
)
(defrule
    (can-research ri-loom)
=>
    (research ri-loom)
)
(defrule
    (or
      (and
        (dropsite-min-distance wood > 2)
        (resource-found wood)
      )
      (and
        (game-time >= 60)
        (building-type-count-total lumber-camp == 0)
      )
    )
    (up-pending-objects c: lumber-camp == 0)
    (can-build lumber-camp)
    (dropsite-min-distance wood != 255)
=>
    (build lumber-camp)
)
(defrule
    (true)
=>
    (set-goal 6 0)
    (disable-self)
)
(defrule
    (or
      (dropsite-min-distance gold > 3)
      (and
        (unit-type-count 579 == 0)
        (and
          (unit-type-count 581 == 0)
          (strategic-number sn-gold-gatherer-percentage > 0)
        )
      )
    )
    (resource-found gold)
    (up-pending-objects c: mining-camp == 0)
    (can-build mining-camp)
    (dropsite-min-distance gold != 255)
=>
    (build mining-camp)
    (set-goal 6 1)
)
(defrule
    (goal 6 1)
=>
    (up-modify-sn sn-camp-max-distance c:+ 3)
)
(defrule
    (true)
=>
    (set-goal 6 0)
    (up-get-fact unit-type-count villager 5)
    (up-modify-goal 5 s:* sn-food-gatherer-percentage)
    (up-modify-goal 5 c:/ 100)
    (up-get-fact building-type-count-total farm 7)
)
(defrule
    (up-compare-goal 7 g:< 5)
    (can-build farm)
=>
    (build farm)
)
(defrule
    (can-build mill)
    (up-pending-objects c: mill < 5)
    (building-type-count-total mill < 1)
=>
    (build mill)
)
(defrule
    (population-headroom != 0)
    (up-pending-objects c: house < 2)
    (can-build house)
    (housing-headroom < 5)
=>
    (build house)
)
(defrule
    (can-build barracks)
    (up-pending-objects c: barracks < 5)
    (building-type-count-total barracks < 2)
=>
    (build barracks)
)
```
