# attack
Makes use of the attack-now action, with a cooldown of 60 seconds.
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
    (set-goal 1 0)
    (disable-self)
)
(defrule
    (true)
=>
    (up-get-fact game-time 0 2)
)
(defrule
    (up-compare-goal 2 g:>= 1)
=>
    (attack-now)
    (up-modify-goal 1 g:= 2)
    (up-modify-goal 1 c:+ 60)
)

```
---
```
attack with 30 units
```
```
(defrule
    (military-population < 30)
=>
    (up-jump-rule 3)
)
(defrule
    (true)
=>
    (set-goal 1 0)
    (disable-self)
)
(defrule
    (true)
=>
    (up-get-fact game-time 0 2)
)
(defrule
    (up-compare-goal 2 g:>= 1)
=>
    (attack-now)
    (up-modify-goal 1 g:= 2)
    (up-modify-goal 1 c:+ 60)
)

```
---
