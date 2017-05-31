
from iNet.iClient import TcpClient
from iSerial.iSerial import iSerial
from comProtocol.cubesat import obc_decode
from ui.uiCreate import uiCreate

import threading
import sys
import time


class crossGCS(uiCreate):

    def __init__(self):
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
        self.gcSerial = iSerial()
        self.gcSerial.openSerialPort("COM8",115200)
        
        threading._start_new_thread(self.serial_rec_process,())

    def serial_rec_process(self):
        while True:
            # try:
            data = self.gcSerial.read(190)
            # print(str(data))
            # print("len =" + str(len(data)))
            if(len(data)>=190):
                obc = obc_decode(data)
                self.displayOBC(obc)
                self.displayEPS(obc)
                self.displayADCS(obc)
            else:
                self.gcSerial.clear()
               
            # except:
            #     print("serial Error")
                # sys.exit()

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
    gcs = crossGCS()











     