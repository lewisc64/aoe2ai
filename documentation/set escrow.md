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
