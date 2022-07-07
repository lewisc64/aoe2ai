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

```
---
