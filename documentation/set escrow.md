# set escrow
Creates rule that sets the escrow percentage.
## Usage
```
escrow PERCENTAGE RESOURCE_NAME
```
## Examples
```
escrow 10 wood
```
```
(defrule
    (true)
=>
    (set-escrow-percentage wood 10)
)

```
---
```
escrow 50 gold with maximum 300
```
```
(defrule
    (escrow-amount gold < 300)
=>
    (set-escrow-percentage gold 50)
)
(defrule
    (escrow-amount gold >= 300)
=>
    (set-escrow-percentage gold 0)
    (up-modify-escrow gold c:= 300)
)

```
---
