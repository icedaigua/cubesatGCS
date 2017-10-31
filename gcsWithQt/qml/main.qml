import QtQuick 2.0

import QtQuick.Window 2.1
import QtQuick.Controls 1.2
import QtQuick.Dialogs 1.1

Rectangle {
    width: 250
    height: 175

    Text {
        id: helloText
        anchors.verticalCenter: parent.verticalCenter
        anchors.horizontalCenter: parent.horizontalCenter
        text: "Hello World!!!\n Traditional first app using PyQt5"
        horizontalAlignment: Text.AlignHCenter
    }
}