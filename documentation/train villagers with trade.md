# train villagers with trade
Trains the specified amount of villagers, with a portion of that being trade carts if it is a team game.
## Usage
```
train NUMBER villagers with NUMBER trade
```
## Examples
```
train 120 villagers with 30 trade
```
```
(defrule
    (true)
=>
    (up-get-fact unit-type-count-total villager 2)
    (set-goal 1 120)
)
(defrule
    (players-building-type-count any-ally market >= 1)
=>
    (up-modify-goal 1 c:- 30)
)
(defrule
    (can-train villager)
    (up-compare-goal 2 g:< 1)
=>
    (train villager)
)
(defrule
    (players-building-type-count any-ally market >= 1)
    (can-build market)
    (building-type-count-total market < 2)
=>
    (build market)
)
(defrule
    (players-building-type-count any-ally market >= 1)
    (can-research ri-caravan)
=>
    (research ri-caravan)
)
(defrule
    (players-building-type-count any-ally market >= 1)
    (unit-type-count 178 == 0)
    (unit-type-count 205 == 0)
    (can-train trade-cart)
    (unit-type-count-total trade-cart < 30)
=>
    (train trade-cart)
)
(defrule
    (true)
=>
    (up-get-fact population 0 3)
    (up-get-fact population-cap 0 4)
    (up-modify-goal 4 g:- 3)
)
(defrule
    (up-compare-goal 4 c:< 10)
    (civilian-population >= 120)
=>
    (delete-unit villager)
)

```
---
```
train 120 villagers with 30 trade using escrow for caravan
```
```
(defrule
    (true)
=>
    (up-get-fact unit-type-count-total villager 2)
    (set-goal 1 120)
)
(defrule
    (players-building-type-count any-ally market >= 1)
=>
    (up-modify-goal 1 c:- 30)
)
(defrule
    (can-train villager)
    (up-compare-goal 2 g:< 1)
=>
    (train villager)
)
(defrule
    (players-building-type-count any-ally market >= 1)
    (can-build market)
    (building-type-count-total market < 2)
=>
    (build market)
)
(defrule
    (players-building-type-count any-ally market >= 1)
    (can-research-with-escrow ri-caravan)
=>
    (release-escrow food)
    (release-escrow gold)
    (research ri-caravan)
)
(defrule
    (players-building-type-count any-ally market >= 1)
    (unit-type-count 178 == 0)
    (unit-type-count 205 == 0)
    (can-train trade-cart)
    (unit-type-count-total trade-cart < 30)
=>
    (train trade-cart)
)
(defrule
    (true)
=>
    (up-get-fact population 0 3)
    (up-get-fact population-cap 0 4)
    (up-modify-goal 4 g:- 3)
)
(defrule
    (up-compare-goal 4 c:< 10)
    (civilian-population >= 120)
=>
    (delete-unit villager)
)

```
---
