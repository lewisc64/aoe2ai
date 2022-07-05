# add condition
Adds a condition to the condition stack. 'If' is preferred.
## Usage
```
#add condition CONDITION
    RULES
#remove condition
```
## Examples
```
#add condition current-age == dark-age
    rule
#remove condition
```
```
(defrule
    (current-age == dark-age)
=>
    (do-nothing)
)

```
---
