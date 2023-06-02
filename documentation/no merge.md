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
(defconst chat-caa155adf81fddd29ab4b21a147927fb0295eb53 "rules")
(defconst chat-517d5a7240861ec297fa07542a7bf7470bb604fe "different")
(defconst chat-af10ef20dd9060bbeead0afbc55381a66af442ef "in")
(defrule
    (true)
=>
    (chat-to-all chat-af10ef20dd9060bbeead0afbc55381a66af442ef)
)
(defrule
    (true)
=>
    (chat-to-all chat-517d5a7240861ec297fa07542a7bf7470bb604fe)
)
(defrule
    (true)
=>
    (chat-to-all chat-caa155adf81fddd29ab4b21a147927fb0295eb53)
)

```
---
