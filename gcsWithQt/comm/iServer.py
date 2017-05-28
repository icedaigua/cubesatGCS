from time import ctime,sleep
import socket
import threading

import struct

BUFSIZE = 1024

def rec_process(soc):
    while True:
        try:
            data = soc.recv(BUFSIZE)
            print("rec length= %d"%(len(data)))
            print(data)
        except:
            print("conn disconnect")
            soc.close()
            break

def send_buf(soc):
    while True:
        try:
            sleep(1)
            packed_data = generate_buff()
            soc.sendall(packed_data)

            packed_data = generate_buff_2()
            soc.sendall(packed_data)
        except:
            print("conn disconnect")
            soc.close()
            break

def generate_buff():
    reboot_cnt = 1
    rec_cmd_cnt = 0
    down_cnt =  1

    utc_time = 1482890254
    temp=[1.0,2.0,3.0]

    values = (0xEB,0x50,183, 'NJUST-3'.encode('utf8'),reboot_cnt,rec_cmd_cnt,down_cnt,0,1,1,12341,utc_time,temp[0],temp[1],temp[2],0)

    packer = struct.Struct('=3B 7s 1B 2H 1I 2B 1h 1I 3f 1B')
    packed_data = packer.pack(*values)

    return packed_data

def generate_buff_2():

    reboot_cnt = 1
    rec_cmd_cnt = 0
    down_cnt =  2

    utc_time = 1282890254

    values = (0xEB,0x51,163, reboot_cnt,rec_cmd_cnt,down_cnt,0,1,1,utc_time,0)

    # packer = struct.Struct('I f')
    packer = struct.Struct('=3B 1B 2H 1I 2B 1I 1B')
    packed_data = packer.pack(*values)

    return packed_data


def create_server(ADDR):
    sock = socket.socket(socket.AF_INET,socket.SOCK_STREAM)
    sock.bind(ADDR)

    sock.listen(10)

    while True:
        print("waiting for connection")
        tcpClientSocket,addr = sock.accept()
        print("connect from",addr)

        threading._start_new_thread(rec_process,(tcpClientSocket,))
        threading._start_new_thread(send_buf,(tcpClientSocket,))

    tcpClientSocket.close()
    sock.close()




def get_local_ip():
    hostname = socket.gethostname()

    print(hostname)
    local_ip = socket.gethostbyname(hostname)

    return local_ip



if __name__ == "__main__":
    # print(__file__)
    import sys
    print(sys.argv[1])

    HOST = sys.argv[1] #'192.168.0.102'
    PORT = int(sys.argv[2]) #50000

    create_server((HOST,PORT))

