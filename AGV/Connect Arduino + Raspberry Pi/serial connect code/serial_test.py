import RPi.GPIO as GPIO
import time
import serial #pyserial 라이브러리 임포트

#아두이노와 연결된 시리얼 활성화
ser=serial.Serial('/dec/ttyACM0', 9600) #아두이노와 연결된 포트 ACM0

#pin 넘버링을 BCM방식을 사용한다.
GPIO.setmode(GPIO.BCM)

#후방감지센서
TGP=23
ECP=24
GPIO.setup(TGP,GPIO.OUT) #초음파 내보냄 = 출력모드
GPIO.setup(ECP,GPIO.IN) #반사파 수신 = 입력모드

try:
    while True:
        GPIO.output(TGP, GPIO.LOW)
        time.sleep(0.0001)
        GPIO.output(TGP, GPIO.HIGH)

        #에코핀이 on되는 시점을 시작 시간으로 잡는다.
        while GPIO.input(ECP) == 0:
            start = time.time()
        #에코핀이 다시 off되는 시점을 반사파 수신 시간으로 잡는다.
        while GPIO.input(echoPin) == 1:
            stop = time.time()
        #Calculate pulse length
        rtTotime = stop - start
        #초음파는 반사파이기 때문에 실제 이동 거리는 2배이다. 따라서 2로 나눈다.
        #음속은 편의상 340m/s로 계산한다. 현재 온도를 반영해서 보정할 수 있다.

        distance = rtTotime * 34000/2
        print("distance: %.2f cm" % distance)

        if 10 < distance <= 30:
            ser.write('O')
            time.sleep(1)
        elif 5<distance<= 10:
            ser.write('T')
            time.sleep(1)
        else:
            ser.write('S')
            time.sleep(0.5)

except KeyboardInterrupt:
    GPIO.cleanup()