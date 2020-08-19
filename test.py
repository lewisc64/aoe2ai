import aoeai
from aoeai import Defrule

print(aoeai.translate("""
const wood-threshold = 300
const food-threshold = 300
const gold-threshold = 300

#if up-compare-goal non-escrowed-wood > wood-threshold
  #if up-compare-goal non-escrowed-food > food-threshold
    #if up-compare-goal non-escrowed-gold > gold-threshold
      // do nothing, all above thresholds.
    #else
      distribute 4 villagers from wood and food to gold
    #end if
  #else
    #if up-compare-goal non-escrowed-gold > gold-threshold
      distribute 4 villagers from wood and gold to food
    #else
      distribute 4 villagers from wood to food and gold
    #end if
  #end if
#else
  #if up-compare-goal non-escrowed-food > food-threshold
    #if up-compare-goal non-escrowed-gold > gold-threshold
      distribute 4 villagers from food and gold to wood
    #else
      distribute 4 villagers from food to wood and gold
    #end if
  #else
    #if up-compare-goal non-escrowed-gold > gold-threshold
      distribute 4 villagers from gold to wood and food
    #else
      // do nothing, all below thresholds.
    #end if
  #end if
#end if
"""))
