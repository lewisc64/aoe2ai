# keep away from building
## Usage
```
keep UNIT TILES tiles away from enemy/ally/my BUILDING
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
    (up-find-player enemy find-closest 5)
    (up-modify-sn sn-focus-player-number g:= 5)
    (up-find-remote c: town-center c: 255)
    (up-get-search-state 1)
    (set-goal 5 0)
    (up-filter-distance c: -1 c: 8)
    (up-filter-exclude -1 -1 orderid-move -1)
)
(defrule
    (up-compare-goal 5 g:< 3)
=>
    (up-set-target-object search-remote g: 5)
    (up-get-point position-object 41)
    (up-set-target-point 41)
    (up-get-object-data 38 41)
    (up-get-object-data 39 42)
    (up-reset-search 1 1 0 0)
    (up-find-local c: archer-line c: 255)
    (up-set-target-object search-local c: 0)
    (up-get-object-data 38 43)
    (up-get-object-data 39 44)
    (up-lerp-tiles 43 41 c: -500)
    (set-strategic-number sn-target-point-adjustment 6)
    (up-target-point 43 action-default -1 -1)
    (set-strategic-number sn-target-point-adjustment 0)
    (up-modify-goal 5 c:+ 1)
    (up-jump-rule -1)
)

```
---
