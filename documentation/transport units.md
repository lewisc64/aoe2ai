# transport units
Garrisons units into transport ships and sends them to the target player.
## Usage
```
transport units
```
## Examples
```
transport units
```
```
(defrule
    (soldier-count >= 1)
    (unit-type-count transport-ship >= 1)
=>
    (up-full-reset-search)
    (up-modify-sn sn-focus-player-number s:= sn-target-player-number)
    (up-filter-include -1 -1 -1 0)
    (up-find-remote c: town-center c: 1)
    (up-find-remote c: building-class c: 1)
)
(defrule
    (soldier-count >= 1)
    (unit-type-count transport-ship >= 1)
    (up-set-target-object search-remote c: 0)
=>
    (up-get-object-data 47 1)
    (up-full-reset-search)
    (up-get-fact player-number 0 2)
    (up-modify-sn sn-focus-player-number g:= 2)
    (fe-filter-garrisoned c: 0)
    (up-find-local c: archery-class c: 255)
    (up-find-local c: infantry-class c: 255)
    (up-find-local c: cavalry-class c: 255)
    (up-find-local c: siege-weapon-class c: 255)
    (up-find-local c: monastery-class c: 255)
    (up-find-local c: petard-class c: 255)
    (up-find-local c: archery-cannon-class c: 255)
    (up-find-local c: 947 c: 255)
    (up-find-local c: packed-trebuchet-class c: 255)
    (up-find-local c: scorpion-class c: 255)
    (up-remove-objects search-local 47 g:== 1)
    (up-set-target-object search-local c: 0)
    (up-get-object-data 47 3)
    (up-remove-objects search-local 47 g:!= 3)
    (up-get-point position-object 41)
    (up-set-target-point 41)
    (up-reset-filters)
    (up-find-remote c: 920 c: 40)
    (up-remove-objects search-remote 18 c:>= 20)
    (up-clean-search search-remote 44 1)
    (up-set-target-object search-remote c: 0)
    (up-target-objects 1 action-garrison -1 -1)
)
(defrule
    (soldier-count >= 1)
    (unit-type-count transport-ship >= 1)
=>
    (up-full-reset-search)
    (up-modify-sn sn-focus-player-number s:= sn-target-player-number)
    (up-find-local c: 920 c: 40)
    (up-remove-objects search-local 18 c:< 5)
    (up-get-point position-target 41)
    (up-target-point 41 action-unload -1 -1)
)

```
---
