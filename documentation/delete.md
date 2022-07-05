# delete
Creates a rule that deletes the specified object.
## Usage
```
delete unit/building NAME
```
## Examples
```
delete unit villager
```
```
(defrule
    (true)
=>
    (delete-unit villager)
)

```
---
```
delete building town-center
```
```
(defrule
    (true)
=>
    (delete-building town-center)
)

```
---
