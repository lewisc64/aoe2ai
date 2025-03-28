# delay
Block body is only allowed to trigger after the time is up.
## Usage
```
#delay by AMOUNT TIME_UNIT
    RULES
#end delay
```
## Examples
```
#delay by 30 seconds
    chat to all "30 in-game seconds has passed."
#end delay
```
```
(defconst chat-b640597567a441556749250ce228bf3ad5f03a3f "30 in-game seconds has passed.")
(defrule
    (true)
=>
    (enable-timer 1 30)
    (disable-self)
)
(defrule
    (timer-triggered 1)
=>
    (chat-to-all chat-b640597567a441556749250ce228bf3ad5f03a3f)
)

```
---
```
#delay by 30 minutes
    chat to all "30 in-game minutes has passed."
#end delay
```
```
(defconst chat-a4b9a95b5ede9f308f8196a9b2b99623c578d34a "30 in-game minutes has passed.")
(defrule
    (true)
=>
    (enable-timer 1 1800)
    (disable-self)
)
(defrule
    (timer-triggered 1)
=>
    (chat-to-all chat-a4b9a95b5ede9f308f8196a9b2b99623c578d34a)
)

```
---
```
#delay by 5 real seconds
    chat to all "5 real seconds have passed."
#end delay
```
```
(defconst chat-4d67ce39e29a395c4e17c819d2bb18d357d0e065 "5 real seconds have passed.")
(defrule
    (true)
=>
    (up-get-precise-time 0 1)
    (up-modify-goal 1 c:+ 5000)
    (set-goal 2 0)
    (disable-self)
)
(defrule
    (true)
=>
    (up-get-precise-time 0 2)
)
(defrule
    (up-compare-goal 2 g:>= 1)
=>
    (chat-to-all chat-4d67ce39e29a395c4e17c819d2bb18d357d0e065)
)

```
---
