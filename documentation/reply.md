# reply
Body is allowed to trigger when the taunt is detected from the specified player. Taunt is acknowledged regardless of whether the body successfully triggers.
## Usage
```
#reply to ENEMY/ALLY taunt TAUNT_NUMBER
   RULES
#end reply
```
## Examples
```
#reply to ally taunt 31
    attack
#end reply
```
```
(defrule
    (taunt-detected any-ally 31)
=>
    (set-goal 1 0)
    (disable-self)
)
(defrule
    (taunt-detected any-ally 31)
=>
    (up-get-fact game-time 0 2)
)
(defrule
    (up-compare-goal 2 g:>= 1)
    (taunt-detected any-ally 31)
=>
    (attack-now)
    (up-modify-goal 1 g:= 2)
    (up-modify-goal 1 c:+ 60)
)
(defrule
    (taunt-detected any-ally 31)
=>
    (acknowledge-taunt this-any-ally 31)
)

```
---
