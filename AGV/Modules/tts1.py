import RPi.GPIO as GPIO
import time
import socket
import os

PIRP = 22
IRP = 26

GPIO.setmode(GPIO.BCM)
GPIO.setup(IRP, GPIO.IN) #장애물 감지 센서(IR)
GPIO.setup(PIRP, GPIO.IN,pull_up_down = GPIO.PUD_DOWN) #인체감지센서(PIR)
# GPIO 22 pin Direction is INPUT set and Pull Down
# becouse IR sensor is port to receive human detection signal, so necessarily set Pull Down
# if set Pull up human detection signal is not off 

#def beep(): # 부저 코드
#    GPIO.output(BP, True)
#    time.sleep(0.5)ython 3.7.3 (/usr/bin/python3)
#    GPIO.output(BP, False)
#    time.sleep(0.5)

#말하기 함수 정의
def speak(optinm, msg):
    os.system("espeak {} '{}'".format(option, msg))

# 출력 데이터 정의
detected = {
    'where' : '에',
    'what' : '가 탐지되었습니다.'
}

#옵션 및 메세지 정의
option = '-v ko+f3 -s 120 -p 95'
msg = '후방'+ detected['where'] + ' 물체'+ str(detected['what'])
mon = '로봇이동중입니다. 비켜주세요.'

try:
    print("module test [ CTRL+C to exit ]")
    time.sleep(3)
    print("ready")

    while True:
        if GPIO.input(PIRP) == 0 or GPIO.input(IRP) == 0:
            if GPIO.input(IRP) == 0:
                #말하기 실행
                print('espeak', option, msg)
                speak(option, msg)
                print("detected back")
                time.sleep(1)
                #beep()
                #sendPacket()

            elif GPIO.input(PIRP) == 0:
                 t=time.localtime()
                 speak(option, mon)
                 print(("%d:%d:%d motion detected!") % (t.tm_hour, t.tm_min, t.tm_sec))
                 time.sleep(1)        

except KeyboardInterrupt:
    print("quit")
    GPIO.cleanup()