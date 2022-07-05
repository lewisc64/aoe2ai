# when
Rules in the 'then' block are allowed to trigger when any rule in the main 'when' block is triggered.
## Usage
```
#when
    RULE
#then ?always
    RULES
#end when
```
## Examples
```
#when
    build 1 house
#then
    chat to all "I built a house!"
#end when
```
```
(defrule
    (true)
=>
    (set-goal 1 0)
    (disable-self)
)
(defrule
    (can-build house)
    (up-pending-objects c: house < 5)
    (building-type-count-total house < 1)
=>
    (build house)
    (set-goal 1 1)
)
(defrule
    (goal 1 1)
=>
    (chat-to-all "I built a house!")
)
(defrule
    (true)
=>
    (set-goal 1 0)
)

```
---
