# repeat
Each rule is allowed to be triggered once after the time has elapsed, the process repeats.
## Usage
```
#repeat every AMOUNT TIME_UNIT
    RULES
#end repeat
```
## Examples
```
#repeat every 30 seconds
    chat to all "hello"
#end repeat
```
```
(defrule
    (true)
=>
    (enable-timer 1 30)
    (disable-self)
)
(defrule
    (timer-triggered 1)
=>
    (chat-to-all "hello")
    (disable-timer 1)
    (enable-timer 1 30)
)

```
---
