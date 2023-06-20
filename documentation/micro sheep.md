# micro sheep
Takes sheep one at a time. Garrisons them in a mill for Gurjaras.
## Usage
```
micro sheep
```
## Examples
```
micro sheep
```
```
(defrule
    (civ-selected gurjaras)
=>
    (up-jump-rule 6)
)
(defrule
    (true)
=>
    (up-full-reset-search)
    (up-get-point position-self 45)
    (up-set-target-point 45)
    (up-modify-sn sn-focus-player-number c:= 0)
    (up-filter-distance c: -1 c: 10)
    (up-filter-status c: 5 c: 0)
    (up-find-status-remote c: livestock-class c: 1)
    (up-get-search-state 41)
)
(defrule
    (up-compare-goal 43 c:>= 1)
=>
    (up-reset-filters)
    (up-find-local c: 592 c: 255)
    (up-find-local c: 590 c: 255)
    (up-target-objects 0 action-default -1 -1)
)
(defrule
    (goal 43 0)
=>
    (up-full-reset-search)
    (up-get-fact player-number 0 1)
    (up-modify-sn sn-focus-player-number g:= 1)
    (up-find-remote c: livestock-class c: 1)
    (up-find-local c: 592 c: 255)
    (up-find-local c: 590 c: 255)
    (up-target-objects 0 action-default -1 -1)
)
(defrule
    (true)
=>
    (up-full-reset-search)
    (up-get-fact player-number 0 1)
    (up-modify-sn sn-focus-player-number g:= 1)
    (up-find-local c: livestock-class c: 255)
    (up-remove-objects search-local -1 == 0)
    (up-find-remote c: town-center c: 1)
    (up-set-target-object search-remote c: 0)
    (up-get-point 45 position-object)
    (up-get-point 47 position-center)
    (up-lerp-tiles 45 47 c: -5)
    (up-target-point 45 action-default -1 -1)
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
    (up-get-search-state 41)
)
(defrule
    (up-compare-goal 43 c:>= 1)
=>
    (up-full-reset-search)
    (up-get-fact player-number 0 1)
    (up-modify-sn sn-focus-player-number g:= 1)
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
    (up-get-point position-object 45)
    (up-set-target-point 45)
    (up-reset-search 1 1 0 0)
    (up-find-local c: 592 c: 255)
    (up-find-local c: 590 c: 255)
    (up-filter-distance c: -1 c: 30)
    (up-find-remote c: 907 c: 255)
    (up-target-objects 0 action-default -1 -1)
    (up-reset-search 1 1 1 1)
    (up-get-fact player-number 0 1)
    (up-modify-sn sn-focus-player-number g:= 1)
    (up-reset-filters)
    (up-find-local c: livestock-class c: 255)
    (up-find-remote c: mill c: 1)
    (up-target-objects 0 action-garrison -1 -1)
    (up-reset-search 1 1 0 0)
    (up-find-local c: mill c: 1)
    (up-target-objects 0 action-gather -1 -1)
)

```
---
