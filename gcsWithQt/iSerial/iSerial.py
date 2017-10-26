#!/usr/bin/python

import serial

class iSerial:

    """serial functions include :
                    checkports()
                    openSerialPort()
                    closeSerialPort()
                    baudRateChanged()"""

    def __init__(self):
        self._ser = serial.Serial()

    # def checkPorts(self):
    #     list = list_ports.comports()
    #     ports = []
    #     for port in list:
    #         if not port[0].startswith("/dev/ttyS"):
    #             ports.append(port[0])
    #     return ports

    def openSerialPort(self, serialName, serialBaudRate):
        self._ser.baudrate = serialBaudRate
        self._ser.port = serialName
        self._ser.bytesize = serial.EIGHTBITS #number of bits per bytes
        self._ser.parity = serial.PARITY_NONE #set parity check: no parity
        self._ser.stopbits = serial.STOPBITS_ONE #number of stop bits
        self._ser.timeout = 1          #block read
        # xonxoff = False,     #disable software flow control
        self._ser.rtscts = False     #disable hardware (RTS/CTS) flow control
        # # ser.dsrdtr = False       #disable hardware (DSR/DTR) flow control
        # # ser.writeTimeout = 2     #timeout for write
        # interCharTimeout=None
        self._ser.open()
        # self.protocol.write_line('Serial opened !')

    def closeSerialPort(self):
        # self.ReadThread.stop()
        self._ser.close()
        # print(b'Serialport closed!')

    def baudRatechanged(self, baudRate):
        serialconfdict = self._ser.get_settings()
        serialconfdict['baudrate'] = baudRate
        self._ser.apply_settings(serialconfdict)
        print("serial baudrate changed !")
    def write(self, send_buf):
        self._ser.write(send_buf)
    def read(self,len):
        response = self._ser.read(len)
        # print("read data: " + str(response))
        return response
    def clear(self):
        self._ser.flushInput()
        self._ser.flushOutput()


#

if __name__ == '__main__':
    print("OK")
    # iSerial()



#  def iSerial():
#     ser = serial.Serial()
   
#     ser.port = "COM8"  #"/dev/ttyUSB7"
#     ser.baudrate = 115200
#     ser.bytesize = serial.EIGHTBITS #number of bits per bytes
#     ser.parity = serial.PARITY_NONE #set parity check: no parity
#     ser.stopbits = serial.STOPBITS_ONE #number of stop bits
#     ser.timeout = 1          #block read
#     # xonxoff = False,     #disable software flow control
#     ser.rtscts = False     #disable hardware (RTS/CTS) flow control
#     # # ser.dsrdtr = False       #disable hardware (DSR/DTR) flow control
#     # # ser.writeTimeout = 2     #timeout for write
#     # interCharTimeout=None
#     # )
#     try: 
#         ser.open()
#     except:
#         print("error open serial port: ")
#         exit()

#     if ser.isOpen():

#         # try:
#             # ser.flushInput() #flush input buffer, discarding all its contents
#             # ser.flushOutput()#flush output buffer, aborting current output 
#             #          #and discard all that is in buffer

#             # #write data
#             # ser.write("AT+CSQ")
#             # print("write data: AT+CSQ")

#             # time.sleep(0.5)  #give the serial port sometime to receive the data

#             # numOfLines = 0

#         while(True):
#             # rec_len = ser.inWaiting()
#             # if(rec_len>0):
#             response = ser.read(190)
#             # print("read data: " + str(response))
#             # info = unpack('=4B 3H 1I 2B 1I 1h 6h 12H 14H 1I 2H 2B 3H 1I 1H 1I 1h 1B 3H 3h 1f 6h 1H 3h 3f 3h 10h 1B',response)
#                 # numOfLines = numOfLines + 1

#                 # if (numOfLines >= 5):
#                 #     break

#                 # ser.close()
#         # except IOError as err:
#         #     print("I/O error: {0}".format(err))
#         # except:
#         #      print("serial communication error")

#     # else:
#     #     print("cannot open serial port")

# from serial import threaded
# from serial.tools import list_ports

# LineReader = serial.threaded.LineReader
# ReaderThread = serial.threaded.ReaderThread

# class PrintLines(LineReader):

#     def connection_made(self, transport):
#         super(PrintLines, self).connection_made(transport)
#         print('port opened\n')

#     def handle_line(self, data):
#         print('line received: {!r}\n'.format(data))

#     def data_received(self,data):
#         super(PrintLines, self).data_received(data)
#         sys.stdout.write(data)
#         """data为串行口输出内容"""

#     def connection_lost(self, exc):
#         if exc:
#             traceback.print_exc(exc)
#         print('port closed\n')


# class aserialPort:

#     """serial functions include :
#                     checkports()
#                     openSerialPort()
#                     closeSerialPort()
#                     baudRateChanged()"""

#     def __init__(self):
#         self.threadOpenedOnce = False
#         self.ser = serial.Serial()

#     def checkPorts(self):
#         list = list_ports.comports()
#         ports = []
#         for port in list:
#             if not port[0].startswith("/dev/ttyS"):
#                 ports.append(port[0])
#         return ports

#     def openSerialPort(self, serialName, serialBaudRate):
#         self.ser.baudrate = serialBaudRate
#         self.ser.port = serialName
#         self.ser.open()
#         self.ReadThread = ReaderThread(self.ser, PrintLines)
#         self.ReadThread.start()
#         self.ReadThread.connect()
#         self.transport, self.protocol = self.ReadThread.connect()
#         self.protocol.write_line('Serial opened !')

#     def closeSerialPort(self):
#         self.ReadThread.stop()
#         self.ser.close()
#         print(b'Serialport closed!')

#     def baudRatechanged(self, baudRate):
#         serialconfdict = self.ser.get_settings()
#         serialconfdict['baudrate'] = baudRate
#         self.ser.apply_settings(serialconfdict)
#         print("serial baudrate changed !")