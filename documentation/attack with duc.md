# attack with duc
Attacks the target player with all military units using the attack move command.

Affects the following sn's:
 - sn-disable-defend-groups (sets to 1)
 - sn-number-attack-groups (sets to 0)
 - sn-gather-defense-units (sets to 0)
## Usage
```
attack with duc
```
## Examples
```
attack with duc
```
```
(defrule
    (player-in-game target-player)
=>
    (set-strategic-number sn-disable-defend-groups 1)
    (set-strategic-number sn-number-attack-groups 0)
    (set-strategic-number sn-gather-defense-units 0)
    (up-full-reset-search)
    (up-filter-exclude -1 -1 orderid-explore -1)
    (up-find-local c: archery-class c: 255)
    (up-find-local c: infantry-class c: 255)
    (up-find-local c: cavalry-class c: 255)
    (up-find-local c: siege-weapon-class c: 255)
    (up-find-local c: monastery-class c: 255)
    (up-find-local c: petard-class c: 255)
    (up-find-local c: cavalry-archer-class c: 255)
    (up-find-local c: archery-cannon-class c: 255)
    (up-find-local c: 947 c: 255)
    (up-find-local c: scorpion-class c: 255)
    (up-remove-objects search-local 49 == 0)
    (up-get-point position-enemy 41)
)
(defrule
    (player-in-game target-player)
    (nand
      (up-compare-goal 41 c:<= 0)
      (up-compare-goal 42 c:<= 0)
    )
=>
    (up-target-point 41 19 -1 stance-aggressive)
)
(defrule
    (player-in-game target-player)
    (unit-type-count trebuchet-set >= 1)
=>
    (up-full-reset-search)
    (up-find-local c: packed-trebuchet-class c: 255)
    (up-find-local c: unpacked-trebuchet-class c: 255)
    (up-modify-sn sn-focus-player-number s:= sn-target-player-number)
    (up-find-remote c: castle c: 255)
    (up-find-remote c: town-center c: 255)
    (up-find-remote c: tower-class c: 255)
    (up-find-remote c: donjon c: 255)
    (up-find-remote c: krepost c: 255)
    (up-get-point position-self 41)
    (up-set-target-point 41)
    (up-clean-search search-remote 44 1)
    (up-set-target-object search-remote c: 0)
    (up-target-objects 1 action-default -1 -1)
)
(defrule
    (player-in-game target-player)
=>
    (enable-timer 1 300)
    (disable-self)
)
(defrule
    (player-in-game target-player)
    (timer-triggered 1)
=>
    (up-reset-unit c: archery-class)
    (up-reset-unit c: infantry-class)
    (up-reset-unit c: cavalry-class)
    (up-reset-unit c: siege-weapon-class)
    (up-reset-unit c: monastery-class)
    (up-reset-unit c: petard-class)
    (up-reset-unit c: cavalry-archer-class)
    (up-reset-unit c: archery-cannon-class)
    (up-reset-unit c: 947)
    (up-reset-unit c: scorpion-class)
    (chat-to-all "resetti")
    (disable-timer 1)
    (enable-timer 1 300)
)

```
---
