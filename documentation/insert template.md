# insert template
Inserts all the rules within a template. Can make replacements as a form of pre-processing.
## Usage
```
insert TEMPLATE_NAME
```
## Examples
```
#template train-unit
    train {unit}
#end template

insert train-unit(unit="archer-line")
```
```
(defrule
    (can-train archer-line)
=>
    (train archer-line)
)

```
---
