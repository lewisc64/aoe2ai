import aoeai

print(aoeai.interpret("""

#if game-time < 50
    chat to all \"yo\"
#else if game-time < 100
    chat to all \"yoyo\"
#else if game-time < 150
    chat to all \"yoyoyo\"
#end if

"""))
