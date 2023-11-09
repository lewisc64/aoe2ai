# micro mule carts
Moves mule carts to sensible locations.
## Usage
```
micro mule carts
```
## Examples
```
micro mule carts
```
```
(defrule
    (unit-type-count mule-cart == 0)
=>
    (up-jump-rule 7)
)
(defrule
    (true)
=>
    (set-strategic-number sn-defer-dropsite-update 0)
    (up-full-reset-search)
    (up-get-fact player-number 0 1)
    (up-modify-sn sn-focus-player-number g:= 1)
    (up-find-remote c: mule-cart c: 255)
    (up-get-search-state 41)
)
(defrule
    (true)
=>
    (set-goal 2 0)
    (up-reset-filters)
    (up-filter-distance c: 1 c: 5)
)
(defrule
    (up-compare-goal 2 g:< 43)
=>
    (up-set-target-object search-remote g: 2)
    (up-get-point position-object 45)
    (up-set-target-point 45)
    (up-get-object-data 38 45)
    (up-get-object-data 39 46)
    (up-reset-search 1 1 0 0)
    (up-find-local c: mule-cart c: 255)
    (up-set-target-object search-local c: 0)
    (up-get-object-data 38 47)
    (up-get-object-data 39 48)
    (up-lerp-tiles 47 45 c: -500)
    (set-strategic-number sn-target-point-adjustment 6)
    (up-target-point 47 action-default -1 -1)
    (set-strategic-number sn-target-point-adjustment 0)
    (up-modify-goal 2 c:+ 1)
    (up-jump-rule -1)
)
(defrule
    (true)
=>
    (up-full-reset-search)
    (up-modify-sn sn-focus-player-number g:= 1)
    (up-filter-exclude -1 -1 orderid-move -1)
    (up-find-local c: mule-cart c: 255)
    (up-set-target-object search-local c: 0)
    (up-reset-filters)
    (up-find-remote c: 123 c: 255)
    (up-find-remote c: 218 c: 255)
    (up-clean-search search-remote 44 1)
    (up-target-objects 0 action-move -1 -1)
)
(defrule
    (dropsite-min-distance wood > 5)
    (strategic-number sn-wood-gatherer-percentage > 0)
=>
    (up-full-reset-search)
    (up-find-local c: town-center c: 1)
    (up-set-target-object search-local c: 0)
    (up-full-reset-search)
    (up-modify-sn sn-focus-player-number c:= 0)
    (up-filter-status c: 3 c: 0)
    (up-find-resource c: wood c: 1)
    (up-clean-search search-remote 44 1)
    (up-find-local c: mule-cart c: 255)
    (up-set-target-object search-remote c: 0)
    (up-clean-search search-local 44 1)
    (up-remove-objects search-local -1 >= 1)
    (up-target-objects 0 action-move -1 -1)
)
(defrule
    (dropsite-min-distance gold > 5)
    (strategic-number sn-gold-gatherer-percentage > 0)
=>
    (up-full-reset-search)
    (up-find-local c: town-center c: 1)
    (up-set-target-object search-local c: 0)
    (up-full-reset-search)
    (up-modify-sn sn-focus-player-number c:= 0)
    (up-filter-status c: 3 c: 0)
    (up-find-resource c: gold c: 1)
    (up-clean-search search-remote 44 1)
    (up-find-local c: mule-cart c: 255)
    (up-set-target-object search-remote c: 0)
    (up-clean-search search-local 44 1)
    (up-remove-objects search-local -1 >= 1)
    (up-target-objects 0 action-move -1 -1)
)
(defrule
    (dropsite-min-distance stone > 5)
    (strategic-number sn-stone-gatherer-percentage > 0)
=>
    (up-full-reset-search)
    (up-find-local c: town-center c: 1)
    (up-set-target-object search-local c: 0)
    (up-full-reset-search)
    (up-modify-sn sn-focus-player-number c:= 0)
    (up-filter-status c: 3 c: 0)
    (up-find-resource c: stone c: 1)
    (up-clean-search search-remote 44 1)
    (up-find-local c: mule-cart c: 255)
    (up-set-target-object search-remote c: 0)
    (up-clean-search search-local 44 1)
    (up-remove-objects search-local -1 >= 1)
    (up-target-objects 0 action-move -1 -1)
)

```
---
