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
(defconst chat-ae73cad7048986423902bfb35b0725445e57f22d "two!")
(defconst chat-bc61c414f24f721c838e924297e6446ab4361886 "one!")
(defrule
    (true)
=>
    (set-goal 1 0)
    (generate-random-number 2)
    (up-get-fact random-number 0 1)
    (up-get-precise-time 0 2)
    (up-modify-goal 1 g:+ 2)
    (up-modify-goal 1 c:mod 2)
    (up-modify-goal 1 c:+ 1)
)
(defrule
    (goal 1 1)
=>
    (chat-to-all chat-bc61c414f24f721c838e924297e6446ab4361886)
)
(defrule
    (goal 1 2)
=>
    (chat-to-all chat-ae73cad7048986423902bfb35b0725445e57f22d)
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
(defconst chat-ae73cad7048986423902bfb35b0725445e57f22d "two!")
(defconst chat-bc61c414f24f721c838e924297e6446ab4361886 "one!")
(defrule
    (true)
=>
    (set-goal 1 0)
    (generate-random-number 2)
    (up-get-fact random-number 0 1)
    (up-get-precise-time 0 2)
    (up-modify-goal 1 g:+ 2)
    (up-modify-goal 1 c:mod 2)
    (up-modify-goal 1 c:+ 1)
    (disable-self)
)
(defrule
    (goal 1 1)
=>
    (chat-to-all chat-bc61c414f24f721c838e924297e6446ab4361886)
)
(defrule
    (goal 1 2)
=>
    (chat-to-all chat-ae73cad7048986423902bfb35b0725445e57f22d)
)

```
---
