from struct import pack,unpack

def up_ctrl_encode(pid,func,delay):
    flags = 0
    sport = 1
    dport = 18
    dst = 26
    src = 1
    pri = 0
    header = generateHeader(flags, sport, dport, dst, src, pri)
    crc = 0

    ctrlLen = 15
    return pack("=1I 3B 2I",header,ctrlLen,pid,func,delay,crc)

def up_para_encode(pid,func,delay,para):
    flags = 0
    sport = 1
    dport = 18
    dst = 26
    src = 1
    pri = 0
    header = generateHeader(flags, sport, dport, dst, src, pri)
    crc = 0

    ctrlLen = 23
    return pack("=1I 3B 4I",header,ctrlLen,pid,func,delay,para[0],para[1],crc)

def up_orbit_encode(pid,func,delay,orbit):
    flags = 0
    sport = 1
    dport = 18
    dst = 26
    src = 1
    pri = 0
    header = generateHeader(flags, sport, dport, dst, src, pri)
    crc = 0

    ctrlLen = 79
    return pack("=1I 3B 1I 8d 1I",header,ctrlLen,pid,func,delay,\
                orbit[0],orbit[1],orbit[2],orbit[3],\
                orbit[4],orbit[5],orbit[6],orbit[7],\
                crc)

def obc_decode(bytesBuf):
    return unpack("=4B 1H 1B 1H 2B 1H 1I 1B 1I 1h 1I 4B 1I 1B 1H 1I 1H 6b 12B 3H 11b 1H",bytesBuf)

def generateHeader(flags, sport, dport, dst, src, pri):

    header = ((pri & 0x03) << 31) + ((src & 0x1F) << 25)    \
            + ((dst & 0x1F) << 20) + ((dport & 0x3F) << 14) \
            +((sport & 0x3F) << 8) + (flags << 0)
    
    return (((header & 0xff000000) >> 24) | ((header & 0x000000ff) << 24) \
            | ((header & 0x0000ff00) << 8) | ((header & 0x00ff0000) >> 8))
    

if __name__ == '__main__':
    #  flags = 0
    #  sport = 1
    #  dport = 18
    #  dst = 26
    #  src = 1
    #  pri = 0
    #  print(generateHeader(flags, sport, dport, dst, src, pri))

    #  print((1 & 0x1F)<<25)
    #  print((26 & 0x1F)<<20)
    para=[1,2]
    print(para[0])
    print(up_ctrl_encode(1,0x08,1234))

