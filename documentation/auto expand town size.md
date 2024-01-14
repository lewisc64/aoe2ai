# auto expand town size
Automatically expands town size in order to place buildings. Must be below any builds that it needs to affect. Any build rules below this will be placed at the given maximum.

Affects the following sn's:
 - sn-maximum-town-size
 - sn-minimum-town-size
 - sn-safe-town-size

This will disturb any town size attacks because it takes absolute control over the town size. Disable this during TSA.
## Usage
```
auto expand town size
auto expand town size to RADIUS
```
## Examples
```
build barracks; auto expand town size
```
```
(defrule
    (can-build barracks)
    (up-pending-objects c: barracks < 5)
=>
    (build barracks)
)
(defrule
    (true)
=>
    (set-goal 1 0)
    (set-strategic-number sn-maximum-town-size 8)
    (set-strategic-number sn-minimum-town-size 8)
    (set-strategic-number sn-safe-town-size 8)
    (disable-self)
)
(defrule
    (up-pending-placement c: barracks)
    (goal 1 0)
=>
    (set-strategic-number sn-maximum-town-size 8)
    (set-strategic-number sn-safe-town-size 8)
    (set-goal 1 1)
)
(defrule
    (not
      (up-pending-placement c: barracks)
    )
    (goal 1 1)
=>
    (set-goal 1 0)
)
(defrule
    (true)
=>
    (up-get-fact unit-type-count villager 3)
    (up-modify-goal 3 s:min sn-cap-civilian-builders)
    (up-get-fact unit-type-count villager-builder 2)
)
(defrule
    (up-pending-placement c: barracks)
    (up-compare-goal 2 g:< 3)
=>
    (up-modify-sn sn-maximum-town-size c:+ 4)
    (up-modify-sn sn-safe-town-size s:= sn-maximum-town-size)
)
(defrule
    (not
      (up-pending-placement c: barracks)
    )
=>
    (up-modify-sn sn-maximum-town-size c:= 30)
    (up-modify-sn sn-safe-town-size s:= sn-maximum-town-size)
)

```
---
```
build barracks; auto expand town size to 50
```
```
(defrule
    (can-build barracks)
    (up-pending-objects c: barracks < 5)
=>
    (build barracks)
)
(defrule
    (true)
=>
    (set-goal 1 0)
    (set-strategic-number sn-maximum-town-size 8)
    (set-strategic-number sn-minimum-town-size 8)
    (set-strategic-number sn-safe-town-size 8)
    (disable-self)
)
(defrule
    (up-pending-placement c: barracks)
    (goal 1 0)
=>
    (set-strategic-number sn-maximum-town-size 8)
    (set-strategic-number sn-safe-town-size 8)
    (set-goal 1 1)
)
(defrule
    (not
      (up-pending-placement c: barracks)
    )
    (goal 1 1)
=>
    (set-goal 1 0)
)
(defrule
    (true)
=>
    (up-get-fact unit-type-count villager 3)
    (up-modify-goal 3 s:min sn-cap-civilian-builders)
    (up-get-fact unit-type-count villager-builder 2)
)
(defrule
    (up-pending-placement c: barracks)
    (up-compare-goal 2 g:< 3)
=>
    (up-modify-sn sn-maximum-town-size c:+ 4)
    (up-modify-sn sn-safe-town-size s:= sn-maximum-town-size)
)
(defrule
    (not
      (up-pending-placement c: barracks)
    )
=>
    (up-modify-sn sn-maximum-town-size c:= 50)
    (up-modify-sn sn-safe-town-size s:= sn-maximum-town-size)
)

```
---
