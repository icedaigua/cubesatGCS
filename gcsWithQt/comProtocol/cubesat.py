from struct import pack,unpack

def up_ctrl_encode():
    return pack("=4B1f1B",)

def up_ctrl_decode(str):
    return unpack("=4B1f1B",str)

def obc_decode(str):
    return unpack("=4B 1H 1B 1H 2B 1H 1I 1B 1I 1h 1I 4B 1I 1B 1H 1I 1H 6b 12B 3H 11b 1H",str)