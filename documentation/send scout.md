# send scout
Sets up the userpatch rule to send the scout somewhere else.
## Usage
```
scout AREA_NAME
```
## Examples
```
scout opposite
```
```
(defrule
    (true)
=>
    (up-send-scout group-type-land-explore scout-opposite)
)

```
---
```
scout enemy
```
```
(defrule
    (true)
=>
    (up-send-scout group-type-land-explore scout-enemy)
)

```
---
