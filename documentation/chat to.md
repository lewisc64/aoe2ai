# chat to
## Usage
```
chat to PLAYER_TYPE "MESSAGE"
```
## Examples
```
chat to all "hello everyone!"
```
```
(defrule
    (true)
=>
    (chat-to-all "hello everyone!")
)

```
---
```
chat to self "hello me!"
```
```
(defrule
    (true)
=>
    (chat-local-to-self "hello me!")
)

```
---
```
chat to allies "hello friends!"
```
```
(defrule
    (true)
=>
    (chat-to-allies "hello friends!")
)

```
---
```
chat to 5 "hello player 5!"
```
```
(defrule
    (true)
=>
    (chat-to-player 5 "hello player 5!")
)

```
---
```
chat to target-player "hello target player!"
```
```
(defrule
    (true)
=>
    (chat-to-player target-player "hello target player!")
)

```
---
