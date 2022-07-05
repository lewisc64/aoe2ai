# no merge
Prevents the rules within from being merged together in compilation.
## Usage
```
#nomerge
   RULES
#end nomerge
```
## Examples
```
#nomerge
    chat to all "in"
    chat to all "different"
    chat to all "rules"
#end nomerge
```
```
(defrule
    (true)
=>
    (chat-to-all "in")
)
(defrule
    (true)
=>
    (chat-to-all "different")
)
(defrule
    (true)
=>
    (chat-to-all "rules")
)

```
---
