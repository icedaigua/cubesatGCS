
from ui.MainFrm import *

from abc import abstractmethod
import sys
import time

class uiCreate:
    def __init__(self):

        self.app = QtWidgets.QApplication(sys.argv)
        self.mainwindow = QtWidgets.QMainWindow()

        self.ui = Ui_Dialog()    
        self.ui.setupUi(self.mainwindow)
        
        self.ui_initz()
        self.ui_btn_click_conn()

        self.mainwindow.show()
        sys.exit(self.app.exec_())

    def ui_initz(self):
        self.ui.comboBox_2.addItems(["COM1","COM2","COM3","COM4","COM5","COM6"])
    
    def ui_btn_click_conn(self):
        self.ui.pBtn_test.clicked.connect(self.pBtn_test_click)
        self.ui.pBtn_open_conn.clicked.connect(self.pBtn_open_conn_click)
        self.ui.pBtn_open_serial.clicked.connect(self.pBtn_open_serial_click)

    def displayOBC(self,obc):
        # try:   
        self.ui.lE_sat_id.setText(str(obc[2]))
        self.ui.lE_reboot_cnt.setText(str(obc[4]))
        self.ui.lE_rec_cmd_cnt.setText(str(obc[6]))
        self.ui.lE_down_cnt.setText(str(obc[9]))
        self.ui.lE_reboot_time.setText(str(obc[10]))
        self.ui.lE_obc_workmode.setText(str(obc[11]))
        self.ui.lE_obc_utc_time.setText(str(obc[12]))
     
        self.ui.lE_obc_flash_index.setText(str(obc[45]))
        self.ui.lE_obc_flash_cnt.setText(str(obc[46]))

        # except :
        #     sys.exit()
        
    def displayEPS(self,eps):
        
        self.ui.lE_sun_c_1.setText(str(eps[18]))
        self.ui.lE_sun_c_2.setText(str(eps[19]))
        self.ui.lE_sun_c_3.setText(str(eps[20]))
        self.ui.lE_sun_c_4.setText(str(eps[21]))
        self.ui.lE_sun_c_5.setText(str(eps[22]))
        self.ui.lE_sun_c_6.setText(str(eps[23]))
        self.ui.lE_sun_V_1.setText(str(eps[24]))
        self.ui.lE_sun_V_2.setText(str(eps[25]))
        self.ui.lE_sun_V_3.setText(str(eps[26]))
        self.ui.lE_sun_V_4.setText(str(eps[27]))
        self.ui.lE_sun_V_5.setText(str(eps[28]))
        self.ui.lE_sun_V_6.setText(str(eps[29]))

    def displayADCS(self,adcs):
        self.ui.lE_adcs_reboot_cnt.setText(str(adcs[49]))
        self.ui.lE_adcs_rec_cnt.setText(str(adcs[50]))
        self.ui.lE_adcs_send_cnt.setText(str(adcs[51]))
        self.ui.lE_adcs_reboot_time.setText(str(adcs[52]))
        self.ui.lE_adcs_utc_time.setText(str(adcs[54]))
        self.ui.lE_adcs_cpu_temp.setText(str(adcs[55]))

        self.ui.lE_adcs_mode.setText(str(adcs[56]))
        self.ui.lE_adcs_dam_cnt.setText(str(adcs[57]))
        self.ui.lE_adcs_pitch_cnt.setText(str(adcs[58]))
        self.ui.lE_adcs_ctrl_cnt.setText(str(adcs[59]))
       
        self.ui.lE_adcs_adc_1.setText(str(adcs[80]))
        self.ui.lE_adcs_adc_2.setText(str(adcs[81]))
        self.ui.lE_adcs_adc_3.setText(str(adcs[82]))
        self.ui.lE_adcs_adc_4.setText(str(adcs[83]))
        self.ui.lE_adcs_adc_5.setText(str(adcs[84]))
        self.ui.lE_adcs_adc_6.setText(str(adcs[85]))
        self.ui.lE_adcs_adc_7.setText(str(adcs[86]))
        self.ui.lE_adcs_adc_8.setText(str(adcs[87]))
        self.ui.lE_adcs_adc_9.setText(str(adcs[88]))
        self.ui.lE_adcs_adc_10.setText(str(adcs[89]))
      

    def pBtn_test_click(self):
        QtWidgets.QMessageBox.information(self.ui.pBtn_test,"str","str")
    
    @abstractmethod
    def pBtn_open_conn_click(self):
        pass
    
    @abstractmethod
    def pBtn_open_serial_click(self):
        pass

    def show_msg(self,msg):
        self.ui.tE_rec_buff.append(msg)  



if __name__=='__main__':
    
    ui = uiCreate()
 
# import sys  
# from PyQt5 import QtWidgets  
  
  
# #pyqt窗口必须在QApplication方法中使用  
# app=QtWidgets.QApplication(sys.argv)  
  
  
# label=QtWidgets.QLabel("<p style='color: red; margin-left: 20px'><b>hell world</b></p>")      #qt支持html标签，强大吧  
# #有了实例，就需要用show()让他显示  
# label.show()  
    
