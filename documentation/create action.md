# create action
Creates a rule with the action contained within.
## Usage
```
@ACTION
```
## Examples
```
@up-get-fact military-population 0 1
```
```
(defrule
    (true)
=>
    (up-get-fact military-population 0 1)
)

```
---
