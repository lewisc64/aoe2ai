# build
Sets up the rule to build the building. Can only build 5 buildings at a time to prevent accidental build queue flooding.
## Usage
```
build ?forward AMOUNT BUILDING_NAME with RESOURCE_NAME escrow
```
## Examples
```
build 1 barracks
```
```
(defrule
    (can-build barracks)
    (up-pending-objects c: barracks < 5)
    (building-type-count-total barracks < 1)
=>
    (build barracks)
)

```
---
```
build forward castle
```
```
(defrule
    (can-build castle)
    (up-pending-objects c: castle < 5)
=>
    (build-forward castle)
)

```
---
```
build archery-range with wood escrow
```
```
(defrule
    (can-build-with-escrow archery-range)
    (up-pending-objects c: archery-range < 5)
=>
    (release-escrow wood)
    (build archery-range)
)

```
---
