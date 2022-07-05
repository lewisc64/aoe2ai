# build walls
Wall placement must be enabled on the same perimeter to function.
## Usage
```
build stone/palisade walls/gates on perimeter PERIMETER_NUMBER
```
## Examples
```
build stone walls on perimeter 2
```
```
(defrule
    (can-build-wall 2 stone-wall-line)
=>
    (build-wall 2 stone-wall-line)
)

```
---
```
build stone gates on perimeter 2
```
```
(defrule
    (building-type-count-total stone-wall-line > 0)
    (can-build-gate 2)
    (building-type-count-total gate < 5)
=>
    (build-gate 2)
)

```
---
```
build palisade walls on perimeter 1
```
```
(defrule
    (can-build-wall 1 palisade-wall)
=>
    (build-wall 1 palisade-wall)
)

```
---
