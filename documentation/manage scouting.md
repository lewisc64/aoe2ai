# manage scouting
Scouts using one soldier. Will scout with a villager for 10 minutes if none are available.
## Usage
```
manage scouting
```
## Examples
```
manage scouting
```
```
(defrule
    (true)
=>
    (set-strategic-number sn-percent-civilian-explorers 0)
    (set-strategic-number sn-cap-civilian-explorers 0)
    (set-strategic-number sn-total-number-explorers 1)
    (set-strategic-number sn-number-explore-groups 1)
    (set-strategic-number sn-number-boat-explore-groups 1)
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

```
---
