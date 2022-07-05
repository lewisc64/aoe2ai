# drop off food
## Usage
```
drop off food
```
## Examples
```
drop off food
```
```
(defrule
    (true)
=>
    (up-drop-resources sheep-food c: 5)
    (up-drop-resources farm-food c: 5)
    (up-drop-resources forage-food c: 5)
    (up-drop-resources deer-food c: 20)
    (up-drop-resources boar-food c: 10)
)

```
---
