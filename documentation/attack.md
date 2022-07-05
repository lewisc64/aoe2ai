# attack
Makes use of the attack-now action.
## Usage
```
attack with AMOUNT units
```
## Examples
```
attack
```
```
(defrule
    (true)
=>
    (attack-now)
)

```
---
```
attack with 30 units
```
```
(defrule
    (military-population >= 30)
=>
    (attack-now)
)

```
---
