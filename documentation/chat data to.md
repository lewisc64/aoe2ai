# chat data to
## Usage
```
chat goal/sn/data to PLAYER_TYPE "MESSAGE" DATA
```
## Examples
```
chat goal to all "Hello everyone! Goal 1 is %d!" 1
```
```
(defconst chat-c6966003051a186fb8b44aa622fcdbdb188dcc31 "Hello everyone! Goal 1 is %d!")
(defrule
    (true)
=>
    (up-chat-data-to-all chat-c6966003051a186fb8b44aa622fcdbdb188dcc31 g: 1)
)

```
---
```
chat sn to self "My town size is: %d" sn-maximum-town-size
```
```
(defconst chat-a89d8e72f45f347653013cd4bf54a6fa7696cc6b "My town size is: %d")
(defrule
    (true)
=>
    (up-chat-data-local-to-self chat-a89d8e72f45f347653013cd4bf54a6fa7696cc6b s: sn-maximum-town-size)
)

```
---
```
@up-get-player-color my-player-number -1
chat const to allies "I am color %s!" 7031232
```
```
(defconst chat-f4583a8415ae0a42b71001e9cc3aa50de6ab3d8d "I am color %s!")
(defrule
    (true)
=>
    (up-get-player-color my-player-number -1)
    (up-chat-data-to-allies chat-f4583a8415ae0a42b71001e9cc3aa50de6ab3d8d c: 7031232)
)

```
---
