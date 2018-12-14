import aoeai

print(aoeai.translate("""

#add condition town-under-attack
    #add condition unit-type-count-total militiaman-line > 0 or unit-type-count-total spearman-line > 0
        load "Patient.aoe2ai"
    #remove condition
#remove condition

"""))
