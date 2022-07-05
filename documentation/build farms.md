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
    (true)
=>
    (up-get-fact unit-type-count villager 1)
    (up-modify-goal 1 s:* sn-food-gatherer-percentage)
    (up-modify-goal 1 c:/ 100)
    (up-get-fact building-type-count-total farm 2)
)
(defrule
    (up-compare-goal 2 g:< 1)
    (can-build farm)
=>
    (build farm)
)

```
---
