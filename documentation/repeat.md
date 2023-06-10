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
(defconst chat-aaf4c61ddcc5e8a2dabede0f3b482cd9aea9434d "hello")
(defrule
    (true)
=>
    (enable-timer 1 30)
    (disable-self)
)
(defrule
    (timer-triggered 1)
=>
    (chat-to-all chat-aaf4c61ddcc5e8a2dabede0f3b482cd9aea9434d)
    (disable-timer 1)
    (enable-timer 1 30)
)

```
---
