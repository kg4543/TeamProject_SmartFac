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

try:
    print("module test [ CTRL+C to exit ]")
    time.sleep(3)
    print("ready")

    while True:
        if GPIO.input(PIRP) == 0 or GPIO.input(IRP) == 0:
            D = "Danger! "
            B = "detected behind, stay, away"
            H = " motion detected! stay, away"
            if GPIO.input(IRP) == 0:
                #말하기 실행
                os.system('echo %s | festival --tts' %D)
                os.system('echo %s | festival --tts' %B)
                print("detected behind")
                time.sleep(1)
                #beep()
                #sendPacket()

            elif GPIO.input(PIRP) == 0:
                t=time.localtime()
                os.system('echo %s | festival --tts' %D)
                os.system('echo %s | festival --tts' %H)
                print(("%d:%d:%d motion detected!") % (t.tm_hour, t.tm_min, t.tm_sec))
                time.sleep(1)        

except KeyboardInterrupt:
    print("quit")
    GPIO.cleanup()