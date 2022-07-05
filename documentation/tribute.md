# tribute
Gives the specified player resources.
## Usage
```
tribute AMOUNT RESOURCE_NAME to PLAYER_NUMBER
```
## Examples
```
tribute 300 food to any-ally
```
```
(defrule
    (true)
=>
    (tribute-to-player any-ally food 300)
)

```
---
