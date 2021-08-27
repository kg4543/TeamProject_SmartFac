import RPi.GPIO as GPIO
import time
import socket
import os

PIRP = 22
BP = 17
IRP = 26
TGP = 23
ECP = 24

Melody1 = [262,294,392,523]

GPIO.setmode(GPIO.BCM)

GPIO.setup(TGP, GPIO.OUT) #triggerPin = 초음파 반사 = 출력모드로 설정
GPIO.setup(ECP, GPIO.IN) #echoPin = 반사판 IN = 입력모드로 설정
GPIO.setup(IRP, GPIO.IN) #장애물 감지 센서(IR)
GPIO.setup(BP, GPIO.OUT) #부저
GPIO.setup(PIRP, GPIO.IN,pull_up_down = GPIO.PUD_DOWN) #인체감지센서(PIR)
# GPIO 22 pin Direction is INPUT set and Pull Down
# becouse IR sensor is port to receive human detection signal, so necessarily set Pull Down
# if set Pull up human detection signal is not off 

Buzz = GPIO.PWM(BP,440) #부저 PWM

#def beep(): # 부저 코드
#    GPIO.output(BP, True)
#    time.sleep(0.5)ython 3.7.3 (/usr/bin/python3)
#    GPIO.output(BP, False)
#    time.sleep(0.5)

def sendpacket(): #google assistant conect 구글 어시스턴트 연결
    #Create a UDP socket
    sock=socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    R3_address=(('google assistant ip', 4210))
    msge = "Hey google"
    #UDP - send packet
    sock.connect((R3_address))
    print('Send packet')
    sock.send(msge.encode(encoding='utf_8', errors='strict'))
    sock.close()

try:
    print("module test [ CTRL+C to exit ]")
    time.sleep(3)
    print("ready")

    while True:
        GPIO.output(TGP, GPIO.LOW) #초음파 센서 트리거핀
        time.sleep(0.0001)
        GPIO.output(TGP, GPIO.HIGH) 
        
        #에코핀이 on되는 시점을 시작시간으로 잡기
        while GPIO.input(ECP) == 0: 
            start = time.time()
        #에코핀이 off되는 시점을 반사파 수신 시간으로 잡기
        while GPIO.input(ECP) == 1:
            stop = time.time()

        #Calculate Pulse length
        rtTotime = stop - start
        distance = rtTotime * 34000/2
        print("distance: %.2fcm" % distance)

        if GPIO.input(PIRP) == 0 or GPIO.input(IRP) == 0 or distance <= 5:
            if distance <= 5:
                 #os.system('echo"It is short distance"|festival --tts')
                 Buzz.start(50)
                 #for i in range(0, len(Melody1)):
                 #Buzz.ChangeFrequency(Melody1[i]):
                 time.sleep(2)
                 Buzz.stop()

            elif GPIO.input(IRP) == 0:
                 Buzz.start(50)
                 for i in range(0, len(Melody1)):
                    Buzz.ChangeFrequency(Melody1[i])
                 print("detected back")
                 time.sleep(1)
                 #beep()
                 #sendPacket()
                 Buzz.stop()
                
            elif GPIO.input(PIRP) == 0:
                 t=time.localtime()      
                 print(("%d:%d:%d motion detected!") % (t.tm_hour, t.tm_min, t.tm_sec))
                 time.sleep(1)         

except KeyboardInterrupt:
    print("quit")
    GPIO.cleanup()
