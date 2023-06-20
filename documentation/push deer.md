# push deer
## Usage
```
push deer with UNIT_LIST within 30 tiles
```
## Examples
```
push deer
```
```
(defrule
    (dropsite-min-distance deer-hunting != 255)
    (dropsite-min-distance deer-hunting != -1)
=>
    (up-full-reset-search)
    (up-get-point position-self 41)
    (up-set-target-point 41)
    (up-modify-goal 41 c:* 100)
    (up-modify-goal 42 c:* 100)
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
    (up-get-object-data 38 43)
    (up-get-object-data 39 44)
    (up-lerp-tiles 43 41 c: -70)
    (up-reset-filters)
    (up-find-local c: scout-cavalry-line c: 1)
    (up-find-local c: eagle-warrior-line c: 1)
    (up-find-local c: camel-line c: 1)
    (set-strategic-number sn-target-point-adjustment 6)
    (up-target-point 43 action-default -1 -1)
    (set-strategic-number sn-target-point-adjustment 0)
)
(defrule
    (dropsite-min-distance deer-hunting <= 4)
    (dropsite-min-distance deer-hunting != -1)
=>
    (up-full-reset-search)
    (up-find-local c: town-center c: 1)
    (up-set-target-object search-local c: 0)
    (up-get-point position-object 41)
    (up-reset-search 1 1 0 0)
    (up-filter-distance c: -1 c: 4)
    (up-find-local c: villager-class c: 4)
    (up-find-remote c: 909 c: 100)
    (up-get-search-state 1)
)
(defrule
    (up-compare-goal 1 >= 1)
    (up-compare-goal 3 >= 1)
=>
    (up-target-objects 0 action-default -1 -1)
    (up-send-scout group-type-land-explore scout-border)
)

```
---
```
push deer with archer-line within 100 tiles
```
```
(defrule
    (dropsite-min-distance deer-hunting != 255)
    (dropsite-min-distance deer-hunting != -1)
=>
    (up-full-reset-search)
    (up-get-point position-self 41)
    (up-set-target-point 41)
    (up-modify-goal 41 c:* 100)
    (up-modify-goal 42 c:* 100)
    (set-strategic-number sn-focus-player-number 0)
    (up-filter-distance c: -1 c: 100)
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
    (up-get-object-data 38 43)
    (up-get-object-data 39 44)
    (up-lerp-tiles 43 41 c: -70)
    (up-reset-filters)
    (up-find-local c: archer-line c: 1)
    (set-strategic-number sn-target-point-adjustment 6)
    (up-target-point 43 action-default -1 -1)
    (set-strategic-number sn-target-point-adjustment 0)
)
(defrule
    (dropsite-min-distance deer-hunting <= 4)
    (dropsite-min-distance deer-hunting != -1)
=>
    (up-full-reset-search)
    (up-find-local c: town-center c: 1)
    (up-set-target-object search-local c: 0)
    (up-get-point position-object 41)
    (up-reset-search 1 1 0 0)
    (up-filter-distance c: -1 c: 4)
    (up-find-local c: villager-class c: 4)
    (up-find-remote c: 909 c: 100)
    (up-get-search-state 1)
)
(defrule
    (up-compare-goal 1 >= 1)
    (up-compare-goal 3 >= 1)
=>
    (up-target-objects 0 action-default -1 -1)
    (up-send-scout group-type-land-explore scout-border)
)

```
---
