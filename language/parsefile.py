import aoeai
import sys
import os

if len(sys.argv) < 4 or "-help" in sys.argv:
    print("Usage:")
    print("parsefile.py input_file_path output_folder_path ai_name")
    print("Flags:")
    print(" - -help")
    print(" - -userpatch")
    exit()
    
input_path, output_path, name = sys.argv[1:4]

file = open(input_path, "r")
content = file.read()
file.close()

per = aoeai.translate(content, stamp=True, userpatch=("-userpatch" in sys.argv))

per_path = output_path + "/" + name + ".per"
ai_path = output_path + "/" + name + ".ai"

file = open(per_path, "w")
file.write(per)
print("Saved to '{}'.".format(file.name))
file.close()

if not os.path.isfile(ai_path):
    file = open(ai_path, "w")
    print("Saved to '{}'.".format(file.name))
    file.close()

