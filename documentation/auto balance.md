# auto balance
Redistributes villagers at a set interval around a threshold. By default it will balance every 60 seconds with a threshold of 300. When getting the resource amounts, it subtracts the escrowed portion.
## Usage
```
auto balance RESOURCES around THRESHOLD every AMOUNT seconds
```
## Examples
```
auto balance wood and food and gold
```
```
(defrule
    (true)
=>
    (enable-timer 1 60)
    (disable-self)
)
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
    (strategic-number sn-wood-gatherer-percentage >= 2)
    (up-compare-goal 2 c:> 300)
    (strategic-number sn-food-gatherer-percentage >= 2)
    (up-compare-goal 3 c:> 300)
    (strategic-number sn-gold-gatherer-percentage <= 96)
    (up-compare-goal 4 c:<= 300)
    (timer-triggered 1)
=>
    (up-modify-sn sn-wood-gatherer-percentage c:- 2)
    (up-modify-sn sn-food-gatherer-percentage c:- 2)
    (up-modify-sn sn-gold-gatherer-percentage c:+ 4)
)
(defrule
    (strategic-number sn-wood-gatherer-percentage >= 2)
    (up-compare-goal 2 c:> 300)
    (strategic-number sn-gold-gatherer-percentage >= 2)
    (up-compare-goal 4 c:> 300)
    (strategic-number sn-food-gatherer-percentage <= 96)
    (up-compare-goal 3 c:<= 300)
    (timer-triggered 1)
=>
    (up-modify-sn sn-wood-gatherer-percentage c:- 2)
    (up-modify-sn sn-gold-gatherer-percentage c:- 2)
    (up-modify-sn sn-food-gatherer-percentage c:+ 4)
)
(defrule
    (strategic-number sn-wood-gatherer-percentage >= 4)
    (up-compare-goal 2 c:> 300)
    (strategic-number sn-food-gatherer-percentage <= 98)
    (up-compare-goal 3 c:<= 300)
    (strategic-number sn-gold-gatherer-percentage <= 98)
    (up-compare-goal 4 c:<= 300)
    (timer-triggered 1)
=>
    (up-modify-sn sn-wood-gatherer-percentage c:- 4)
    (up-modify-sn sn-food-gatherer-percentage c:+ 2)
    (up-modify-sn sn-gold-gatherer-percentage c:+ 2)
)
(defrule
    (strategic-number sn-food-gatherer-percentage >= 2)
    (up-compare-goal 3 c:> 300)
    (strategic-number sn-gold-gatherer-percentage >= 2)
    (up-compare-goal 4 c:> 300)
    (strategic-number sn-wood-gatherer-percentage <= 96)
    (up-compare-goal 2 c:<= 300)
    (timer-triggered 1)
=>
    (up-modify-sn sn-food-gatherer-percentage c:- 2)
    (up-modify-sn sn-gold-gatherer-percentage c:- 2)
    (up-modify-sn sn-wood-gatherer-percentage c:+ 4)
)
(defrule
    (strategic-number sn-food-gatherer-percentage >= 4)
    (up-compare-goal 3 c:> 300)
    (strategic-number sn-wood-gatherer-percentage <= 98)
    (up-compare-goal 2 c:<= 300)
    (strategic-number sn-gold-gatherer-percentage <= 98)
    (up-compare-goal 4 c:<= 300)
    (timer-triggered 1)
=>
    (up-modify-sn sn-food-gatherer-percentage c:- 4)
    (up-modify-sn sn-wood-gatherer-percentage c:+ 2)
    (up-modify-sn sn-gold-gatherer-percentage c:+ 2)
)
(defrule
    (strategic-number sn-gold-gatherer-percentage >= 4)
    (up-compare-goal 4 c:> 300)
    (strategic-number sn-wood-gatherer-percentage <= 98)
    (up-compare-goal 2 c:<= 300)
    (strategic-number sn-food-gatherer-percentage <= 98)
    (up-compare-goal 3 c:<= 300)
    (timer-triggered 1)
=>
    (up-modify-sn sn-gold-gatherer-percentage c:- 4)
    (up-modify-sn sn-wood-gatherer-percentage c:+ 2)
    (up-modify-sn sn-food-gatherer-percentage c:+ 2)
)
(defrule
    (timer-triggered 1)
=>
    (disable-timer 1)
    (enable-timer 1 60)
)

```
---
```
auto balance all
```
```
(defrule
    (true)
=>
    (enable-timer 1 60)
    (disable-self)
)
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
    (up-get-fact resource-amount stone 5)
    (up-get-fact escrow-amount stone 1)
    (up-modify-goal 5 g:- 1)
)
(defrule
    (strategic-number sn-wood-gatherer-percentage >= 1)
    (up-compare-goal 2 c:> 300)
    (strategic-number sn-food-gatherer-percentage >= 1)
    (up-compare-goal 3 c:> 300)
    (strategic-number sn-gold-gatherer-percentage >= 1)
    (up-compare-goal 4 c:> 300)
    (strategic-number sn-stone-gatherer-percentage <= 97)
    (up-compare-goal 5 c:<= 300)
    (timer-triggered 1)
=>
    (up-modify-sn sn-wood-gatherer-percentage c:- 1)
    (up-modify-sn sn-food-gatherer-percentage c:- 1)
    (up-modify-sn sn-gold-gatherer-percentage c:- 1)
    (up-modify-sn sn-stone-gatherer-percentage c:+ 3)
)
(defrule
    (strategic-number sn-wood-gatherer-percentage >= 1)
    (up-compare-goal 2 c:> 300)
    (strategic-number sn-food-gatherer-percentage >= 1)
    (up-compare-goal 3 c:> 300)
    (strategic-number sn-stone-gatherer-percentage >= 1)
    (up-compare-goal 5 c:> 300)
    (strategic-number sn-gold-gatherer-percentage <= 97)
    (up-compare-goal 4 c:<= 300)
    (timer-triggered 1)
=>
    (up-modify-sn sn-wood-gatherer-percentage c:- 1)
    (up-modify-sn sn-food-gatherer-percentage c:- 1)
    (up-modify-sn sn-stone-gatherer-percentage c:- 1)
    (up-modify-sn sn-gold-gatherer-percentage c:+ 3)
)
(defrule
    (strategic-number sn-wood-gatherer-percentage >= 2)
    (up-compare-goal 2 c:> 300)
    (strategic-number sn-food-gatherer-percentage >= 2)
    (up-compare-goal 3 c:> 300)
    (strategic-number sn-gold-gatherer-percentage <= 98)
    (up-compare-goal 4 c:<= 300)
    (strategic-number sn-stone-gatherer-percentage <= 98)
    (up-compare-goal 5 c:<= 300)
    (timer-triggered 1)
=>
    (up-modify-sn sn-wood-gatherer-percentage c:- 2)
    (up-modify-sn sn-food-gatherer-percentage c:- 2)
    (up-modify-sn sn-gold-gatherer-percentage c:+ 2)
    (up-modify-sn sn-stone-gatherer-percentage c:+ 2)
)
(defrule
    (strategic-number sn-wood-gatherer-percentage >= 1)
    (up-compare-goal 2 c:> 300)
    (strategic-number sn-gold-gatherer-percentage >= 1)
    (up-compare-goal 4 c:> 300)
    (strategic-number sn-stone-gatherer-percentage >= 1)
    (up-compare-goal 5 c:> 300)
    (strategic-number sn-food-gatherer-percentage <= 97)
    (up-compare-goal 3 c:<= 300)
    (timer-triggered 1)
=>
    (up-modify-sn sn-wood-gatherer-percentage c:- 1)
    (up-modify-sn sn-gold-gatherer-percentage c:- 1)
    (up-modify-sn sn-stone-gatherer-percentage c:- 1)
    (up-modify-sn sn-food-gatherer-percentage c:+ 3)
)
(defrule
    (strategic-number sn-wood-gatherer-percentage >= 2)
    (up-compare-goal 2 c:> 300)
    (strategic-number sn-gold-gatherer-percentage >= 2)
    (up-compare-goal 4 c:> 300)
    (strategic-number sn-food-gatherer-percentage <= 98)
    (up-compare-goal 3 c:<= 300)
    (strategic-number sn-stone-gatherer-percentage <= 98)
    (up-compare-goal 5 c:<= 300)
    (timer-triggered 1)
=>
    (up-modify-sn sn-wood-gatherer-percentage c:- 2)
    (up-modify-sn sn-gold-gatherer-percentage c:- 2)
    (up-modify-sn sn-food-gatherer-percentage c:+ 2)
    (up-modify-sn sn-stone-gatherer-percentage c:+ 2)
)
(defrule
    (strategic-number sn-wood-gatherer-percentage >= 2)
    (up-compare-goal 2 c:> 300)
    (strategic-number sn-stone-gatherer-percentage >= 2)
    (up-compare-goal 5 c:> 300)
    (strategic-number sn-food-gatherer-percentage <= 98)
    (up-compare-goal 3 c:<= 300)
    (strategic-number sn-gold-gatherer-percentage <= 98)
    (up-compare-goal 4 c:<= 300)
    (timer-triggered 1)
=>
    (up-modify-sn sn-wood-gatherer-percentage c:- 2)
    (up-modify-sn sn-stone-gatherer-percentage c:- 2)
    (up-modify-sn sn-food-gatherer-percentage c:+ 2)
    (up-modify-sn sn-gold-gatherer-percentage c:+ 2)
)
(defrule
    (strategic-number sn-wood-gatherer-percentage >= 3)
    (up-compare-goal 2 c:> 300)
    (strategic-number sn-food-gatherer-percentage <= 99)
    (up-compare-goal 3 c:<= 300)
    (strategic-number sn-gold-gatherer-percentage <= 99)
    (up-compare-goal 4 c:<= 300)
    (strategic-number sn-stone-gatherer-percentage <= 99)
    (up-compare-goal 5 c:<= 300)
    (timer-triggered 1)
=>
    (up-modify-sn sn-wood-gatherer-percentage c:- 3)
    (up-modify-sn sn-food-gatherer-percentage c:+ 1)
    (up-modify-sn sn-gold-gatherer-percentage c:+ 1)
    (up-modify-sn sn-stone-gatherer-percentage c:+ 1)
)
(defrule
    (strategic-number sn-food-gatherer-percentage >= 1)
    (up-compare-goal 3 c:> 300)
    (strategic-number sn-gold-gatherer-percentage >= 1)
    (up-compare-goal 4 c:> 300)
    (strategic-number sn-stone-gatherer-percentage >= 1)
    (up-compare-goal 5 c:> 300)
    (strategic-number sn-wood-gatherer-percentage <= 97)
    (up-compare-goal 2 c:<= 300)
    (timer-triggered 1)
=>
    (up-modify-sn sn-food-gatherer-percentage c:- 1)
    (up-modify-sn sn-gold-gatherer-percentage c:- 1)
    (up-modify-sn sn-stone-gatherer-percentage c:- 1)
    (up-modify-sn sn-wood-gatherer-percentage c:+ 3)
)
(defrule
    (strategic-number sn-food-gatherer-percentage >= 2)
    (up-compare-goal 3 c:> 300)
    (strategic-number sn-gold-gatherer-percentage >= 2)
    (up-compare-goal 4 c:> 300)
    (strategic-number sn-wood-gatherer-percentage <= 98)
    (up-compare-goal 2 c:<= 300)
    (strategic-number sn-stone-gatherer-percentage <= 98)
    (up-compare-goal 5 c:<= 300)
    (timer-triggered 1)
=>
    (up-modify-sn sn-food-gatherer-percentage c:- 2)
    (up-modify-sn sn-gold-gatherer-percentage c:- 2)
    (up-modify-sn sn-wood-gatherer-percentage c:+ 2)
    (up-modify-sn sn-stone-gatherer-percentage c:+ 2)
)
(defrule
    (strategic-number sn-food-gatherer-percentage >= 2)
    (up-compare-goal 3 c:> 300)
    (strategic-number sn-stone-gatherer-percentage >= 2)
    (up-compare-goal 5 c:> 300)
    (strategic-number sn-wood-gatherer-percentage <= 98)
    (up-compare-goal 2 c:<= 300)
    (strategic-number sn-gold-gatherer-percentage <= 98)
    (up-compare-goal 4 c:<= 300)
    (timer-triggered 1)
=>
    (up-modify-sn sn-food-gatherer-percentage c:- 2)
    (up-modify-sn sn-stone-gatherer-percentage c:- 2)
    (up-modify-sn sn-wood-gatherer-percentage c:+ 2)
    (up-modify-sn sn-gold-gatherer-percentage c:+ 2)
)
(defrule
    (strategic-number sn-food-gatherer-percentage >= 3)
    (up-compare-goal 3 c:> 300)
    (strategic-number sn-wood-gatherer-percentage <= 99)
    (up-compare-goal 2 c:<= 300)
    (strategic-number sn-gold-gatherer-percentage <= 99)
    (up-compare-goal 4 c:<= 300)
    (strategic-number sn-stone-gatherer-percentage <= 99)
    (up-compare-goal 5 c:<= 300)
    (timer-triggered 1)
=>
    (up-modify-sn sn-food-gatherer-percentage c:- 3)
    (up-modify-sn sn-wood-gatherer-percentage c:+ 1)
    (up-modify-sn sn-gold-gatherer-percentage c:+ 1)
    (up-modify-sn sn-stone-gatherer-percentage c:+ 1)
)
(defrule
    (strategic-number sn-gold-gatherer-percentage >= 2)
    (up-compare-goal 4 c:> 300)
    (strategic-number sn-stone-gatherer-percentage >= 2)
    (up-compare-goal 5 c:> 300)
    (strategic-number sn-wood-gatherer-percentage <= 98)
    (up-compare-goal 2 c:<= 300)
    (strategic-number sn-food-gatherer-percentage <= 98)
    (up-compare-goal 3 c:<= 300)
    (timer-triggered 1)
=>
    (up-modify-sn sn-gold-gatherer-percentage c:- 2)
    (up-modify-sn sn-stone-gatherer-percentage c:- 2)
    (up-modify-sn sn-wood-gatherer-percentage c:+ 2)
    (up-modify-sn sn-food-gatherer-percentage c:+ 2)
)
(defrule
    (strategic-number sn-gold-gatherer-percentage >= 3)
    (up-compare-goal 4 c:> 300)
    (strategic-number sn-wood-gatherer-percentage <= 99)
    (up-compare-goal 2 c:<= 300)
    (strategic-number sn-food-gatherer-percentage <= 99)
    (up-compare-goal 3 c:<= 300)
    (strategic-number sn-stone-gatherer-percentage <= 99)
    (up-compare-goal 5 c:<= 300)
    (timer-triggered 1)
=>
    (up-modify-sn sn-gold-gatherer-percentage c:- 3)
    (up-modify-sn sn-wood-gatherer-percentage c:+ 1)
    (up-modify-sn sn-food-gatherer-percentage c:+ 1)
    (up-modify-sn sn-stone-gatherer-percentage c:+ 1)
)
(defrule
    (strategic-number sn-stone-gatherer-percentage >= 3)
    (up-compare-goal 5 c:> 300)
    (strategic-number sn-wood-gatherer-percentage <= 99)
    (up-compare-goal 2 c:<= 300)
    (strategic-number sn-food-gatherer-percentage <= 99)
    (up-compare-goal 3 c:<= 300)
    (strategic-number sn-gold-gatherer-percentage <= 99)
    (up-compare-goal 4 c:<= 300)
    (timer-triggered 1)
=>
    (up-modify-sn sn-stone-gatherer-percentage c:- 3)
    (up-modify-sn sn-wood-gatherer-percentage c:+ 1)
    (up-modify-sn sn-food-gatherer-percentage c:+ 1)
    (up-modify-sn sn-gold-gatherer-percentage c:+ 1)
)
(defrule
    (timer-triggered 1)
=>
    (disable-timer 1)
    (enable-timer 1 60)
)

```
---
```
auto balance wood and food every 30 seconds
```
```
(defrule
    (true)
=>
    (enable-timer 1 30)
    (disable-self)
)
(defrule
    (true)
=>
    (up-get-fact resource-amount wood 2)
    (up-get-fact escrow-amount wood 1)
    (up-modify-goal 2 g:- 1)
    (up-get-fact resource-amount food 3)
    (up-get-fact escrow-amount food 1)
    (up-modify-goal 3 g:- 1)
)
(defrule
    (strategic-number sn-wood-gatherer-percentage >= 4)
    (up-compare-goal 2 c:> 300)
    (strategic-number sn-food-gatherer-percentage <= 96)
    (up-compare-goal 3 c:<= 300)
    (timer-triggered 1)
=>
    (up-modify-sn sn-wood-gatherer-percentage c:- 4)
    (up-modify-sn sn-food-gatherer-percentage c:+ 4)
)
(defrule
    (strategic-number sn-food-gatherer-percentage >= 4)
    (up-compare-goal 3 c:> 300)
    (strategic-number sn-wood-gatherer-percentage <= 96)
    (up-compare-goal 2 c:<= 300)
    (timer-triggered 1)
=>
    (up-modify-sn sn-food-gatherer-percentage c:- 4)
    (up-modify-sn sn-wood-gatherer-percentage c:+ 4)
)
(defrule
    (timer-triggered 1)
=>
    (disable-timer 1)
    (enable-timer 1 30)
)

```
---
