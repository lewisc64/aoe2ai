# set up scouting
## Usage
```
set up scouting
```
## Examples
```
set up scouting
```
```
(defrule
    (true)
=>
    (set-strategic-number sn-percent-civilian-explorers 0)
    (set-strategic-number sn-cap-civilian-explorers 0)
    (set-strategic-number sn-total-number-explorers 1)
    (set-strategic-number sn-number-explore-groups 1)
    (set-strategic-number sn-initial-exploration-required 0)
    (set-strategic-number sn-wild-animal-exploration 1)
    (disable-self)
)

```
---
