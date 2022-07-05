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
(defrule
    (true)
=>
    (chat-to-all "hello")
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
(defrule
    (true)
=>
    (set-goal 1 1)
    (disable-self)
)
(defrule
    (goal 1 1)
=>
    (chat-to-all "hello")
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
