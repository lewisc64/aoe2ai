# add action
Adds an action to the action stack.
## Usage
```
#add action ACTION
    RULES
#remove action
```
## Examples
```
#add action chat-to-all "hello"
    rule
#remove action
```
```
(defrule
    (true)
=>
    (chat-to-all "hello")
)

```
---
