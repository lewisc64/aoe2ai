# target player
Sets sn-target-player-number and sn-focus-player-number.
## Usage
```
target winning/closest/attacking enemy/ally
```
## Examples
```
target closest enemy
```
```
(defrule
    (true)
=>
    (up-find-player enemy find-closest 1)
    (up-modify-sn sn-target-player-number g:= 1)
)

```
---
