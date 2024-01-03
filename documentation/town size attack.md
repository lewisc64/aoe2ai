# town size attack
Performs a town size attack (TSA) by inflating the town size until enemy buildings are within it.
## Usage
```
town size attack
```
## Examples
```
town size attack
```
```
(defrule
    (or
      (up-building-type-in-town c: town-center >= 1)
      (up-building-type-in-town c: castle >= 1)
    )
=>
    (up-modify-sn sn-maximum-town-size c:- 1)
)
(defrule
    (nor
      (up-building-type-in-town c: town-center >= 1)
      (up-building-type-in-town c: castle >= 1)
    )
=>
    (up-modify-sn sn-maximum-town-size c:+ 5)
)
(defrule
    (true)
=>
    (up-modify-sn sn-maximum-town-size c:max 50)
    (up-modify-sn sn-maximum-town-size c:min 30000)
)

```
---
