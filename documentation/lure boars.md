# lure boars
## Usage
```
lure boars
```
## Examples
```
lure boars
```
```
(defrule
    (true)
=>
    (set-strategic-number sn-enable-boar-hunting 2)
    (set-strategic-number sn-minimum-number-hunters 1)
    (set-strategic-number sn-minimum-boar-lure-group-size 1)
    (set-strategic-number sn-minimum-boar-hunt-group-size 1)
    (set-strategic-number sn-maximum-hunt-drop-distance 48)
    (disable-self)
)
(defrule
    (dropsite-min-distance live-boar < 4)
    (strategic-number sn-minimum-number-hunters != 8)
=>
    (set-strategic-number sn-minimum-number-hunters 8)
    (up-drop-resources c: sheep-food 0)
)
(defrule
    (food-amount < 50)
    (up-pending-objects c: villager <= 1)
    (strategic-number sn-minimum-number-hunters == 8)
=>
    (up-drop-resources c: boar-food 10)
)
(defrule
    (strategic-number sn-minimum-number-hunters == 8)
    (dropsite-min-distance live-boar > 4)
    (or
      (dropsite-min-distance boar-food > 4)
      (dropsite-min-distance boar-food == -1)
    )
=>
    (set-strategic-number sn-minimum-number-hunters 1)
    (up-retask-gatherers food c: 255)
)

```
---
