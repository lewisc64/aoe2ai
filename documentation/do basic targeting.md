# do basic targeting
Targets the closest enemy, does not retarget until they lose or become allied.
## Examples
```
do basic targeting
```
```
(defrule
    (true)
=>
    (set-goal 2 0)
)
(defrule
    (or
      (strategic-number sn-target-player-number <= 0)
      (or
        (not
          (player-in-game target-player)
        )
        (or
          (not
            (stance-toward target-player enemy)
          )
          (players-population target-player == 0)
        )
      )
    )
=>
    (up-find-player enemy find-closest 1)
    (up-modify-sn sn-target-player-number g:= 1)
    (set-goal 2 1)
)
(defrule
    (goal 2 1)
    (players-military-population target-player == 0)
    (players-population target-player < 20)
=>
    (up-find-next-player enemy find-closest 1)
)
(defrule
    (goal 2 1)
    (players-military-population target-player == 0)
    (players-population target-player < 20)
    (up-compare-goal 1 c:>= 1)
=>
    (up-modify-sn sn-target-player-number g:= 1)
    (up-jump-rule -2)
)

```
---
