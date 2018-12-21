import aoeai
import sys


if len(sys.argv) != 4:
    print("Usage:")
    print("parsefile.py input_file_path output_folder_path ai_name")
    exit()
    
input_path, output_path, name = sys.argv[1:]

file = open(input_path, "r")
content = file.read()
file.close()

per = aoeai.translate(content, stamp=True)

file = open(output_path + "/" + name + ".per", "w")
file.write(per)
print("Saved to '{}'.".format(file.name))
file.close()

file = open(output_path + "/" + name + ".ai", "w")
print("Saved to '{}'.".format(file.name))
file.close()

