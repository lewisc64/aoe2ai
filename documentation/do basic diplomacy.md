# do basic diplomacy
Includes rules to manage open diplomacy games. Tries to maintain one enemy.
## Usage
```
do basic diplomacy
do basic diplomacy without backstabbing
```
## Examples
```
do basic diplomacy
```
```
(defrule
    (true)
=>
    (set-stance every-enemy neutral)
    (disable-self)
)
(defrule
    (true)
=>
    (generate-random-number 8)
    (set-goal 1 0)
    (set-goal 2 0)
    (set-goal 3 0)
)
(defrule
    (player-in-game 1)
    (stance-toward 1 ally)
=>
    (up-modify-goal 1 c:+ 1)
)
(defrule
    (player-in-game 1)
    (stance-toward 1 neutral)
=>
    (up-modify-goal 2 c:+ 1)
)
(defrule
    (player-in-game 1)
    (stance-toward 1 enemy)
=>
    (up-modify-goal 3 c:+ 1)
)
(defrule
    (player-in-game 2)
    (stance-toward 2 ally)
=>
    (up-modify-goal 1 c:+ 1)
)
(defrule
    (player-in-game 2)
    (stance-toward 2 neutral)
=>
    (up-modify-goal 2 c:+ 1)
)
(defrule
    (player-in-game 2)
    (stance-toward 2 enemy)
=>
    (up-modify-goal 3 c:+ 1)
)
(defrule
    (player-in-game 3)
    (stance-toward 3 ally)
=>
    (up-modify-goal 1 c:+ 1)
)
(defrule
    (player-in-game 3)
    (stance-toward 3 neutral)
=>
    (up-modify-goal 2 c:+ 1)
)
(defrule
    (player-in-game 3)
    (stance-toward 3 enemy)
=>
    (up-modify-goal 3 c:+ 1)
)
(defrule
    (player-in-game 4)
    (stance-toward 4 ally)
=>
    (up-modify-goal 1 c:+ 1)
)
(defrule
    (player-in-game 4)
    (stance-toward 4 neutral)
=>
    (up-modify-goal 2 c:+ 1)
)
(defrule
    (player-in-game 4)
    (stance-toward 4 enemy)
=>
    (up-modify-goal 3 c:+ 1)
)
(defrule
    (player-in-game 5)
    (stance-toward 5 ally)
=>
    (up-modify-goal 1 c:+ 1)
)
(defrule
    (player-in-game 5)
    (stance-toward 5 neutral)
=>
    (up-modify-goal 2 c:+ 1)
)
(defrule
    (player-in-game 5)
    (stance-toward 5 enemy)
=>
    (up-modify-goal 3 c:+ 1)
)
(defrule
    (player-in-game 6)
    (stance-toward 6 ally)
=>
    (up-modify-goal 1 c:+ 1)
)
(defrule
    (player-in-game 6)
    (stance-toward 6 neutral)
=>
    (up-modify-goal 2 c:+ 1)
)
(defrule
    (player-in-game 6)
    (stance-toward 6 enemy)
=>
    (up-modify-goal 3 c:+ 1)
)
(defrule
    (player-in-game 7)
    (stance-toward 7 ally)
=>
    (up-modify-goal 1 c:+ 1)
)
(defrule
    (player-in-game 7)
    (stance-toward 7 neutral)
=>
    (up-modify-goal 2 c:+ 1)
)
(defrule
    (player-in-game 7)
    (stance-toward 7 enemy)
=>
    (up-modify-goal 3 c:+ 1)
)
(defrule
    (player-in-game 8)
    (stance-toward 8 ally)
=>
    (up-modify-goal 1 c:+ 1)
)
(defrule
    (player-in-game 8)
    (stance-toward 8 neutral)
=>
    (up-modify-goal 2 c:+ 1)
)
(defrule
    (player-in-game 8)
    (stance-toward 8 enemy)
=>
    (up-modify-goal 3 c:+ 1)
)
(defrule
    (stance-toward my-player-number ally)
=>
    (up-modify-goal 1 c:- 1)
)
(defrule
    (stance-toward my-player-number neutral)
=>
    (up-modify-goal 2 c:- 1)
)
(defrule
    (stance-toward my-player-number enemy)
=>
    (up-modify-goal 3 c:- 1)
)
(defrule
    (random-number == 1)
    (goal 3 0)
    (stance-toward 1 neutral)
    (not
      (players-stance 1 ally)
    )
    (player-in-game 1)
=>
    (set-stance 1 enemy)
    (up-jump-rule 55)
)
(defrule
    (random-number == 2)
    (goal 3 0)
    (stance-toward 2 neutral)
    (not
      (players-stance 2 ally)
    )
    (player-in-game 2)
=>
    (set-stance 2 enemy)
    (up-jump-rule 54)
)
(defrule
    (random-number == 3)
    (goal 3 0)
    (stance-toward 3 neutral)
    (not
      (players-stance 3 ally)
    )
    (player-in-game 3)
=>
    (set-stance 3 enemy)
    (up-jump-rule 53)
)
(defrule
    (random-number == 4)
    (goal 3 0)
    (stance-toward 4 neutral)
    (not
      (players-stance 4 ally)
    )
    (player-in-game 4)
=>
    (set-stance 4 enemy)
    (up-jump-rule 52)
)
(defrule
    (random-number == 5)
    (goal 3 0)
    (stance-toward 5 neutral)
    (not
      (players-stance 5 ally)
    )
    (player-in-game 5)
=>
    (set-stance 5 enemy)
    (up-jump-rule 51)
)
(defrule
    (random-number == 6)
    (goal 3 0)
    (stance-toward 6 neutral)
    (not
      (players-stance 6 ally)
    )
    (player-in-game 6)
=>
    (set-stance 6 enemy)
    (up-jump-rule 50)
)
(defrule
    (random-number == 7)
    (goal 3 0)
    (stance-toward 7 neutral)
    (not
      (players-stance 7 ally)
    )
    (player-in-game 7)
=>
    (set-stance 7 enemy)
    (up-jump-rule 49)
)
(defrule
    (random-number == 8)
    (goal 3 0)
    (stance-toward 8 neutral)
    (not
      (players-stance 8 ally)
    )
    (player-in-game 8)
=>
    (set-stance 8 enemy)
    (up-jump-rule 48)
)
(defrule
    (random-number == 1)
    (goal 3 0)
    (stance-toward 1 neutral)
    (players-stance 1 ally)
    (player-in-game 1)
=>
    (set-stance 1 enemy)
    (up-jump-rule 47)
)
(defrule
    (random-number == 2)
    (goal 3 0)
    (stance-toward 2 neutral)
    (players-stance 2 ally)
    (player-in-game 2)
=>
    (set-stance 2 enemy)
    (up-jump-rule 46)
)
(defrule
    (random-number == 3)
    (goal 3 0)
    (stance-toward 3 neutral)
    (players-stance 3 ally)
    (player-in-game 3)
=>
    (set-stance 3 enemy)
    (up-jump-rule 45)
)
(defrule
    (random-number == 4)
    (goal 3 0)
    (stance-toward 4 neutral)
    (players-stance 4 ally)
    (player-in-game 4)
=>
    (set-stance 4 enemy)
    (up-jump-rule 44)
)
(defrule
    (random-number == 5)
    (goal 3 0)
    (stance-toward 5 neutral)
    (players-stance 5 ally)
    (player-in-game 5)
=>
    (set-stance 5 enemy)
    (up-jump-rule 43)
)
(defrule
    (random-number == 6)
    (goal 3 0)
    (stance-toward 6 neutral)
    (players-stance 6 ally)
    (player-in-game 6)
=>
    (set-stance 6 enemy)
    (up-jump-rule 42)
)
(defrule
    (random-number == 7)
    (goal 3 0)
    (stance-toward 7 neutral)
    (players-stance 7 ally)
    (player-in-game 7)
=>
    (set-stance 7 enemy)
    (up-jump-rule 41)
)
(defrule
    (random-number == 8)
    (goal 3 0)
    (stance-toward 8 neutral)
    (players-stance 8 ally)
    (player-in-game 8)
=>
    (set-stance 8 enemy)
    (up-jump-rule 40)
)
(defrule
    (random-number == 1)
    (goal 3 0)
    (goal 2 0)
    (player-in-game 1)
=>
    (set-stance 1 enemy)
    (up-jump-rule 39)
)
(defrule
    (random-number == 2)
    (goal 3 0)
    (goal 2 0)
    (player-in-game 2)
=>
    (set-stance 2 enemy)
    (up-jump-rule 38)
)
(defrule
    (random-number == 3)
    (goal 3 0)
    (goal 2 0)
    (player-in-game 3)
=>
    (set-stance 3 enemy)
    (up-jump-rule 37)
)
(defrule
    (random-number == 4)
    (goal 3 0)
    (goal 2 0)
    (player-in-game 4)
=>
    (set-stance 4 enemy)
    (up-jump-rule 36)
)
(defrule
    (random-number == 5)
    (goal 3 0)
    (goal 2 0)
    (player-in-game 5)
=>
    (set-stance 5 enemy)
    (up-jump-rule 35)
)
(defrule
    (random-number == 6)
    (goal 3 0)
    (goal 2 0)
    (player-in-game 6)
=>
    (set-stance 6 enemy)
    (up-jump-rule 34)
)
(defrule
    (random-number == 7)
    (goal 3 0)
    (goal 2 0)
    (player-in-game 7)
=>
    (set-stance 7 enemy)
    (up-jump-rule 33)
)
(defrule
    (random-number == 8)
    (goal 3 0)
    (goal 2 0)
    (player-in-game 8)
=>
    (set-stance 8 enemy)
    (up-jump-rule 32)
)
(defrule
    (random-number == 1)
    (not
      (goal 3 0)
    )
    (stance-toward 1 neutral)
    (not
      (players-stance 1 enemy)
    )
    (player-in-game 1)
=>
    (set-stance 1 ally)
    (up-jump-rule 31)
)
(defrule
    (random-number == 2)
    (not
      (goal 3 0)
    )
    (stance-toward 2 neutral)
    (not
      (players-stance 2 enemy)
    )
    (player-in-game 2)
=>
    (set-stance 2 ally)
    (up-jump-rule 30)
)
(defrule
    (random-number == 3)
    (not
      (goal 3 0)
    )
    (stance-toward 3 neutral)
    (not
      (players-stance 3 enemy)
    )
    (player-in-game 3)
=>
    (set-stance 3 ally)
    (up-jump-rule 29)
)
(defrule
    (random-number == 4)
    (not
      (goal 3 0)
    )
    (stance-toward 4 neutral)
    (not
      (players-stance 4 enemy)
    )
    (player-in-game 4)
=>
    (set-stance 4 ally)
    (up-jump-rule 28)
)
(defrule
    (random-number == 5)
    (not
      (goal 3 0)
    )
    (stance-toward 5 neutral)
    (not
      (players-stance 5 enemy)
    )
    (player-in-game 5)
=>
    (set-stance 5 ally)
    (up-jump-rule 27)
)
(defrule
    (random-number == 6)
    (not
      (goal 3 0)
    )
    (stance-toward 6 neutral)
    (not
      (players-stance 6 enemy)
    )
    (player-in-game 6)
=>
    (set-stance 6 ally)
    (up-jump-rule 26)
)
(defrule
    (random-number == 7)
    (not
      (goal 3 0)
    )
    (stance-toward 7 neutral)
    (not
      (players-stance 7 enemy)
    )
    (player-in-game 7)
=>
    (set-stance 7 ally)
    (up-jump-rule 25)
)
(defrule
    (random-number == 8)
    (not
      (goal 3 0)
    )
    (stance-toward 8 neutral)
    (not
      (players-stance 8 enemy)
    )
    (player-in-game 8)
=>
    (set-stance 8 ally)
    (up-jump-rule 24)
)
(defrule
    (random-number == 1)
    (up-compare-goal 3 >= 2)
    (stance-toward 1 enemy)
    (players-stance 1 enemy)
    (player-in-game 1)
=>
    (set-stance 1 ally)
    (disable-self)
    (up-jump-rule 23)
)
(defrule
    (random-number == 2)
    (up-compare-goal 3 >= 2)
    (stance-toward 2 enemy)
    (players-stance 2 enemy)
    (player-in-game 2)
=>
    (set-stance 2 ally)
    (disable-self)
    (up-jump-rule 22)
)
(defrule
    (random-number == 3)
    (up-compare-goal 3 >= 2)
    (stance-toward 3 enemy)
    (players-stance 3 enemy)
    (player-in-game 3)
=>
    (set-stance 3 ally)
    (disable-self)
    (up-jump-rule 21)
)
(defrule
    (random-number == 4)
    (up-compare-goal 3 >= 2)
    (stance-toward 4 enemy)
    (players-stance 4 enemy)
    (player-in-game 4)
=>
    (set-stance 4 ally)
    (disable-self)
    (up-jump-rule 20)
)
(defrule
    (random-number == 5)
    (up-compare-goal 3 >= 2)
    (stance-toward 5 enemy)
    (players-stance 5 enemy)
    (player-in-game 5)
=>
    (set-stance 5 ally)
    (disable-self)
    (up-jump-rule 19)
)
(defrule
    (random-number == 6)
    (up-compare-goal 3 >= 2)
    (stance-toward 6 enemy)
    (players-stance 6 enemy)
    (player-in-game 6)
=>
    (set-stance 6 ally)
    (disable-self)
    (up-jump-rule 18)
)
(defrule
    (random-number == 7)
    (up-compare-goal 3 >= 2)
    (stance-toward 7 enemy)
    (players-stance 7 enemy)
    (player-in-game 7)
=>
    (set-stance 7 ally)
    (disable-self)
    (up-jump-rule 17)
)
(defrule
    (random-number == 8)
    (up-compare-goal 3 >= 2)
    (stance-toward 8 enemy)
    (players-stance 8 enemy)
    (player-in-game 8)
=>
    (set-stance 8 ally)
    (disable-self)
    (up-jump-rule 16)
)
(defrule
    (random-number == 1)
    (up-compare-goal 3 >= 2)
    (not
      (stance-toward 1 ally)
    )
    (players-stance 1 ally)
    (player-in-game 1)
=>
    (set-stance 1 ally)
    (up-jump-rule 15)
)
(defrule
    (random-number == 2)
    (up-compare-goal 3 >= 2)
    (not
      (stance-toward 2 ally)
    )
    (players-stance 2 ally)
    (player-in-game 2)
=>
    (set-stance 2 ally)
    (up-jump-rule 14)
)
(defrule
    (random-number == 3)
    (up-compare-goal 3 >= 2)
    (not
      (stance-toward 3 ally)
    )
    (players-stance 3 ally)
    (player-in-game 3)
=>
    (set-stance 3 ally)
    (up-jump-rule 13)
)
(defrule
    (random-number == 4)
    (up-compare-goal 3 >= 2)
    (not
      (stance-toward 4 ally)
    )
    (players-stance 4 ally)
    (player-in-game 4)
=>
    (set-stance 4 ally)
    (up-jump-rule 12)
)
(defrule
    (random-number == 5)
    (up-compare-goal 3 >= 2)
    (not
      (stance-toward 5 ally)
    )
    (players-stance 5 ally)
    (player-in-game 5)
=>
    (set-stance 5 ally)
    (up-jump-rule 11)
)
(defrule
    (random-number == 6)
    (up-compare-goal 3 >= 2)
    (not
      (stance-toward 6 ally)
    )
    (players-stance 6 ally)
    (player-in-game 6)
=>
    (set-stance 6 ally)
    (up-jump-rule 10)
)
(defrule
    (random-number == 7)
    (up-compare-goal 3 >= 2)
    (not
      (stance-toward 7 ally)
    )
    (players-stance 7 ally)
    (player-in-game 7)
=>
    (set-stance 7 ally)
    (up-jump-rule 9)
)
(defrule
    (random-number == 8)
    (up-compare-goal 3 >= 2)
    (not
      (stance-toward 8 ally)
    )
    (players-stance 8 ally)
    (player-in-game 8)
=>
    (set-stance 8 ally)
    (up-jump-rule 8)
)
(defrule
    (random-number == 1)
    (stance-toward 1 neutral)
    (players-stance 1 enemy)
    (player-in-game 1)
=>
    (set-stance 1 enemy)
    (up-jump-rule 7)
)
(defrule
    (random-number == 2)
    (stance-toward 2 neutral)
    (players-stance 2 enemy)
    (player-in-game 2)
=>
    (set-stance 2 enemy)
    (up-jump-rule 6)
)
(defrule
    (random-number == 3)
    (stance-toward 3 neutral)
    (players-stance 3 enemy)
    (player-in-game 3)
=>
    (set-stance 3 enemy)
    (up-jump-rule 5)
)
(defrule
    (random-number == 4)
    (stance-toward 4 neutral)
    (players-stance 4 enemy)
    (player-in-game 4)
=>
    (set-stance 4 enemy)
    (up-jump-rule 4)
)
(defrule
    (random-number == 5)
    (stance-toward 5 neutral)
    (players-stance 5 enemy)
    (player-in-game 5)
=>
    (set-stance 5 enemy)
    (up-jump-rule 3)
)
(defrule
    (random-number == 6)
    (stance-toward 6 neutral)
    (players-stance 6 enemy)
    (player-in-game 6)
=>
    (set-stance 6 enemy)
    (up-jump-rule 2)
)
(defrule
    (random-number == 7)
    (stance-toward 7 neutral)
    (players-stance 7 enemy)
    (player-in-game 7)
=>
    (set-stance 7 enemy)
    (up-jump-rule 1)
)
(defrule
    (random-number == 8)
    (stance-toward 8 neutral)
    (players-stance 8 enemy)
    (player-in-game 8)
=>
    (set-stance 8 enemy)
)
(defrule
    (game-time >= 1200)
    (population < 10)
=>
    (set-stance every-enemy ally)
    (set-stance every-neutral ally)
    (resign)
)
(defrule
    (players-stance 1 ally)
    (not
      (stance-toward 1 ally)
    )
    (not
      (player-in-game 1)
    )
=>
    (set-stance 1 ally)
    (up-reset-unit c: -1)
)
(defrule
    (players-stance 2 ally)
    (not
      (stance-toward 2 ally)
    )
    (not
      (player-in-game 2)
    )
=>
    (set-stance 2 ally)
    (up-reset-unit c: -1)
)
(defrule
    (players-stance 3 ally)
    (not
      (stance-toward 3 ally)
    )
    (not
      (player-in-game 3)
    )
=>
    (set-stance 3 ally)
    (up-reset-unit c: -1)
)
(defrule
    (players-stance 4 ally)
    (not
      (stance-toward 4 ally)
    )
    (not
      (player-in-game 4)
    )
=>
    (set-stance 4 ally)
    (up-reset-unit c: -1)
)
(defrule
    (players-stance 5 ally)
    (not
      (stance-toward 5 ally)
    )
    (not
      (player-in-game 5)
    )
=>
    (set-stance 5 ally)
    (up-reset-unit c: -1)
)
(defrule
    (players-stance 6 ally)
    (not
      (stance-toward 6 ally)
    )
    (not
      (player-in-game 6)
    )
=>
    (set-stance 6 ally)
    (up-reset-unit c: -1)
)
(defrule
    (players-stance 7 ally)
    (not
      (stance-toward 7 ally)
    )
    (not
      (player-in-game 7)
    )
=>
    (set-stance 7 ally)
    (up-reset-unit c: -1)
)
(defrule
    (players-stance 8 ally)
    (not
      (stance-toward 8 ally)
    )
    (not
      (player-in-game 8)
    )
=>
    (set-stance 8 ally)
    (up-reset-unit c: -1)
)
(defrule
    (true)
=>
    (up-get-threat-data 3 2 -1 -1)
)
(defrule
    (goal 2 1)
    (up-compare-goal 3 c:< 1000)
    (not
      (stance-toward 1 enemy)
    )
=>
    (set-stance 1 enemy)
)
(defrule
    (goal 2 2)
    (up-compare-goal 3 c:< 1000)
    (not
      (stance-toward 2 enemy)
    )
=>
    (set-stance 2 enemy)
)
(defrule
    (goal 2 3)
    (up-compare-goal 3 c:< 1000)
    (not
      (stance-toward 3 enemy)
    )
=>
    (set-stance 3 enemy)
)
(defrule
    (goal 2 4)
    (up-compare-goal 3 c:< 1000)
    (not
      (stance-toward 4 enemy)
    )
=>
    (set-stance 4 enemy)
)
(defrule
    (goal 2 5)
    (up-compare-goal 3 c:< 1000)
    (not
      (stance-toward 5 enemy)
    )
=>
    (set-stance 5 enemy)
)
(defrule
    (goal 2 6)
    (up-compare-goal 3 c:< 1000)
    (not
      (stance-toward 6 enemy)
    )
=>
    (set-stance 6 enemy)
)
(defrule
    (goal 2 7)
    (up-compare-goal 3 c:< 1000)
    (not
      (stance-toward 7 enemy)
    )
=>
    (set-stance 7 enemy)
)
(defrule
    (goal 2 8)
    (up-compare-goal 3 c:< 1000)
    (not
      (stance-toward 8 enemy)
    )
=>
    (set-stance 8 enemy)
)
(defrule
    (true)
=>
    (set-stance my-player-number ally)
)

```
---
```
do basic diplomacy without backstabbing
```
```
(defrule
    (true)
=>
    (set-stance every-enemy neutral)
    (disable-self)
)
(defrule
    (true)
=>
    (generate-random-number 8)
    (set-goal 1 0)
    (set-goal 2 0)
    (set-goal 3 0)
)
(defrule
    (player-in-game 1)
    (stance-toward 1 ally)
=>
    (up-modify-goal 1 c:+ 1)
)
(defrule
    (player-in-game 1)
    (stance-toward 1 neutral)
=>
    (up-modify-goal 2 c:+ 1)
)
(defrule
    (player-in-game 1)
    (stance-toward 1 enemy)
=>
    (up-modify-goal 3 c:+ 1)
)
(defrule
    (player-in-game 2)
    (stance-toward 2 ally)
=>
    (up-modify-goal 1 c:+ 1)
)
(defrule
    (player-in-game 2)
    (stance-toward 2 neutral)
=>
    (up-modify-goal 2 c:+ 1)
)
(defrule
    (player-in-game 2)
    (stance-toward 2 enemy)
=>
    (up-modify-goal 3 c:+ 1)
)
(defrule
    (player-in-game 3)
    (stance-toward 3 ally)
=>
    (up-modify-goal 1 c:+ 1)
)
(defrule
    (player-in-game 3)
    (stance-toward 3 neutral)
=>
    (up-modify-goal 2 c:+ 1)
)
(defrule
    (player-in-game 3)
    (stance-toward 3 enemy)
=>
    (up-modify-goal 3 c:+ 1)
)
(defrule
    (player-in-game 4)
    (stance-toward 4 ally)
=>
    (up-modify-goal 1 c:+ 1)
)
(defrule
    (player-in-game 4)
    (stance-toward 4 neutral)
=>
    (up-modify-goal 2 c:+ 1)
)
(defrule
    (player-in-game 4)
    (stance-toward 4 enemy)
=>
    (up-modify-goal 3 c:+ 1)
)
(defrule
    (player-in-game 5)
    (stance-toward 5 ally)
=>
    (up-modify-goal 1 c:+ 1)
)
(defrule
    (player-in-game 5)
    (stance-toward 5 neutral)
=>
    (up-modify-goal 2 c:+ 1)
)
(defrule
    (player-in-game 5)
    (stance-toward 5 enemy)
=>
    (up-modify-goal 3 c:+ 1)
)
(defrule
    (player-in-game 6)
    (stance-toward 6 ally)
=>
    (up-modify-goal 1 c:+ 1)
)
(defrule
    (player-in-game 6)
    (stance-toward 6 neutral)
=>
    (up-modify-goal 2 c:+ 1)
)
(defrule
    (player-in-game 6)
    (stance-toward 6 enemy)
=>
    (up-modify-goal 3 c:+ 1)
)
(defrule
    (player-in-game 7)
    (stance-toward 7 ally)
=>
    (up-modify-goal 1 c:+ 1)
)
(defrule
    (player-in-game 7)
    (stance-toward 7 neutral)
=>
    (up-modify-goal 2 c:+ 1)
)
(defrule
    (player-in-game 7)
    (stance-toward 7 enemy)
=>
    (up-modify-goal 3 c:+ 1)
)
(defrule
    (player-in-game 8)
    (stance-toward 8 ally)
=>
    (up-modify-goal 1 c:+ 1)
)
(defrule
    (player-in-game 8)
    (stance-toward 8 neutral)
=>
    (up-modify-goal 2 c:+ 1)
)
(defrule
    (player-in-game 8)
    (stance-toward 8 enemy)
=>
    (up-modify-goal 3 c:+ 1)
)
(defrule
    (stance-toward my-player-number ally)
=>
    (up-modify-goal 1 c:- 1)
)
(defrule
    (stance-toward my-player-number neutral)
=>
    (up-modify-goal 2 c:- 1)
)
(defrule
    (stance-toward my-player-number enemy)
=>
    (up-modify-goal 3 c:- 1)
)
(defrule
    (random-number == 1)
    (goal 3 0)
    (stance-toward 1 neutral)
    (not
      (players-stance 1 ally)
    )
    (player-in-game 1)
=>
    (set-stance 1 enemy)
    (up-jump-rule 55)
)
(defrule
    (random-number == 2)
    (goal 3 0)
    (stance-toward 2 neutral)
    (not
      (players-stance 2 ally)
    )
    (player-in-game 2)
=>
    (set-stance 2 enemy)
    (up-jump-rule 54)
)
(defrule
    (random-number == 3)
    (goal 3 0)
    (stance-toward 3 neutral)
    (not
      (players-stance 3 ally)
    )
    (player-in-game 3)
=>
    (set-stance 3 enemy)
    (up-jump-rule 53)
)
(defrule
    (random-number == 4)
    (goal 3 0)
    (stance-toward 4 neutral)
    (not
      (players-stance 4 ally)
    )
    (player-in-game 4)
=>
    (set-stance 4 enemy)
    (up-jump-rule 52)
)
(defrule
    (random-number == 5)
    (goal 3 0)
    (stance-toward 5 neutral)
    (not
      (players-stance 5 ally)
    )
    (player-in-game 5)
=>
    (set-stance 5 enemy)
    (up-jump-rule 51)
)
(defrule
    (random-number == 6)
    (goal 3 0)
    (stance-toward 6 neutral)
    (not
      (players-stance 6 ally)
    )
    (player-in-game 6)
=>
    (set-stance 6 enemy)
    (up-jump-rule 50)
)
(defrule
    (random-number == 7)
    (goal 3 0)
    (stance-toward 7 neutral)
    (not
      (players-stance 7 ally)
    )
    (player-in-game 7)
=>
    (set-stance 7 enemy)
    (up-jump-rule 49)
)
(defrule
    (random-number == 8)
    (goal 3 0)
    (stance-toward 8 neutral)
    (not
      (players-stance 8 ally)
    )
    (player-in-game 8)
=>
    (set-stance 8 enemy)
    (up-jump-rule 48)
)
(defrule
    (random-number == 1)
    (goal 3 0)
    (stance-toward 1 neutral)
    (players-stance 1 ally)
    (player-in-game 1)
=>
    (set-stance 1 enemy)
    (up-jump-rule 47)
)
(defrule
    (random-number == 2)
    (goal 3 0)
    (stance-toward 2 neutral)
    (players-stance 2 ally)
    (player-in-game 2)
=>
    (set-stance 2 enemy)
    (up-jump-rule 46)
)
(defrule
    (random-number == 3)
    (goal 3 0)
    (stance-toward 3 neutral)
    (players-stance 3 ally)
    (player-in-game 3)
=>
    (set-stance 3 enemy)
    (up-jump-rule 45)
)
(defrule
    (random-number == 4)
    (goal 3 0)
    (stance-toward 4 neutral)
    (players-stance 4 ally)
    (player-in-game 4)
=>
    (set-stance 4 enemy)
    (up-jump-rule 44)
)
(defrule
    (random-number == 5)
    (goal 3 0)
    (stance-toward 5 neutral)
    (players-stance 5 ally)
    (player-in-game 5)
=>
    (set-stance 5 enemy)
    (up-jump-rule 43)
)
(defrule
    (random-number == 6)
    (goal 3 0)
    (stance-toward 6 neutral)
    (players-stance 6 ally)
    (player-in-game 6)
=>
    (set-stance 6 enemy)
    (up-jump-rule 42)
)
(defrule
    (random-number == 7)
    (goal 3 0)
    (stance-toward 7 neutral)
    (players-stance 7 ally)
    (player-in-game 7)
=>
    (set-stance 7 enemy)
    (up-jump-rule 41)
)
(defrule
    (random-number == 8)
    (goal 3 0)
    (stance-toward 8 neutral)
    (players-stance 8 ally)
    (player-in-game 8)
=>
    (set-stance 8 enemy)
    (up-jump-rule 40)
)
(defrule
    (random-number == 1)
    (goal 3 0)
    (goal 2 0)
    (up-compare-goal 1 >= 2)
    (player-in-game 1)
=>
    (set-stance 1 enemy)
    (up-jump-rule 39)
)
(defrule
    (random-number == 2)
    (goal 3 0)
    (goal 2 0)
    (up-compare-goal 1 >= 2)
    (player-in-game 2)
=>
    (set-stance 2 enemy)
    (up-jump-rule 38)
)
(defrule
    (random-number == 3)
    (goal 3 0)
    (goal 2 0)
    (up-compare-goal 1 >= 2)
    (player-in-game 3)
=>
    (set-stance 3 enemy)
    (up-jump-rule 37)
)
(defrule
    (random-number == 4)
    (goal 3 0)
    (goal 2 0)
    (up-compare-goal 1 >= 2)
    (player-in-game 4)
=>
    (set-stance 4 enemy)
    (up-jump-rule 36)
)
(defrule
    (random-number == 5)
    (goal 3 0)
    (goal 2 0)
    (up-compare-goal 1 >= 2)
    (player-in-game 5)
=>
    (set-stance 5 enemy)
    (up-jump-rule 35)
)
(defrule
    (random-number == 6)
    (goal 3 0)
    (goal 2 0)
    (up-compare-goal 1 >= 2)
    (player-in-game 6)
=>
    (set-stance 6 enemy)
    (up-jump-rule 34)
)
(defrule
    (random-number == 7)
    (goal 3 0)
    (goal 2 0)
    (up-compare-goal 1 >= 2)
    (player-in-game 7)
=>
    (set-stance 7 enemy)
    (up-jump-rule 33)
)
(defrule
    (random-number == 8)
    (goal 3 0)
    (goal 2 0)
    (up-compare-goal 1 >= 2)
    (player-in-game 8)
=>
    (set-stance 8 enemy)
    (up-jump-rule 32)
)
(defrule
    (random-number == 1)
    (not
      (goal 3 0)
    )
    (stance-toward 1 neutral)
    (not
      (players-stance 1 enemy)
    )
    (player-in-game 1)
=>
    (set-stance 1 ally)
    (up-jump-rule 31)
)
(defrule
    (random-number == 2)
    (not
      (goal 3 0)
    )
    (stance-toward 2 neutral)
    (not
      (players-stance 2 enemy)
    )
    (player-in-game 2)
=>
    (set-stance 2 ally)
    (up-jump-rule 30)
)
(defrule
    (random-number == 3)
    (not
      (goal 3 0)
    )
    (stance-toward 3 neutral)
    (not
      (players-stance 3 enemy)
    )
    (player-in-game 3)
=>
    (set-stance 3 ally)
    (up-jump-rule 29)
)
(defrule
    (random-number == 4)
    (not
      (goal 3 0)
    )
    (stance-toward 4 neutral)
    (not
      (players-stance 4 enemy)
    )
    (player-in-game 4)
=>
    (set-stance 4 ally)
    (up-jump-rule 28)
)
(defrule
    (random-number == 5)
    (not
      (goal 3 0)
    )
    (stance-toward 5 neutral)
    (not
      (players-stance 5 enemy)
    )
    (player-in-game 5)
=>
    (set-stance 5 ally)
    (up-jump-rule 27)
)
(defrule
    (random-number == 6)
    (not
      (goal 3 0)
    )
    (stance-toward 6 neutral)
    (not
      (players-stance 6 enemy)
    )
    (player-in-game 6)
=>
    (set-stance 6 ally)
    (up-jump-rule 26)
)
(defrule
    (random-number == 7)
    (not
      (goal 3 0)
    )
    (stance-toward 7 neutral)
    (not
      (players-stance 7 enemy)
    )
    (player-in-game 7)
=>
    (set-stance 7 ally)
    (up-jump-rule 25)
)
(defrule
    (random-number == 8)
    (not
      (goal 3 0)
    )
    (stance-toward 8 neutral)
    (not
      (players-stance 8 enemy)
    )
    (player-in-game 8)
=>
    (set-stance 8 ally)
    (up-jump-rule 24)
)
(defrule
    (random-number == 1)
    (up-compare-goal 3 >= 2)
    (stance-toward 1 enemy)
    (players-stance 1 enemy)
    (player-in-game 1)
=>
    (set-stance 1 ally)
    (disable-self)
    (up-jump-rule 23)
)
(defrule
    (random-number == 2)
    (up-compare-goal 3 >= 2)
    (stance-toward 2 enemy)
    (players-stance 2 enemy)
    (player-in-game 2)
=>
    (set-stance 2 ally)
    (disable-self)
    (up-jump-rule 22)
)
(defrule
    (random-number == 3)
    (up-compare-goal 3 >= 2)
    (stance-toward 3 enemy)
    (players-stance 3 enemy)
    (player-in-game 3)
=>
    (set-stance 3 ally)
    (disable-self)
    (up-jump-rule 21)
)
(defrule
    (random-number == 4)
    (up-compare-goal 3 >= 2)
    (stance-toward 4 enemy)
    (players-stance 4 enemy)
    (player-in-game 4)
=>
    (set-stance 4 ally)
    (disable-self)
    (up-jump-rule 20)
)
(defrule
    (random-number == 5)
    (up-compare-goal 3 >= 2)
    (stance-toward 5 enemy)
    (players-stance 5 enemy)
    (player-in-game 5)
=>
    (set-stance 5 ally)
    (disable-self)
    (up-jump-rule 19)
)
(defrule
    (random-number == 6)
    (up-compare-goal 3 >= 2)
    (stance-toward 6 enemy)
    (players-stance 6 enemy)
    (player-in-game 6)
=>
    (set-stance 6 ally)
    (disable-self)
    (up-jump-rule 18)
)
(defrule
    (random-number == 7)
    (up-compare-goal 3 >= 2)
    (stance-toward 7 enemy)
    (players-stance 7 enemy)
    (player-in-game 7)
=>
    (set-stance 7 ally)
    (disable-self)
    (up-jump-rule 17)
)
(defrule
    (random-number == 8)
    (up-compare-goal 3 >= 2)
    (stance-toward 8 enemy)
    (players-stance 8 enemy)
    (player-in-game 8)
=>
    (set-stance 8 ally)
    (disable-self)
    (up-jump-rule 16)
)
(defrule
    (random-number == 1)
    (up-compare-goal 3 >= 2)
    (not
      (stance-toward 1 ally)
    )
    (players-stance 1 ally)
    (player-in-game 1)
=>
    (set-stance 1 ally)
    (up-jump-rule 15)
)
(defrule
    (random-number == 2)
    (up-compare-goal 3 >= 2)
    (not
      (stance-toward 2 ally)
    )
    (players-stance 2 ally)
    (player-in-game 2)
=>
    (set-stance 2 ally)
    (up-jump-rule 14)
)
(defrule
    (random-number == 3)
    (up-compare-goal 3 >= 2)
    (not
      (stance-toward 3 ally)
    )
    (players-stance 3 ally)
    (player-in-game 3)
=>
    (set-stance 3 ally)
    (up-jump-rule 13)
)
(defrule
    (random-number == 4)
    (up-compare-goal 3 >= 2)
    (not
      (stance-toward 4 ally)
    )
    (players-stance 4 ally)
    (player-in-game 4)
=>
    (set-stance 4 ally)
    (up-jump-rule 12)
)
(defrule
    (random-number == 5)
    (up-compare-goal 3 >= 2)
    (not
      (stance-toward 5 ally)
    )
    (players-stance 5 ally)
    (player-in-game 5)
=>
    (set-stance 5 ally)
    (up-jump-rule 11)
)
(defrule
    (random-number == 6)
    (up-compare-goal 3 >= 2)
    (not
      (stance-toward 6 ally)
    )
    (players-stance 6 ally)
    (player-in-game 6)
=>
    (set-stance 6 ally)
    (up-jump-rule 10)
)
(defrule
    (random-number == 7)
    (up-compare-goal 3 >= 2)
    (not
      (stance-toward 7 ally)
    )
    (players-stance 7 ally)
    (player-in-game 7)
=>
    (set-stance 7 ally)
    (up-jump-rule 9)
)
(defrule
    (random-number == 8)
    (up-compare-goal 3 >= 2)
    (not
      (stance-toward 8 ally)
    )
    (players-stance 8 ally)
    (player-in-game 8)
=>
    (set-stance 8 ally)
    (up-jump-rule 8)
)
(defrule
    (random-number == 1)
    (stance-toward 1 neutral)
    (players-stance 1 enemy)
    (player-in-game 1)
=>
    (set-stance 1 enemy)
    (up-jump-rule 7)
)
(defrule
    (random-number == 2)
    (stance-toward 2 neutral)
    (players-stance 2 enemy)
    (player-in-game 2)
=>
    (set-stance 2 enemy)
    (up-jump-rule 6)
)
(defrule
    (random-number == 3)
    (stance-toward 3 neutral)
    (players-stance 3 enemy)
    (player-in-game 3)
=>
    (set-stance 3 enemy)
    (up-jump-rule 5)
)
(defrule
    (random-number == 4)
    (stance-toward 4 neutral)
    (players-stance 4 enemy)
    (player-in-game 4)
=>
    (set-stance 4 enemy)
    (up-jump-rule 4)
)
(defrule
    (random-number == 5)
    (stance-toward 5 neutral)
    (players-stance 5 enemy)
    (player-in-game 5)
=>
    (set-stance 5 enemy)
    (up-jump-rule 3)
)
(defrule
    (random-number == 6)
    (stance-toward 6 neutral)
    (players-stance 6 enemy)
    (player-in-game 6)
=>
    (set-stance 6 enemy)
    (up-jump-rule 2)
)
(defrule
    (random-number == 7)
    (stance-toward 7 neutral)
    (players-stance 7 enemy)
    (player-in-game 7)
=>
    (set-stance 7 enemy)
    (up-jump-rule 1)
)
(defrule
    (random-number == 8)
    (stance-toward 8 neutral)
    (players-stance 8 enemy)
    (player-in-game 8)
=>
    (set-stance 8 enemy)
)
(defrule
    (game-time >= 1200)
    (population < 10)
=>
    (set-stance every-enemy ally)
    (set-stance every-neutral ally)
    (resign)
)
(defrule
    (players-stance 1 ally)
    (not
      (stance-toward 1 ally)
    )
    (not
      (player-in-game 1)
    )
=>
    (set-stance 1 ally)
    (up-reset-unit c: -1)
)
(defrule
    (players-stance 2 ally)
    (not
      (stance-toward 2 ally)
    )
    (not
      (player-in-game 2)
    )
=>
    (set-stance 2 ally)
    (up-reset-unit c: -1)
)
(defrule
    (players-stance 3 ally)
    (not
      (stance-toward 3 ally)
    )
    (not
      (player-in-game 3)
    )
=>
    (set-stance 3 ally)
    (up-reset-unit c: -1)
)
(defrule
    (players-stance 4 ally)
    (not
      (stance-toward 4 ally)
    )
    (not
      (player-in-game 4)
    )
=>
    (set-stance 4 ally)
    (up-reset-unit c: -1)
)
(defrule
    (players-stance 5 ally)
    (not
      (stance-toward 5 ally)
    )
    (not
      (player-in-game 5)
    )
=>
    (set-stance 5 ally)
    (up-reset-unit c: -1)
)
(defrule
    (players-stance 6 ally)
    (not
      (stance-toward 6 ally)
    )
    (not
      (player-in-game 6)
    )
=>
    (set-stance 6 ally)
    (up-reset-unit c: -1)
)
(defrule
    (players-stance 7 ally)
    (not
      (stance-toward 7 ally)
    )
    (not
      (player-in-game 7)
    )
=>
    (set-stance 7 ally)
    (up-reset-unit c: -1)
)
(defrule
    (players-stance 8 ally)
    (not
      (stance-toward 8 ally)
    )
    (not
      (player-in-game 8)
    )
=>
    (set-stance 8 ally)
    (up-reset-unit c: -1)
)
(defrule
    (true)
=>
    (up-get-threat-data 3 2 -1 -1)
)
(defrule
    (goal 2 1)
    (up-compare-goal 3 c:< 1000)
    (not
      (stance-toward 1 enemy)
    )
=>
    (set-stance 1 enemy)
)
(defrule
    (goal 2 2)
    (up-compare-goal 3 c:< 1000)
    (not
      (stance-toward 2 enemy)
    )
=>
    (set-stance 2 enemy)
)
(defrule
    (goal 2 3)
    (up-compare-goal 3 c:< 1000)
    (not
      (stance-toward 3 enemy)
    )
=>
    (set-stance 3 enemy)
)
(defrule
    (goal 2 4)
    (up-compare-goal 3 c:< 1000)
    (not
      (stance-toward 4 enemy)
    )
=>
    (set-stance 4 enemy)
)
(defrule
    (goal 2 5)
    (up-compare-goal 3 c:< 1000)
    (not
      (stance-toward 5 enemy)
    )
=>
    (set-stance 5 enemy)
)
(defrule
    (goal 2 6)
    (up-compare-goal 3 c:< 1000)
    (not
      (stance-toward 6 enemy)
    )
=>
    (set-stance 6 enemy)
)
(defrule
    (goal 2 7)
    (up-compare-goal 3 c:< 1000)
    (not
      (stance-toward 7 enemy)
    )
=>
    (set-stance 7 enemy)
)
(defrule
    (goal 2 8)
    (up-compare-goal 3 c:< 1000)
    (not
      (stance-toward 8 enemy)
    )
=>
    (set-stance 8 enemy)
)
(defrule
    (true)
=>
    (set-stance my-player-number ally)
)

```
---
