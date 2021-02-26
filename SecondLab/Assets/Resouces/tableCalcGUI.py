import sys

import matplotlib.pyplot as plt
from matplotlib.backends.backend_tkagg import FigureCanvasTkAgg
import matplotlib.ticker as ticker
import tkinter as tk


def main():
    root = tk.Tk()
    root.title("Graph")
    fig = plt.figure(figsize=(16, 16))
    ax_1 = fig.add_subplot(111)

    ax_1.set_xlabel("Елементів ", fontsize=20)
    ax_1.set_ylabel("Операцій", fontsize=20)
    ax_1.set_title("Шейкерна сортировка", fontsize=32)
    ax_1.grid(which='major',
              color='k')
    ax_1.minorticks_on()
    ax_1.grid(which='minor',
              color='gray',
              linestyle=':')
    ax_1.tick_params(axis='both',
                     which='major',
                     labelsize=12)
    x, y = getDataFromFile()
    ax_1.plot(x, y, color="orange")
    x1, y1 = [i for i in range(len(x))], [i ** 2 for i in range(len(y))]
    ax_1.plot(x1, y1, color="red")
    numOfElemMin = x1[0]
    numOfElemMax = x1[-1]
    numOfOperMin = numOfElemMin ** 2
    numOfOperMax = numOfElemMax ** 2
    ax_1.xaxis.set_major_locator(ticker.MultipleLocator((numOfElemMax - numOfElemMin) / 10))
    ax_1.set_xlim([numOfElemMin, numOfElemMax])
    ax_1.yaxis.set_major_locator(ticker.MultipleLocator((numOfOperMax - numOfOperMin) / 10))
    ax_1.set_ylim([numOfOperMin, numOfOperMax])
    canvas = FigureCanvasTkAgg(fig, root)
    canvas.get_tk_widget().pack()
    root.mainloop()
    plt.savefig('graph1.png')


def getDataFromFile():
    try:
        f = open(r"C:\Users\Carambula\Desktop\Учёба\АМО\SecondLab\Assets\Sort analyse.txt", "r")
        data = f.read()
        f.close()
    except FileNotFoundError:
        print("File 11.txt not found")
        sys.exit()
    tempList = data.split()
    listOfOper = list()
    listOfElem = list()
    lim = int(len(tempList) / 1)
    if lim % 2 == 1:
        lim -= 1
    for i in range(lim):
        if i % 2 == 0:
            listOfOper.append(tempList[i])
        else:
            listOfElem.append(tempList[i])

    for i in range(len(listOfElem)):
        listOfElem[i] = int(listOfElem[i])

    for i in range(len(listOfOper)):
        listOfOper[i] = int(listOfOper[i])

    return listOfOper, listOfElem


if __name__ == '__main__':
    main()
