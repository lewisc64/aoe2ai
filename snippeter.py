import aoeai, pyperclip

print("aoe2ai snippeter")
print("Output will be copied to the clipboard.\n")

while True:
    output = aoeai.translate(input(">"))
    print("\n" + output + "\n")
    pyperclip.copy(output)
