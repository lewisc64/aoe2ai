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
(defconst chat-1acc0c583f07ae7a0c0e7dc39ae26e88cc304e1b "hello everyone!")
(defrule
    (true)
=>
    (chat-to-all chat-1acc0c583f07ae7a0c0e7dc39ae26e88cc304e1b)
)

```
---
```
chat to self "hello me!"
```
```
(defconst chat-6e66b8941bffb34802de3397bdf06978888ee373 "hello me!")
(defrule
    (true)
=>
    (chat-local-to-self chat-6e66b8941bffb34802de3397bdf06978888ee373)
)

```
---
```
chat to allies "hello friends!"
```
```
(defconst chat-48ffe9cfe6df84f6a27ca3d1f18d42fb850b88bd "hello friends!")
(defrule
    (true)
=>
    (chat-to-allies chat-48ffe9cfe6df84f6a27ca3d1f18d42fb850b88bd)
)

```
---
```
chat to 5 "hello player 5!"
```
```
(defconst chat-1271292582e1e9f194e3782388100bac2060237e "hello player 5!")
(defrule
    (true)
=>
    (chat-to-player 5 chat-1271292582e1e9f194e3782388100bac2060237e)
)

```
---
```
chat to target-player "hello target player!"
```
```
(defconst chat-03b0008762eb91080abf552cfc9788b0b2228ae5 "hello target player!")
(defrule
    (true)
=>
    (chat-to-player target-player chat-03b0008762eb91080abf552cfc9788b0b2228ae5)
)

```
---
