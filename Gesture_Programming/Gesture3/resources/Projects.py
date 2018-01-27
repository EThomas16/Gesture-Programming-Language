import traceback
try:
    The = 100
    Variable = 10
    while The <= 200:
        The = The + Variable
        print("Can't", The)
except:
    traceback.print_exc()
input (' ')