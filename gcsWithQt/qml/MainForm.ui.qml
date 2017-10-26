import Qt 5.9
import QtQuick 2.5

Rectangle {
    width: 360
    height: 360

    Text {
        id: text2
        x: 168
        y: 143
        color: "#ab0f0f"
        text: qsTr("Hello World")
        font.pixelSize: 24
    }

    MouseArea {
        id: mouseArea
        anchors.fill: parent
    }
}
