# -*- coding:utf-8 -*- #한글입력

import RPi.GPIO as GPIO
import time

headpin = 21
neckpin = 20
Upin = 16

GPIO.setmode(GPIO.BCM) #gpio 모드 셋팅
GPIO.setup(headpin, GPIO.OUT)
GPIO.setup(neckpin, GPIO.OUT)
GPIO.setup(Upin, GPIO.OUT)

hp = GPIO.PWM(headpin, 50) #펄스폭변조 세팅 핀, 주파수
np = GPIO.PWM(neckpin, 50)
up = GPIO.PWM(Upin, 50)

hp.start(0)
np.start(0)
up.start(2.5)


#각도별로 회전
try:
   while True:

      hp.ChangeDutyCycle(2.5)
      print("펼쳐")
      time.sleep(3)

      up.ChangeDutyCycle(10)
      print("내려가")
      time.sleep(3)

      hp.ChangeDutyCycle(12)
      print("잡아")
      time.sleep(3)

      up.ChangeDutyCycle(2.5)
      print("올려~!")
      time.sleep(3)

      up.ChangeDutyCycle(10)
      print("내려갓!")
      time.sleep(3)

      hp.ChangeDutyCycle(2.5)
      print("놔")
      time.sleep(3)

      up.ChangeDutyCycle(2.5)
      print("다시올라와")
      time.sleep(3)
      
      hp.ChangeDutyCycle(12)
      print("close")
      time.sleep(3)

except KeyboardInterrupt:
   GPIO.cleanup()