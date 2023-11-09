# keep away
## Usage
```
keep UNIT TILES tiles away from enemy/ally/my OBJECTCLASS
```
## Examples
```
keep archer-line 8 tiles away from enemy town-center
```
```
(defrule
    (true)
=>
    (up-full-reset-search)
    (up-find-player enemy find-closest 1)
    (up-modify-sn sn-focus-player-number g:= 1)
    (up-find-remote c: town-center c: 255)
    (up-remove-objects search-remote object-data-target == siege-weapon-class)
    (up-get-search-state 41)
    (set-goal 1 0)
    (up-reset-filters)
    (up-filter-distance c: 1 c: 8)
)
(defrule
    (up-compare-goal 1 g:< 43)
=>
    (up-set-target-object search-remote g: 1)
    (up-get-point position-object 45)
    (up-set-target-point 45)
    (up-get-object-data 38 45)
    (up-get-object-data 39 46)
    (up-reset-search 1 1 0 0)
    (up-find-local c: archer-line c: 255)
    (up-set-target-object search-local c: 0)
    (up-get-object-data 38 47)
    (up-get-object-data 39 48)
    (up-lerp-tiles 47 45 c: -500)
    (set-strategic-number sn-target-point-adjustment 6)
    (up-target-point 47 action-default -1 -1)
    (set-strategic-number sn-target-point-adjustment 0)
    (up-modify-goal 1 c:+ 1)
    (up-jump-rule -1)
)

```
---
