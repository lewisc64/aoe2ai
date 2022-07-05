# auto expand town size
Automatically expands town size. Must be below any builds that it needs to affect. Any build rules below this will be placed at the given maximum.

Affects the following sn's:
 - sn-maximum-town-size
 - sn-minimum-town-size
 - sn-safe-town-size
 - sn-maximum-food-drop-distance

This will disturb any town size attacks because it takes absolute control over the town size. Disable this during TSA.
## Usage
```
auto expand town size
auto expand town size to RADIUS
```
## Examples
```
auto expand town size
```
```
(defrule
    (true)
=>
    (set-strategic-number sn-maximum-town-size 30)
    (set-strategic-number sn-minimum-town-size 8)
    (set-strategic-number sn-safe-town-size 30)
    (set-strategic-number sn-maximum-food-drop-distance 30)
    (set-goal 2 0)
    (disable-self)
)
(defrule
    (true)
=>
    (up-modify-sn sn-maximum-town-size c:+ 4)
    (up-modify-sn sn-maximum-town-size c:min 30)
    (set-strategic-number sn-minimum-town-size 8)
    (set-strategic-number sn-safe-town-size 30)
    (set-strategic-number sn-maximum-food-drop-distance 30)
)
(defrule
    (goal 1 0)
=>
    (set-goal 2 0)
)
(defrule
    (goal 1 1)
    (goal 2 0)
=>
    (up-modify-sn sn-maximum-town-size s:= sn-minimum-town-size)
    (set-goal 2 1)
)
(defrule
    (true)
=>
    (set-goal 1 0)
)

```
---
```
auto expand town size to 50
```
```
(defrule
    (true)
=>
    (set-strategic-number sn-maximum-town-size 50)
    (set-strategic-number sn-minimum-town-size 8)
    (set-strategic-number sn-safe-town-size 50)
    (set-strategic-number sn-maximum-food-drop-distance 50)
    (set-goal 2 0)
    (disable-self)
)
(defrule
    (true)
=>
    (up-modify-sn sn-maximum-town-size c:+ 4)
    (up-modify-sn sn-maximum-town-size c:min 50)
    (set-strategic-number sn-minimum-town-size 8)
    (set-strategic-number sn-safe-town-size 50)
    (set-strategic-number sn-maximum-food-drop-distance 50)
)
(defrule
    (goal 1 0)
=>
    (set-goal 2 0)
)
(defrule
    (goal 1 1)
    (goal 2 0)
=>
    (up-modify-sn sn-maximum-town-size s:= sn-minimum-town-size)
    (set-goal 2 1)
)
(defrule
    (true)
=>
    (set-goal 1 0)
)

```
---
