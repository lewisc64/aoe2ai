import aoeai

print(aoeai.translate("""
#when
  research ri-arbalest
#then
  #select random
    chat to all "Precision engineering."
  #randor
    chat to all "Apex predator."
  #randor
    chat to all "Perforating, machinating..."
  #end select
#end when
"""))
