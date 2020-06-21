import aoeai
from aoeai import Defrule

print(aoeai.translate("""
#while not(enemy-buildings-in-town) and strategic-number sn-maximum-town-size < 255
  sn-maximum-town-size += 1
#end while
"""))
