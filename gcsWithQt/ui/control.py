
from ui.MainFrm import *

from abc import abstractmethod
import sys
import time

class ui_control:
    def __init__(self):

        self.app = QtWidgets.QApplication(sys.argv)
        self.mainwindow = QtWidgets.QMainWindow()

        self.ui = MainFrmDlg()    
        self.ui.setupUi(self.mainwindow)
        
        self.ui_initz()
        self.ui_btn_click_conn()

        self.mainwindow.show()
        sys.exit(self.app.exec_())

    def ui_initz(self):
        pass
    
    def ui_btn_click_conn(self):
        self.ui.pBtn_test.clicked.connect(self.pBtn_test_click)
        self.ui.pBtn_open_conn.clicked.connect(self.pBtn_open_conn_click)

    def set_display(self,data):
        try:
            self.ui.lineEdit.setText(str(data))
            self.ui.checkBox.isVisible = False
            self.ui.label_4.setText('12343')
        except :
            sys.exit()
        

    def pBtn_test_click(self):
        QtWidgets.QMessageBox.information(self.ui.pBtn_test,"str","str")
    
    @abstractmethod
    def pBtn_open_conn_click(self):
        pass

    def show_msg(self,msg):
        self.ui.tE_rec_buff.append(msg)  



if __name__=='__main__':
    
    ui = ui_control()
 
# import sys  
# from PyQt5 import QtWidgets  
  
  
# #pyqt窗口必须在QApplication方法中使用  
# app=QtWidgets.QApplication(sys.argv)  
  
  
# label=QtWidgets.QLabel("<p style='color: red; margin-left: 20px'><b>hell world</b></p>")      #qt支持html标签，强大吧  
# #有了实例，就需要用show()让他显示  
# label.show()  
    
