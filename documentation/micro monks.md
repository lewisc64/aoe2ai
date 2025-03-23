# micro monks
Forces monks to only convert one target each.
## Usage
```
micro monks
```
## Examples
```
micro monks
```
```
(defrule
    (true)
=>
    (up-full-reset-search)
    (set-strategic-number sn-focus-player-number 1)
    (set-goal 1 0)
)
(defrule
    (players-stance focus-player enemy)
=>
    (up-find-remote c: archery-class c: 255)
)
(defrule
    (strategic-number sn-focus-player-number < 8)
=>
    (up-modify-sn sn-focus-player-number c:+ 1)
    (up-jump-rule -2)
)
(defrule
    (true)
=>
    (up-reset-search 1 1 0 0)
    (up-reset-filters)
    (up-find-local c: monastery-class c: 255)
    (up-remove-objects search-local -1 g:!= 1)
    (up-get-search-state 3)
)
(defrule
    (up-compare-goal 3 c:>= 1)
=>
    (up-set-target-object search-local c: 0)
    (up-get-point position-object 41)
    (up-set-target-point 41)
    (up-clean-search search-remote 44 1)
    (up-set-target-object search-remote c: 0)
    (up-chat-data-to-all "i: %d" g: 1)
)
(defrule
    (up-compare-goal 3 c:>= 1)
=>
    (up-target-objects 1 action-default -1 -1)
)
(defrule
    (up-compare-goal 3 c:>= 1)
=>
    (up-modify-goal 1 c:+ 1)
    (up-jump-rule -4)
)

```
---
