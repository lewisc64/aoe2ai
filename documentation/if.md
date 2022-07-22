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
(defrule
    (goal 1 1)
=>
    (chat-to-all "goal 1 is 1!")
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
(defrule
    (current-age == dark-age)
=>
    (chat-to-all "dark age")
)
(defrule
    (current-age == feudal-age)
    (not
      (current-age == dark-age)
    )
=>
    (chat-to-all "feudal age")
)
(defrule
    (not
      (current-age == dark-age)
    )
    (not
      (current-age == feudal-age)
    )
=>
    (chat-to-all "castle age or imperial age")
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
    (chat-to-all "goal 5 is 1")
    (set-goal 5 2)
)
(defrule
    (goal 5 2)
    (goal 1 1)
=>
    (chat-to-all "This will still execute, because the condition `goal 5 1` was frozen into a goal due to `ifg`.")
)

```
---
