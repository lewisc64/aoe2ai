# if
Adds a condition to the condition stack.
## Usage
```
#if CONDITION
    RULES
#else if CONDITION
    RULES
#else
    RULES
#end if
```
## Examples
```
#if goal 1 1
    chat to all "goal 1 is 1!"
#end if
```
```
(defconst chat-e9179705d0c627f4a2d395e7957281d8d0d69bf1 "goal 1 is 1!")
(defrule
    (goal 1 1)
=>
    (chat-to-all chat-e9179705d0c627f4a2d395e7957281d8d0d69bf1)
)

```
---
```
#if current-age == dark-age
    chat to all "dark age"
#else if current-age == feudal-age
    chat to all "feudal age"
#else
    chat to all "castle age or imperial age"
#end if
```
```
(defconst chat-4d9c4d85286d6ae16d6d4fd729fabbe716c6659e "castle age or imperial age")
(defconst chat-530921cc1ef4c0c7e73c193fea9686740bd199f6 "feudal age")
(defconst chat-1e11faadb3bb6245f005d3a003a0d3c2f339fac3 "dark age")
(defrule
    (current-age == dark-age)
=>
    (chat-to-all chat-1e11faadb3bb6245f005d3a003a0d3c2f339fac3)
)
(defrule
    (not
      (current-age == dark-age)
    )
    (current-age == feudal-age)
=>
    (chat-to-all chat-530921cc1ef4c0c7e73c193fea9686740bd199f6)
)
(defrule
    (not
      (current-age == dark-age)
    )
    (not
      (current-age == feudal-age)
    )
=>
    (chat-to-all chat-4d9c4d85286d6ae16d6d4fd729fabbe716c6659e)
)

```
---
```
#ifg goal 5 1
    chat to all "goal 5 is 1"
    @set-goal 5 2
    #if goal 5 2
        chat to all "This will still execute, because the condition `goal 5 1` was frozen into a goal due to `ifg`."
    #end if
#end if
```
```
(defconst chat-f0b89dd1434265a4c9c3d9691ade85b3c7757617 "This will still execute, because the condition `goal 5 1` was frozen into a goal due to `ifg`.")
(defconst chat-70b031f9bf0e9d1de01eda4071cc0b5e863fee04 "goal 5 is 1")
(defrule
    (true)
=>
    (set-goal 1 0)
)
(defrule
    (goal 5 1)
=>
    (set-goal 1 1)
)
(defrule
    (goal 1 1)
=>
    (chat-to-all chat-70b031f9bf0e9d1de01eda4071cc0b5e863fee04)
    (set-goal 5 2)
)
(defrule
    (goal 5 2)
    (goal 1 1)
=>
    (chat-to-all chat-f0b89dd1434265a4c9c3d9691ade85b3c7757617)
)

```
---
