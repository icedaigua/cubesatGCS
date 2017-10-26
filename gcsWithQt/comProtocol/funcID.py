
fID = {                                                
    'INS_APP_HK_GET'        :   '0x02',                  
    'INS_HK_GET'            :   '0x03',                  #下行星上状态遥测数据  
    'INS_DOWN_CMD_ON'       :   '0x05',                  #下行星上指令数据
    'INS_APP_STR_DOWN'      :   '0x06',                  #

    'INS_OBC_RST'           :   '0x08',                 #星务计算机重启
    'INS_DOWN_CMD_OFF'      :   '0x09',                 #停止下行星上指令数据
    
    'INS_OBC_EPS_ON'        :	'0x0A' ,
    'INS_OBC_EPS_OFF'		:	'0x0B' ,
    'INS_OBC_WORKMODE'  	:	'0x0C' ,
    
    #开关指令
    'INS_MTQ_ON'             :   '0x10',                #磁棒开
    'INS_MTQ_OFF'            :   '0x11',                #磁棒关  
    'INS_GPS_A_ON'           :   '0x12',                #GPSA开
    'INS_GPS_A_OFF'          :   '0x13',                #GPSA关
    'INS_ADCS_ON'            :   '0x15',                #ADCS开机
    'INS_ADCS_OFF'           :   '0x14',                #ADCS关机  
    'INS_RSV_ON'             :   '0x16',                #保留开关1开
    'INS_RSV_OFF'            :   '0x17',                #保留开关1关  
    'INS_MW_A_ON'            :   '0x18',                #动量轮A开         
    'INS_MW_A_OFF'           :   '0x19',                #动量轮A关
    'INS_MW_B_ON'            :   '0x1A',                #动量轮B开
    'INS_MW_B_OFF'           :   '0x1B',                #动量轮B关   
    'INS_SLBRD_ON'           :   '0x1C',                #帆板开
    'INS_SLBRD_OFF'          :   '0x1D',                #帆板关
    'INS_USB_ON'             :   '0x1E',                #天线开
    'INS_USB_OFF'            :   '0x1F',                #USB关 
    'INS_S1_ON'              :   '0x20',                #动量轮C开
    'INS_S1_OFF'             :   '0x21',                #动量轮C关   
    'INS_S2_ON'              :   '0x22',                #天线电源开
    'INS_S2_OFF'             :   '0x23',                #天线电源关
    'INS_S3_ON'              :   '0x24',                #陀螺仪开
    'INS_S3_OFF'             :   '0x25',                #陀螺仪关
    'INS_S4_ON'              :   '0x26',                #动量轮D开
    'INS_S4_OFF'             :   '0x27',                #动量轮D关


    'INS_MTQ1_DIR_POS'       :   '0x28',                #磁棒1正方向
    'INS_MTQ1_DIR_NAG'       :   '0x29',                #磁棒1反方向
    'INS_MTQ2_DIR_POS'       :   '0x2A',                #磁棒2正方向
    'INS_MTQ2_DIR_NAG'       :   '0x2B',                #磁棒2反方向
    'INS_MTQ3_DIR_POS'       :   '0x2C',                #磁棒3正方向
    'INS_MTQ3_DIR_NAG'       :   '0x2D',                #磁棒3反方向
    'INS_PIANZHI_MODE'       :   '0x2E',                #偏置动量模式
    'INS_ZERO_MODE'          :   '0x2F',                #零动量模式

    'INS_DET'                :  '0x31',                 #重新阻尼
    'INS_STA'                :  '0x32',                 #永久阻尼使能
    'INS_DUMP_FOEV_DIS'      :  '0x33',                 #永久阻尼禁止  
    'INS_SW_MAG_A_ON'        :  '0x34',                 #磁强计A开
    'INS_SW_MAG_B_ON'        :  '0x35',                 #磁强计B开
    'INS_SW_MW_A'            :  '0x36',                 #切换至动量轮A
    'INS_SW_MW_B'            :  '0x37',                 #切换至动量轮B
    'INS_SW_MAG_A_OFF'       :  '0x38',                 #磁强计A关
    'INS_SW_MAG_B_OFF'       :  '0x39',                 #磁强计B关
    'INS_SW_BATT_WARM_ON'    :  '0x3A',                 #电源加热开
    'INS_SW_BATT_WARM_OFF'   :  '0x3B',                 #电源加热关
    'INS_ERROR_ENABLE'       :  '0x3D',
    'INS_RSH'                :  '0x3E',
    'INS_CLOSE_ALL'          :  '0x3F',

    #系统指令 
    'INS_SW_1200'            : '0x41',                  #BPSK1200切换
    'INS_SW_9600'            : '0x42',                  #BPSK9600开
    'INS_CW_ON'              : '0x43',                  #CW开
    'INS_COM_TRAN_OFF'       : '0x44',                  #通信机发射机关机
    'INS_COM_PERIOD'         : '0x45',                  #下行更新
    
    #数据注入指令
    'INS_CTL_P_PRA'          :  '0x51',                 #三轴稳定控制律注入
    'INS_CTL_D_PRA'          :  '0x52',                 #三轴稳定控制律注入
    'INS_ZJD_CTL'            :  '0x53',                 #章进动控制系数
    'INS_DMP_FLAG'           :  '0x54',                 #阻尼标志位
    'INS_FLT_FLAG'           :  '0x55',                 #测量标志位
    'INS_CTL_FLAG'           :  '0x56',                 #控制标志位
    'INS_GYRO_FILTER_ON'     :  '0x57',                 #开启陀螺仪滤波
    'INS_GYRO_FILTER_OFF'    :  '0x58',                 #关闭陀螺仪滤波
    'INS_CNT_CTL_FLAG'       :  '0x59',                 #
    'INS_ORB_TLE_FLAG'       :  '0x5A',                 #TLE轨道上注
    'INS_MAG_FILTER_ON'      :  '0x5B',                 #启动磁强计滤波器
    'INS_QR'                 :  '0x5C',                 #QR参数注入
    'INS_MAG_FILTER_OFF'     :  '0x5D',                 #关闭磁强计滤波
    'INS_TIME_IN'            :  '0x5E',                 #姿控参数注入

}

if __name__ == '__main__':
    print(fID['INS_APP_HK_GET'])
    print(fID['INS_MAG_FILTER_OFF'])