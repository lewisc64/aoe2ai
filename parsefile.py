import aoeai
import sys

input_path, output_path, name = sys.argv[1:]

file = open(input_path, "r")
content = file.read()
file.close()

file = open(output_path + "/" + name + ".per", "w")
file.write(aoeai.interpret(content))
print("Saved to '{}'.".format(file.name))
file.close()

file = open(output_path + "/" + name + ".ai", "w")
print("Saved to '{}'.".format(file.name))
file.close()

