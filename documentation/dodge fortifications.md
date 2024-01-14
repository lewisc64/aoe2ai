# dodge fortifications
## Usage
```
dodge fortifications
```
## Examples
```
dodge fortifications
```
```
(defrule
    (true)
=>
    (set-strategic-number sn-focus-player-number 1)
    (up-full-reset-search)
)
(defrule
    (players-stance focus-player enemy)
=>
    (up-find-remote c: town-center c: 255)
)
(defrule
    (strategic-number sn-focus-player-number < 8)
=>
    (up-modify-sn sn-focus-player-number c:+ 1)
    (up-jump-rule -2)
)
(defrule
    (true)
=>
    (up-remove-objects search-remote object-data-hitpoints < 400)
    (set-strategic-number sn-focus-player-number 1)
)
(defrule
    (players-stance focus-player enemy)
=>
    (up-find-remote c: castle c: 255)
    (up-find-remote c: donjon c: 255)
    (up-find-remote c: krepost c: 255)
    (up-find-remote c: tower-class c: 255)
)
(defrule
    (strategic-number sn-focus-player-number < 8)
=>
    (up-modify-sn sn-focus-player-number c:+ 1)
    (up-jump-rule -2)
)
(defrule
    (true)
=>
    (up-remove-objects search-remote object-data-target == siege-weapon-class)
    (up-get-search-state 1)
    (set-goal 5 0)
)
(defrule
    (up-compare-goal 5 g:< 3)
=>
    (up-set-target-object search-remote g: 5)
    (up-get-point position-object 41)
    (up-set-target-point 41)
    (up-get-object-data 38 41)
    (up-get-object-data 39 42)
    (up-reset-filters)
    (up-get-object-data object-data-range 6)
    (up-modify-goal 6 c:+ 8)
    (up-filter-distance c: -1 g: 6)
    (up-filter-exclude -1 -1 orderid-move -1)
    (up-filter-include cmdid-military -1 -1 -1)
    (up-filter-exclude -1 -1 -1 siege-weapon-class)
    (up-reset-search 1 1 0 0)
    (up-find-local c: all-units-class c: 255)
    (up-remove-objects search-local 27 >= 100)
    (up-set-target-object search-local c: 0)
    (up-get-object-data 38 43)
    (up-get-object-data 39 44)
    (up-lerp-tiles 43 41 c: -200)
    (up-cross-tiles 43 41 c: 500)
    (set-strategic-number sn-target-point-adjustment 6)
    (up-target-point 43 action-default -1 -1)
    (set-strategic-number sn-target-point-adjustment 0)
    (up-modify-goal 5 c:+ 1)
    (up-jump-rule -1)
)

```
---
