# train
Trains a unit using the specified parameters.
## Usage
```
train UNIT_NAME
train UNIT_NAME with RESOURCE_NAME escrow
train AMOUNT UNIT_NAME
train AMOUNT UNIT_NAME with RESOURCE_NAME escrow
```
## Examples
```
train archer-line
```
```
(defrule
    (can-train archer-line)
=>
    (train archer-line)
)

```
---
```
train 10 archer-line
```
```
(defrule
    (can-train archer-line)
    (unit-type-count-total archer-line < 10)
=>
    (train archer-line)
)

```
---
```
train 10 archer-line with wood and gold escrow
```
```
(defrule
    (can-train-with-escrow archer-line)
    (unit-type-count-total archer-line < 10)
=>
    (release-escrow wood)
    (release-escrow gold)
    (train archer-line)
)

```
---
