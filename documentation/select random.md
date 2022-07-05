# select random
A random block separated by randors will be allowed to execute. Using persistant mode means the randomly chosen one is picked every time, otherwise it will change.
## Usage
```
#select random ?persistant
   RULES
#randor
   RULES
#end select
```
## Examples
```
#select random
    chat to all "one!"
#randor
    chat to all "two!"
#end select
```
```
(defrule
    (true)
=>
    (set-goal 1 0)
    (generate-random-number 2)
    (up-get-fact random-number 0 1)
)
(defrule
    (goal 1 1)
=>
    (chat-to-all "one!")
)
(defrule
    (goal 1 2)
=>
    (chat-to-all "two!")
)

```
---
```
#select random persistant
    chat to all "one!"
#randor
    chat to all "two!"
#end select
```
```
(defrule
    (true)
=>
    (set-goal 1 0)
    (generate-random-number 2)
    (up-get-fact random-number 0 1)
    (disable-self)
)
(defrule
    (goal 1 1)
=>
    (chat-to-all "one!")
)
(defrule
    (goal 1 2)
=>
    (chat-to-all "two!")
)

```
---
