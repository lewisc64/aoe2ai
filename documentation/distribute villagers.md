# distribute villagers
Percentages must add up to 100. Makes use of the sn-TYPE-gatherer-percentage strategic number.
## Usage
```
distribute villagers WOOD_PERCENT FOOD_PERCENT GOLD_PERCENT STONE_PERCENT
```
## Examples
```
distribute villagers 40 30 20 10
```
```
(defrule
    (true)
=>
    (set-strategic-number sn-wood-gatherer-percentage 40)
    (set-strategic-number sn-food-gatherer-percentage 30)
    (set-strategic-number sn-gold-gatherer-percentage 20)
    (set-strategic-number sn-stone-gatherer-percentage 10)
)

```
---
