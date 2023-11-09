# build mule carts
Builds mule carts. Creates 1 mule cart for every 10 villagers.
## Usage
```
build mule carts
```
## Examples
```
build mule carts
```
```
(defrule
    (true)
=>
    (up-get-fact unit-type-count villager 1)
    (up-modify-goal 1 c:/ 10)
    (up-modify-goal 1 c:+ 1)
    (up-get-fact unit-type-count-total mule-cart 2)
)
(defrule
    (up-compare-goal 2 g:< 1)
    (can-build mule-cart)
    (up-pending-objects c: mule-cart == 0)
=>
    (build mule-cart)
)

```
---
