#! /Users/iceyuyu/anaconda/bin/python

from struct import pack,unpack,Struct

from time import gmtime


down_cnt = 0
last_reset_time = 0
utc_time = 1482890254
temp=[1.0,2.0,3.0]

def display(info):

    # print(info)
    print('length='+str(info[2]))
    print(info[3])
    print('reboot_cnt='+str(info[4]))
    print('rec_cnt='+str(info[5]))
    print('down_cnt='+str(info[6]))

    print(info[12])

    reset_time = gmtime(info[7])
    print('reset_time='+str(reset_time))

    print('control mode='+str(info[8]))
    print('word mode='+str(info[9]))

    utc_time = gmtime(info[12])
    print('utc_time='+str(utc_time))

    obc_temp = (info[13]*2493.0/1023.0-424.0)/6.25
    print('obc_temp = '+str(obc_temp))

    hmr_temp = info[14]*0.01
    print('hmr_temp = '+str(hmr_temp))

    print('momentum speed = %4d %4d'%(info[31],info[32]))
    print('hmr = %10.3f %10.3f %10.3f'%(info[33],info[34],info[35]))
    print('battary = %4d busV = %4d %4d %4d  busC = %4d %4d %4d'%(info[56],info[53],info[54],info[55],info[57],info[58],info[59]))


def parse_file():
    f = open('./comm/data','r')
    lines = f.readlines()

    print(len(lines))

    ssbytes=[]

    for s in lines:
        if len(s)>10:
            ssbytes.clear()
            s_byte = ''.join(s.split(" "))


            for i in range(0,len(s_byte)-1,2):
                tmp = hex(int(s_byte[i:i+2],16))
                i_tmp = int(tmp,16)
                ssbytes.append(i_tmp)


            info = unpack('=3B 7s 1B 2H 1I 2B 1H 1h 1I 18h 2H 3f 6h 3d 3f 3h 2B 16h 12b 5B',bytes(ssbytes))
            display(info)

def s_pack():
    values = (1, 'NJUST-3'.encode('utf8'),2.7)
    
    packer = Struct('I 7s f')
    packed_data = packer.pack(*values)

    print(packed_data)

def generate_buff():
    
    reboot_cnt = 1
    rec_cmd_cnt = 0
    down_cnt =  1
    
    values = (0xEB,0x50,183, 'NJUST-3'.encode('utf8'),reboot_cnt,rec_cmd_cnt,down_cnt,0,1,1,12341,utc_time,temp[0],temp[1],temp[2],0)
    
    # packer = struct.Struct('I f')
    packer = Struct('=3B 7s 1B 2H 1I 2B 1h 1I 3f 1B')
    packed_data = packer.pack(*values)

    return packed_data

if __name__ == '__main__':
    # parse_file()
    # s_pack()
    print(generate_buff())



    # 		unsigned char satname[7];

# 		unsigned char reboot_count;							//1
# 		unsigned short int rec_cmd_count;					//2
# 		unsigned short int down_count;						//2

# 		unsigned int last_reset_time;					//4

# 		unsigned char control_mode;							//1
# 		unsigned char work_mode;							//1

# 		unsigned short int CRC_err_cnt;						//2
# 		short int status_sensor_on_off;						//2

# 		unsigned int utc_time;								//4

# 		short int tmep_hk;									//2
# 		short int temp_hmr;									//2

# 		short int tempe[12]; //adc_1~adc_8					//24  2*12
# 		short int adc; //adc 13								//2   2*1
# 		short int bar[3]; //adc_15; bar x,y,z				//6   2*3

# 		unsigned short int momentum_a_vel;					//2
# 		unsigned short int momentum_b_vel;					//2

# 		float hmr[3];										//12   4*3

# 		short int angle_pitch_mearment;						//2
# 		short int angle_pitch;								//2
# 		short int pitch_rate;								//2

# 		short int dam_count;								//2
# 		short int pitch_count;								//2
# 		short int ctrl_count;								//2

# 		double down_orbit_posi[3];								//24  3*8
# 		float down_orbit_velo[3];							//12 3*4
# //		float mag_wgs84[3];								//12 3*4

# 		short int control_para[3];			//P Z D					//6   2*3

# 		unsigned char gps_status;							//1
# 		unsigned char orbit_bit;							//1

# 		short int vboost[3];		//								//6   2*3
# 		short int vbatt;				//									//2
# 		short int curin[3];			//								//6   2*3
# 		short int cursun;				//									//2
# 		short int cursys;				//									//2
# 		short int curout[6];		//								//12  2*6
# 		short int status_output;							//2
# 		char latchup[6];									//6
# 		char temp[6];									//6

# //		short int bar_compute[3];						//6 3*2

# 		unsigned char atenna_status;						//1
# 		unsigned char battery_status;
# 		unsigned char block_num;
# 		unsigned char index;							//1

# 		unsigned char reserved;
