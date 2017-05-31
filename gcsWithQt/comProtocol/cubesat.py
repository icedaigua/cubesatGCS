from struct import pack,unpack

def up_ctrl_encode():
    return pack("=4B1f1B",)

def up_ctrl_decode(str):
    return unpack("=4B1f1B",str)

def obc_decode(str):
    return unpack("=4B 3H 1I 2B 1I 1h 6h 12H 14H 1I 2H 2B 3H 1I 1H 1I 1h 1B 3H 3h 1f 6h 1H 3h 3f 3h 10h 1B",str)