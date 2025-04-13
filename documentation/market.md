# market
Buys/sells based on a condition. Ignores escrow amounts.
## Usage
```
buy/sell RESOURCE_NAME when RESOURCE_NAME COMPARISON AMOUNT
```
## Examples
```
buy food when gold > 100
```
```
(defrule
    (true)
=>
    (up-get-fact resource-amount gold 2)
    (up-get-fact escrow-amount gold 1)
    (up-modify-goal 2 g:- 1)
)
(defrule
    (up-compare-goal 2 c:> 100)
    (can-buy-commodity food)
=>
    (buy-commodity food)
)

```
---
```
sell wood when wood > 1500
```
```
(defrule
    (true)
=>
    (up-get-fact resource-amount wood 2)
    (up-get-fact escrow-amount wood 1)
    (up-modify-goal 2 g:- 1)
)
(defrule
    (up-compare-goal 2 c:> 1500)
    (can-sell-commodity wood)
=>
    (sell-commodity wood)
)

```
---
