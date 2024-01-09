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
    (set-goal 4 0)
    (up-get-object-type-data c: archer-line 56 3)
)
(defrule
    (unit-available archer-line)
    (up-object-type-count g: 3 c:>= 1)
=>
    (up-get-fact unit-type-count-total archer-line 2)
    (up-modify-goal 1 g:+ 2)
    (up-modify-goal 4 c:+ 1)
)
(defrule
    (true)
=>
    (up-get-object-type-data c: militiaman-line 56 3)
)
(defrule
    (unit-available militiaman-line)
    (up-object-type-count g: 3 c:>= 1)
=>
    (up-get-fact unit-type-count-total militiaman-line 2)
    (up-modify-goal 1 g:+ 2)
    (up-modify-goal 4 c:+ 1)
)
(defrule
    (true)
=>
    (up-get-fact unit-type-count-total archer-line 2)
    (up-modify-goal 5 g:= 1)
    (up-modify-goal 5 c:* 1)
    (up-modify-goal 5 g:/ 4)
)
(defrule
    (up-compare-goal 2 g:<= 5)
    (can-train archer-line)
=>
    (train archer-line)
)
(defrule
    (true)
=>
    (up-get-fact unit-type-count-total militiaman-line 2)
    (up-modify-goal 5 g:= 1)
    (up-modify-goal 5 c:* 1)
    (up-modify-goal 5 g:/ 4)
)
(defrule
    (up-compare-goal 2 g:<= 5)
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
    (set-goal 4 0)
    (up-get-object-type-data c: scout-cavalry-line 56 3)
)
(defrule
    (unit-available scout-cavalry-line)
    (up-object-type-count g: 3 c:>= 1)
=>
    (up-get-fact unit-type-count-total scout-cavalry-line 2)
    (up-modify-goal 1 g:+ 2)
    (up-modify-goal 4 c:+ 10)
)
(defrule
    (true)
=>
    (up-get-object-type-data c: spearman-line 56 3)
)
(defrule
    (unit-available spearman-line)
    (up-object-type-count g: 3 c:>= 1)
=>
    (up-get-fact unit-type-count-total spearman-line 2)
    (up-modify-goal 1 g:+ 2)
    (up-modify-goal 4 c:+ 3)
)
(defrule
    (true)
=>
    (up-get-fact unit-type-count-total scout-cavalry-line 2)
    (up-modify-goal 5 g:= 1)
    (up-modify-goal 5 c:* 10)
    (up-modify-goal 5 g:/ 4)
)
(defrule
    (up-compare-goal 2 g:<= 5)
    (can-train scout-cavalry-line)
=>
    (train scout-cavalry-line)
)
(defrule
    (true)
=>
    (up-get-fact unit-type-count-total spearman-line 2)
    (up-modify-goal 5 g:= 1)
    (up-modify-goal 5 c:* 3)
    (up-modify-goal 5 g:/ 4)
)
(defrule
    (up-compare-goal 2 g:<= 5)
    (can-train spearman-line)
=>
    (train spearman-line)
)

```
---
