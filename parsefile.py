import aoeai
import sys

input_path, output_path, name = sys.argv[1:]


file = open(input_path, "r")
content = file.read()
file.close()

file = open(output_path + "/" + name + ".per", "w")
file.write(aoeai.interpret(content))
file.close()

open(output_path + "/" + name + ".ai", "w").close()

