# do basic targeting
Targets the closest enemy, does not retarget until they lose or become allied.
## Examples
```
do basic targeting
```
```
(defrule
    (or
      (strategic-number sn-target-player-number <= 0)
      (or
        (not
          (player-in-game target-player)
        )
        (not
          (stance-toward target-player enemy)
        )
      )
    )
=>
    (up-find-player enemy find-closest 1)
    (up-modify-sn sn-target-player-number g:= 1)
)

```
---
