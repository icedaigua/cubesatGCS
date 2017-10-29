# import sys
 
# from PyQt5.QtCore import QUrl, QObject, pyqtSlot
# from PyQt5.QtGui import QGuiApplication
# from PyQt5.QtQuick import QQuickView
 
# app = QGuiApplication(sys.argv)
 
# # Create the QML user interface.
# view = QQuickView()
# view.setSource(QUrl('MainForm.ui.qml'))
# # view.setResizeMode(QDeclarativeView.SizeRootObjectToView)
# view.setGeometry(100, 100, 400, 240)
# view.show()
 
# app.exec_()


if __name__ == '__main__':
    import sys
    from PyQt5.QtCore import QUrl
    from PyQt5.QtWidgets import QApplication
    from PyQt5.QtQuick import QQuickView

    # Create main app
    myApp = QApplication(sys.argv)
    # Create a label and set its properties
    appLabel = QQuickView()
    appLabel.setSource(QUrl('basic.qml'))

    # Show the Label
    appLabel.show()

    # Execute the Application and Exit
    myApp.exec_()
    sys.exit()