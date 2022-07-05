# distribute villagers to
Modifies the gatherer percentages. Precomputes, so no constants or facts can be used, just numbers.
## Usage
```
distribute PERCENTAGE villagers from RESOURCE_NAME to RESOURCE_NAME
```
## Examples
```
distribute 5 villagers from wood to stone
```
```
(defrule
    (strategic-number sn-wood-gatherer-percentage >= 5)
    (strategic-number sn-stone-gatherer-percentage <= 95)
=>
    (up-modify-sn sn-wood-gatherer-percentage c:- 5)
    (up-modify-sn sn-stone-gatherer-percentage c:+ 5)
)

```
---
```
distribute 10 villagers from wood and food to gold
```
```
(defrule
    (strategic-number sn-wood-gatherer-percentage >= 5)
    (strategic-number sn-food-gatherer-percentage >= 5)
    (strategic-number sn-gold-gatherer-percentage <= 90)
=>
    (up-modify-sn sn-wood-gatherer-percentage c:- 5)
    (up-modify-sn sn-food-gatherer-percentage c:- 5)
    (up-modify-sn sn-gold-gatherer-percentage c:+ 10)
)

```
---
