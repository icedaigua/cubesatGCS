#! /usr/bin

import sys
import socket
from socket import socket,AF_INET,SOCK_STREAM,SOCK_DGRAM

#tcp ->SOCK_STREAM,udp->SOCK_DGRAM
class TcpClient:

    def __init__(self,host,port):
        self._client = socket(AF_INET,SOCK_STREAM)
        self._BUFSIZE = 1024
        self._host = host
        self._port = port

    def conn(self):
        try:
            self._client.connect((self._host,self._port))
            return True
        except:
            return False
            

    def rec_buff(self):       
        try:
            data = self._client.recv(self._BUFSIZE)
            print("rec length="+str(len(data)))
            return data
        except :
            print("rec buffer error:")
            sys.exit()        

    def send_buf(self,sendbuf):  
        self._client.send(sendbuf)


if __name__ == "__main__":
    # print(__file__)

    import threading
    from struct import unpack

    client = TcpClient('192.168.1.104',50000)
    client.conn()

    def conn_process():
        while True:
            try:
                data = client.rec_buff()
                # print(data[0])
                if(len(data) == 0):
                    print("server is over!")
                    break
                # info = unpack("=If",data)
                # print(str(info[0])+'\t'+str(info[1]))
            except:
                print("client disconnect")
                break

    threading._start_new_thread(conn_process,())

    while True:
        pass

 
    


