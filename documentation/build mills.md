# build mills
Builds mills based on how many farms there are.
## Usage
```
build mills
```
## Examples
```
build mills
```
```
(defrule
    (true)
=>
    (set-goal 1 0)
    (up-get-fact building-type-count-total town-center 2)
    (up-modify-goal 2 c:* 9)
    (up-modify-goal 1 g:+ 2)
    (up-get-fact building-type-count-total mill 2)
    (up-modify-goal 2 c:* 6)
    (up-modify-goal 1 g:+ 2)
    (up-get-fact building-type-count-total farm 2)
)
(defrule
    (or
      (building-type-count-total mill == 0)
      (and
        (not
          (civ-selected khmer)
        )
        (up-compare-goal 2 g:>= 1)
      )
    )
    (or
      (resource-found food)
      (and
        (not
          (civ-selected khmer)
        )
        (game-time >= 60)
      )
    )
    (can-build mill)
=>
    (build mill)
)

```
---
