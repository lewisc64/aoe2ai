# lure boars
Lures boars using one villager to the town center.
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
    (dropsite-min-distance live-boar != -1)
=>
    (set-strategic-number sn-enable-boar-hunting 2)
    (set-strategic-number sn-minimum-number-hunters 1)
    (set-strategic-number sn-minimum-boar-lure-group-size 1)
    (set-strategic-number sn-minimum-boar-hunt-group-size 1)
    (set-strategic-number sn-maximum-hunt-drop-distance 48)
)
(defrule
    (dropsite-min-distance live-boar == -1)
=>
    (set-strategic-number sn-enable-boar-hunting 1)
    (set-strategic-number sn-minimum-number-hunters 0)
    (set-strategic-number sn-minimum-boar-lure-group-size 0)
    (set-strategic-number sn-minimum-boar-hunt-group-size 0)
    (set-strategic-number sn-maximum-hunt-drop-distance 8)
)
(defrule
    (dropsite-min-distance live-boar < 4)
    (dropsite-min-distance live-boar >= 0)
=>
    (up-request-hunters c: 8)
)
(defrule
    (food-amount < 50)
    (up-pending-objects c: villager <= 1)
=>
    (up-drop-resources c: boar-food 10)
)

```
---
