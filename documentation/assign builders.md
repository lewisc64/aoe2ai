# assign builders
Sets the amount of builders that should build a building.
## Usage
```
assign AMOUNT builders to BUILDING_NAME
```
## Examples
```
assign 8 builders to castle
```
```
(defrule
    (true)
=>
    (up-assign-builders c: castle c: 8)
)

```
---
