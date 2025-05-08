# build farms
Builds farms according to how many food gatherers should exist.
## Usage
```
build farms
```
## Examples
```
build farms
```
```
(defrule
    (not
      (civ-selected 53)
    )
=>
    (up-get-fact unit-type-count villager 1)
    (up-modify-goal 1 s:* sn-food-gatherer-percentage)
    (up-modify-goal 1 c:/ 100)
    (up-get-fact building-type-count-total farm 2)
)
(defrule
    (not
      (civ-selected 53)
    )
    (up-compare-goal 2 g:< 1)
    (can-build farm)
=>
    (build farm)
)
(defrule
    (civ-selected 53)
=>
    (up-get-fact unit-type-count villager 1)
    (up-modify-goal 1 s:* sn-food-gatherer-percentage)
    (up-modify-goal 1 c:/ 200)
    (up-get-fact building-type-count-total 1889 2)
)
(defrule
    (civ-selected 53)
    (up-compare-goal 2 g:< 1)
    (can-build 1889)
=>
    (build 1889)
)

```
---
