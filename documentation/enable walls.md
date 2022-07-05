# enable walls
Sets up rule that allows the AI to build walls on the specified perimeter.
## Usage
```
enable walls on perimeter PERIMETER_NUMBER
```
## Examples
```
enable walls on perimeter 1
```
```
(defrule
    (true)
=>
    (enable-wall-placement 1)
    (disable-self)
)

```
---
```
enable walls on perimeter 2
```
```
(defrule
    (true)
=>
    (enable-wall-placement 2)
    (disable-self)
)

```
---
