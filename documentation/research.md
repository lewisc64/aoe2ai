# research
Sets up the rule to research the specified research.
## Usage
```
research TECH_NAME with RESOURCE_NAME escrow
```
## Examples
```
research ri-loom
```
```
(defrule
    (can-research ri-loom)
=>
    (research ri-loom)
)

```
---
```
research feudal-age with food and gold escrow
```
```
(defrule
    (can-research-with-escrow feudal-age)
=>
    (release-escrow food)
    (release-escrow gold)
    (research feudal-age)
)

```
---
```
research blacksmith infantry upgrades
```
```
(defrule
    (can-research ri-forging)
=>
    (research ri-forging)
)
(defrule
    (can-research ri-iron-casting)
=>
    (research ri-iron-casting)
)
(defrule
    (can-research ri-blast-furnace)
=>
    (research ri-blast-furnace)
)
(defrule
    (can-research ri-scale-mail)
=>
    (research ri-scale-mail)
)
(defrule
    (can-research ri-chain-mail)
=>
    (research ri-chain-mail)
)
(defrule
    (can-research ri-plate-mail)
=>
    (research ri-plate-mail)
)

```
---
