# set up basics
## Usage
```
set up basics
```
## Examples
```
set up basics
```
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
    (disable-self)
)

```
---
