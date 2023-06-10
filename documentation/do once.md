# do once
Adds 'disable-self' to the action stack. Makes sure each rule in the block individually runs only once. Can specify 'grouped' afterwards to additionaly switch on a goal.
## Usage
```
#do once ?grouped
    RULES
#end do
```
## Examples
```
#do once
    chat to all "hello"
#end do
```
```
(defconst chat-aaf4c61ddcc5e8a2dabede0f3b482cd9aea9434d "hello")
(defrule
    (true)
=>
    (chat-to-all chat-aaf4c61ddcc5e8a2dabede0f3b482cd9aea9434d)
    (disable-self)
)

```
---
```
#do once grouped
    chat to all "hello"
#end do
```
```
(defconst chat-aaf4c61ddcc5e8a2dabede0f3b482cd9aea9434d "hello")
(defrule
    (true)
=>
    (set-goal 1 1)
    (disable-self)
)
(defrule
    (goal 1 1)
=>
    (chat-to-all chat-aaf4c61ddcc5e8a2dabede0f3b482cd9aea9434d)
    (disable-self)
)
(defrule
    (true)
=>
    (set-goal 1 0)
    (disable-self)
)

```
---
