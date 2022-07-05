# trade excess
Uses the market to rebalance resources around a certain threshold. Ignores escrowed values.
## Usage
```
trade excess RESOURCE_LIST at AMOUNT
```
## Examples
```
trade excess food at 2000
```
```
(defrule
    (true)
=>
    (up-get-fact resource-amount food 2)
    (up-get-fact escrow-amount food 1)
    (up-modify-goal 2 g:- 1)
)
(defrule
    (up-compare-goal 2 c:> 2000)
    (can-sell-commodity food)
=>
    (sell-commodity food)
)

```
---
```
trade excess wood and food and gold at 2000
```
```
(defrule
    (true)
=>
    (up-get-fact resource-amount wood 2)
    (up-get-fact escrow-amount wood 1)
    (up-modify-goal 2 g:- 1)
    (up-get-fact resource-amount food 3)
    (up-get-fact escrow-amount food 1)
    (up-modify-goal 3 g:- 1)
    (up-get-fact resource-amount gold 4)
    (up-get-fact escrow-amount gold 1)
    (up-modify-goal 4 g:- 1)
)
(defrule
    (up-compare-goal 2 c:> 2000)
    (can-sell-commodity wood)
    (up-compare-goal 4 c:< 2000)
=>
    (sell-commodity wood)
)
(defrule
    (up-compare-goal 3 c:> 2000)
    (can-sell-commodity food)
    (up-compare-goal 4 c:< 2000)
=>
    (sell-commodity food)
)
(defrule
    (up-compare-goal 2 c:< 2000)
    (up-compare-goal 4 c:> 2000)
    (can-buy-commodity wood)
=>
    (buy-commodity wood)
)
(defrule
    (up-compare-goal 3 c:< 2000)
    (up-compare-goal 4 c:> 2000)
    (can-buy-commodity food)
=>
    (buy-commodity food)
)

```
---
