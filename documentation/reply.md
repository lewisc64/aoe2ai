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
    (attack-now)
    (acknowledge-taunt this-any-ally 31)
)

```
---
