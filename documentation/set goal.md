# set goal
Sets a goal, and sets up the constant if it does not already exist.
## Usage
```
goal GOAL_NAME = VALUE
```
## Examples
```
goal test = 1
```
```
(defconst test 1)
(defrule
    (true)
=>
    (set-goal test 1)
)

```
---
```
goal test += 1
```
```
(defrule
    (true)
=>
    (up-modify-goal test c:+ 1)
)

```
---
```
goal test *= 5
```
```
(defrule
    (true)
=>
    (up-modify-goal test c:* 5)
)

```
---
