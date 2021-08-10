
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
up.start(0)

#각도별로 회전
try:
	while True:

		up.ChangeDutyCycle(15)
		print("내려가")
		time.sleep(3)

		hp.ChangeDutyCycle(0)
		print("펼쳐")
		time.sleep(3)

		hp.ChangeDutyCycle(10)
		print("잡아")
		time.sleep(3)

		np.ChangeDutyCycle(15)
		print("머리내려")
		time.sleep(3)

		up.ChangeDutyCycle(1)
		print("올려~!")
		time.sleep(3)

		up.ChangeDutyCycle(20)
		print("내려갓!")
		time.sleep(3)

		hp.ChangeDutyCycle(5)
		print("놔")
		time.sleep(3)

		np.ChangeDutyCycle(0.1)
		print("머리들어")
		time.sleep(3)

		up.ChangeDutyCycle(1)
		print("다시올라와")
		time.sleep(3)

		break

except KeyboardInterrupt:
	GPIO.cleanup()


# 지금 문제점이
# 1. 로봇팔의 속도가 너무빠름 => for문으로 속도제어 필요
# 2. head 모터부분에서 고장 발생 => 모터가 반대로 돌지 않음
