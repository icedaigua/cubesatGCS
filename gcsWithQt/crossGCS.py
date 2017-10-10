
from iNet.iClient import TcpClient
from iSerial.iSerial import iSerial
from comProtocol.cubesat import obc_decode
from ui.uiCreate import uiCreate
from iLogging.iLogging import iLogging
import logging

import threading
import sys
import time

# from collections import deque
# import queue


class crossGCS(uiCreate):

    def __init__(self):
        # self.recbuf=deque(maxlen=10240)
        # self._recbuf = queue.Queue(maxsize=10240)
        self._recbuf = []
        super(crossGCS,self).__init__()
        

    def pBtn_open_conn_click(self):

        self._up_client = TcpClient(self.ui.comboBox.currentText,
        self.ui.lE_up_port.text)
        
        if(self._up_client.conn()):
            self.show_msg("up conn success!")
        else:
            self.show_msg("up conn fail!")

        self._down_client = TcpClient(self.ui.comboBox.currentText,
        self.ui.lE_down_port.text)
        
        if(self._down_client.conn()):
            self.show_msg("down conn success!")
        else:
            self.show_msg("down conn fail!")

        threading._start_new_thread(self.rec_process,())
    
    def pBtn_open_serial_click(self):
        print("Open Serial")
        self._gcSerial = iSerial()
        try:
            self._gcSerial.openSerialPort(self.ui.comboBox_2.currentText(),9600)
          
        except:
            print("serial Open Error")
            sys.exit()

        threading._start_new_thread(self.serial_rec_process,())
        threading._start_new_thread(self.AnalysisBuffer,())

    def serial_rec_process(self):
        while True:
            # try:
            data = self._gcSerial.read(200)
            self._recbuf.extend(data)
            # heapq.heappush(self.recbuf,data)
            # print(str(data))
            # print("len =" + str(len(data)))
            # if(len(data)>=190):
            #     obc = obc_decode(data)
            #     self.displayOBC(obc)
            #     self.displayEPS(obc)
            #     self.displayADCS(obc)
            # else:
            #     self.gcSerial.clear()
               
            # except:
            #     print("serial Error")
                # sys.exit()
    def AnalysisBuffer(self):
         while True:
             if len(self._recbuf)>=512:
                header = [self._recbuf.pop(0),self._recbuf.pop(0)]
                # print(header)
                if(header[0]==0x1A and header[1]==0x50):
                    # print(self._recbuf[0:2])
                    obcBuffer = header[:]+self._recbuf[0:80]
                    del self._recbuf[0:80]
                    obc = obc_decode(bytes(obcBuffer))
                    self.displayOBC(obc)
                    print("OBC")
                if(header[0]==0x1A and header[1]==0x51):
                    # print(self._recbuf[0:2])
                    adcsBuffer = header[:]+self._recbuf[0:94]
                    del self._recbuf[0:94]
                    # obc = obc_decode(bytes(adcsBuffer))
                    # self.displayOBC(obc)
                    print("ADCS")
             else:
                 pass


    def rec_process(self):
        while True:
            try:
                data = client.rec_buff()
                if len(data)<200:
                    obc_de = obc_decode(data)
                    print(obc_de)
                    set_display(obc_de[6])
            except:
                print("socket disconnet")
                sys.exit()


if __name__ == '__main__':
    iLogging(sys.path[0]+'/')
    # logging.warning("Warning")
    # logging.debug("Debug")
    # logging.info("info")
    gcs = crossGCS()

    # print(str(0xCA)+str(0x03))
    data=[1,2,3,4,5,6,7,8,9,10]
    print(data[0])
    data2 = data[2:8]
    # print(data[0]==0x51 and data[1]==0x51)
    data.pop(0)
    print(data)
    print(data2)
    print(data.index(5))

    del data[2:5]
    print(data)
    print(data2)
    # print(data.index(11))










     