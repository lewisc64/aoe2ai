import aoeai

print("aoe2ai snippeter")

copy = True
try:
    import pyperclip
    print("Output will be copied to the clipboard.\n")
except ImportError:
    print("pyperclip not found, automatic copying disabled.")
    copy = False

while True:
    output = aoeai.translate(input(">"))
    print("\n" + output + "\n")
    if copy:
        pyperclip.copy(output)
