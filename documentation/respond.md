# respond
When the AI sees the specified amount, it reacts with the specified parameters. If building/unit is unspecified, unit is assumed. If amount is unspecified, 1 is assumed.
## Usage
```
respond to ?AMOUNT NAME ?BUILDING/UNIT with NAME ?BUILDING/UNIT
```
## Examples
```
respond to 2 scout-cavalry-line with 5 spearman-line
```
```
(defrule
    (players-unit-type-count any-enemy scout-cavalry-line >= 2)
    (unit-type-count-total spearman-line < 5)
    (can-train spearman-line)
=>
    (train spearman-line)
)

```
---
```
respond to archery-range building with 10 skirmisher-line
```
```
(defrule
    (players-building-type-count any-enemy archery-range >= 1)
    (unit-type-count-total skirmisher-line < 10)
    (can-train skirmisher-line)
=>
    (train skirmisher-line)
)

```
---
