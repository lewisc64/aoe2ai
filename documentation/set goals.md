# set goals
Sets multiple goals, constants, and ensures they are consecutive.
## Usage
```
goals GOAL1, GOAL2, GOAL3 = 0
```
## Examples
```
goals one, two, three = 0
```
```
(defconst three 3)
(defconst two 2)
(defconst one 1)
(defrule
    (true)
=>
    (set-goal one 0)
    (set-goal two 0)
    (set-goal three 0)
)

```
---
