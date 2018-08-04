from scipy import misc

import matplotlib.pyplot as plt # plt 用于显示图片
import matplotlib.image as mpimg # mpimg 用于读取图片
import numpy as np


def demo1():
    lena = mpimg.imread('./gcsWithQt/dplot/lena.png')
    # 此时 lena 就已经是一个 np.array 了，可以对它进行任意处理
    # lena.shape #(512, 512, 3)


    plt.figure(1)
    plt.imshow(lena) # 显示图片
    plt.axis('off') # 不显示坐标轴
    # plt.show()


    lena_new_sz = misc.imresize(lena, 0.5) # 第二个参数如果是整数，则为百分比，如果是tuple，则为输出图像的尺寸
    plt.figure(2)
    plt.imshow(lena_new_sz)
    plt.axis('off')

    np.save('lena_new_sz', lena_new_sz) # 会在保存的名字后面自动加上.npy
    img = np.load('lena_new_sz.npy') # 读取前面保存的数组

    plt.figure(3)
    plt.imshow(img)
    plt.axis('off')
    plt.show()

def demo2():



if __name__ == '__main__':
    # demo1()
    demo2()