from struct import pack,unpack

def up_ctrl_encode():
    return pack("=4B1f1B",)

def up_ctrl_decode(str):
    return unpack("=4B1f1B",str)

def obc_decode(str):
    return unpack("=4B3H1I2B1I1H1H18B21B32H1I2H3f3H1f2B2H5B",str)