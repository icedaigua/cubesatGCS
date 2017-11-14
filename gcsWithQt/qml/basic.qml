import QtQuick 2.4

Rectangle {
    id: rectangle
    width: 420; height: 420
    gradient: Gradient {
        GradientStop {
            position: 0
            color: "#ffff00"
        }

        GradientStop {
            position: 0.656
            color: "#fffff0"
        }

        GradientStop {
            position: 1
            color: "#00ff00"
        }

    }
    border.width: 2
    border.color: "#000fff"

    Grid {
        id: grid
        x: 8
        y: 8

        columns: 2
        spacing: 5

        Text {
            id: text1
            color: "#000fff"

            text: qsTr("Time")
            styleColor: "#ff0000"
            textFormat: Text.RichText
            renderType: Text.QtRendering
            font.family: "Times New Roman"
            font.pixelSize: 16
            style: Text.Sunken
            font.bold: false
            verticalAlignment: Text.AlignVCenter
            horizontalAlignment: Text.AlignHCenter
        }
        TextEdit {
            id: textEdit
            color: "#ff0000"
            text: qsTr("Text Edit")
            font.family: "Times New Roman"
            readOnly: true
            textFormat: Text.RichText
            font.pixelSize: 16

        }
        Text {
            id: text3

            text: qsTr("Text")
            font.pixelSize: 16
        }

        TextEdit {
            id: textEdit2
            text: qsTr("Text Edit")
            font.pixelSize: 16
        }
    }

    Rectangle {
        id: rectangle1
        x: 96
        y: 91
        width: 123
        height: 20
        color: "#ffffff"
    }


}
