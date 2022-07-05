# block respond
When the AI sees the specified amount, the body is allowed to trigger. If building/unit is unspecified, unit is assumed. If amount is unspecified, 1 is assumed.
## Usage
```
#respond to ?AMOUNT NAME ?BUILDING/UNIT
   RULES
#end respond
```
## Examples
```
#respond to 3 scout-cavalry-line
    train 4 spearman-line
#end respond
```
```
(defrule
    (can-train spearman-line)
    (unit-type-count-total spearman-line < 4)
    (players-unit-type-count any-enemy scout-cavalry-line >= 3)
=>
    (train spearman-line)
)

```
---
