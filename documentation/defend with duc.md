# defend with duc
Moves untasked far units towards the town.

Affects the following sn's:
 - sn-disable-defend-groups (sets to 1)
 - sn-number-attack-groups (sets to 0)
 - sn-gather-defense-units (sets to 0)
## Usage
```
defend with duc
```
## Examples
```
defend with duc
```
```
(defrule
    (true)
=>
    (set-strategic-number sn-disable-defend-groups 1)
    (set-strategic-number sn-number-attack-groups 0)
    (set-strategic-number sn-gather-defense-units 0)
    (up-get-point position-self 41)
    (up-set-target-point 41)
    (up-full-reset-search)
    (up-filter-exclude -1 -1 orderid-explore -1)
    (up-filter-range -1 -1 30 -1)
    (up-find-local c: archery-class c: 255)
    (up-find-local c: infantry-class c: 255)
    (up-find-local c: petard-class c: 255)
    (up-find-local c: archery-cannon-class c: 255)
    (up-remove-objects search-local 49 == 0)
    (up-target-point 41 19 -1 stance-aggressive)
    (up-reset-search 1 1 0 0)
    (up-find-local c: cavalry-class c: 255)
    (up-find-local c: cavalry-archer-class c: 255)
    (up-find-local c: cavalry-cannon-class c: 255)
    (up-find-local c: 947 c: 255)
    (up-remove-objects search-local 49 == 0)
    (up-target-point 41 19 -1 stance-aggressive)
    (up-reset-search 1 1 0 0)
    (up-find-local c: siege-weapon-class c: 255)
    (up-find-local c: monastery-class c: 255)
    (up-find-local c: scorpion-class c: 255)
    (up-find-local c: packed-trebuchet-class c: 255)
    (up-find-local c: unpacked-trebuchet-class c: 255)
    (up-remove-objects search-local 49 == 0)
    (up-target-point 41 19 -1 stance-aggressive)
)

```
---
