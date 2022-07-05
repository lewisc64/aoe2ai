# stages
Blocks out rules having them advance into each other switching by a goal.
## Usage
```
#stages
    RULES
#advance when CONDITION
    RULES
#end stages
```
## Examples
```
#stages
    train archer-line
#advance when unit-type-count archer-line >= 10
    train skirmisher-line
#end stages
```
```
(defrule
    (true)
=>
    (set-goal 1 0)
    (disable-self)
)
(defrule
    (can-train archer-line)
    (goal 1 0)
=>
    (train archer-line)
)
(defrule
    (goal 1 0)
    (unit-type-count archer-line >= 10)
=>
    (set-goal 1 1)
)
(defrule
    (can-train skirmisher-line)
    (goal 1 1)
=>
    (train skirmisher-line)
)

```
---
