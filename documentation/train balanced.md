# train balanced
Trains multiple types of units so they are in the specified ratios on the field.
## Usage
```
train balanced WEIGHT UNIT WEIGHT UNIT [...]
```
## Examples
```
train balanced 1 archer-line 1 militiaman-line
```
```
(defrule
    (true)
=>
    (set-goal 1 0)
    (up-get-fact unit-type-count-total archer-line 2)
    (up-modify-goal 1 g:+ 2)
    (up-get-fact unit-type-count-total militiaman-line 2)
    (up-modify-goal 1 g:+ 2)
    (up-get-fact unit-type-count-total archer-line 2)
    (up-modify-goal 3 g:= 1)
    (up-modify-goal 3 c:* 1)
    (up-modify-goal 3 c:/ 2)
)
(defrule
    (up-compare-goal 2 g:<= 3)
    (can-train archer-line)
=>
    (train archer-line)
)
(defrule
    (true)
=>
    (up-get-fact unit-type-count-total militiaman-line 2)
    (up-modify-goal 3 g:= 1)
    (up-modify-goal 3 c:* 1)
    (up-modify-goal 3 c:/ 2)
)
(defrule
    (up-compare-goal 2 g:<= 3)
    (can-train militiaman-line)
=>
    (train militiaman-line)
)

```
---
```
train balanced 10 scout-cavalry-line 3 spearman-line
```
```
(defrule
    (true)
=>
    (set-goal 1 0)
    (up-get-fact unit-type-count-total scout-cavalry-line 2)
    (up-modify-goal 1 g:+ 2)
    (up-get-fact unit-type-count-total spearman-line 2)
    (up-modify-goal 1 g:+ 2)
    (up-get-fact unit-type-count-total scout-cavalry-line 2)
    (up-modify-goal 3 g:= 1)
    (up-modify-goal 3 c:* 10)
    (up-modify-goal 3 c:/ 13)
)
(defrule
    (up-compare-goal 2 g:<= 3)
    (can-train scout-cavalry-line)
=>
    (train scout-cavalry-line)
)
(defrule
    (true)
=>
    (up-get-fact unit-type-count-total spearman-line 2)
    (up-modify-goal 3 g:= 1)
    (up-modify-goal 3 c:* 3)
    (up-modify-goal 3 c:/ 13)
)
(defrule
    (up-compare-goal 2 g:<= 3)
    (can-train spearman-line)
=>
    (train spearman-line)
)

```
---
