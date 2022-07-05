# order
Executes statements in order once every rule pass. Loops back to the beginning upon reaching the end.
## Examples
```
train archer-line => train skirmisher-line
```
```
(defrule
    (true)
=>
    (set-goal 1 0)
    (disable-self)
)
(defrule
    (can-train skirmisher-line)
    (goal 1 1)
=>
    (train skirmisher-line)
    (set-goal 1 0)
)
(defrule
    (can-train archer-line)
    (goal 1 0)
=>
    (train archer-line)
    (set-goal 1 1)
)

```
---
