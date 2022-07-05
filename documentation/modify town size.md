# modify town size
Modifies the sn-maximum-town-size strategic number.
## Usage
```
set/increase/decrease town size by/to AMOUNT
```
## Examples
```
set town size to 30
```
```
(defrule
    (true)
=>
    (set-strategic-number sn-maximum-town-size 30)
)

```
---
```
increase town size by 10
```
```
(defrule
    (true)
=>
    (up-modify-sn sn-maximum-town-size c:+ 10)
)

```
---
```
decrease town size by 5
```
```
(defrule
    (true)
=>
    (up-modify-sn sn-maximum-town-size c:- 5)
)

```
---
