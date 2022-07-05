# set strategic number
Sets a strategic number. Uses the 'sn-' prefix for recognition of the rule.
## Usage
```
STRATEGIC_NUMBER_NAME = VALUE
```
## Examples
```
sn-gold-gatherer-percentage = 50
```
```
(defrule
    (true)
=>
    (set-strategic-number sn-gold-gatherer-percentage 50)
)

```
---
```
sn-gold-gatherer-percentage -= 5
```
```
(defrule
    (true)
=>
    (up-modify-sn sn-gold-gatherer-percentage c:- 5)
)

```
---
