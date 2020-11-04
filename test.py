import aoeai
from aoeai import Defrule

print(aoeai.translate("""
#if gold-amount < 1500
  sell wood when wood > 1500
  sell food when food > 1500
#else
  buy food when food < 1500
  buy wood when wood < 1500
#end if
"""))
