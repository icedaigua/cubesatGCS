import sys
 
from PyQt5.QtCore import QUrl, QObject, pyqtSlot
from PyQt5.QtGui import QGuiApplication
from PyQt5.QtQuick import QQuickView
 
app = QGuiApplication(sys.argv)
 
# Create the QML user interface.
view = QQuickView()
view.setSource(QUrl('MainForm.ui.qml'))
# view.setResizeMode(QDeclarativeView.SizeRootObjectToView)
view.setGeometry(100, 100, 400, 240)
view.show()
 
app.exec_()