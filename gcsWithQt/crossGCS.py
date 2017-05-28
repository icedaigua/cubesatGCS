
from comm.iClient import TcpClient
from protocol.cubesat import obc_decode
from ui.control import ui_control

import threading
import sys
import time


class crossGCS(ui_control):

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

    def rec_process(self):
        while True:
            try:
                data = client.rec_buff()
                if len(data)<200:
                    obc_de = obc_decode(data)
                    print(obc_de)
                    ui.set_display(obc_de[6])
            except:
                print("socket disconnet")
                process_exit = False
                sys.exit()



gcs = crossGCS()











     