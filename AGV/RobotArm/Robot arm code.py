# -*- coding:utf-8 -*- #한글입력
import RPi.GPIO as GPIO
import time

Gp = 21 
Tp = 20
Lp = 16

GPIO.setmode(GPIO.BCM) #gpio 모드 셋팅
GPIO.setmode(Gp.GPIO.OUT) # Gripper pin
GPIO.setmode(Tp.GPIO.OUT) # Tilt(neck) pin
GPIO.setmode(Lp.GPIO.OUT) # Lift pin

Gp = GPIO.PWM(Gp, 50) #펄스폭변조 세팅 핀, 주파수
Tp = GPIO.PWM(TP, 50)
LP = GPIO.PWM(LP, 50)

Gp.start(0)
Tp.start(0)
Lp.start(0)

val = 1
# val은 ms 단위로 했기때문에 0.6.~ 2 사이를 0.1씩 변하며 구동됨.
inc = 0.1

try:
    while True:
        gpio.outut(Gp, False)
        time.sleep(0.05)
        gpio.output(Gp, True)
        time.sleep((20-val)/1000.0) # = 0.05

        val += inc #0.1
        time.sleep(0.05)

        #val은 ms단위로 하였기에 0.6~2 사이를 0.1씩 변하며 구동된다
        # 0.6 or 2에 다다랐을 경우 if문이 동작해서 inc에 -1를 곱해준다.
        if val> 2 or val < 0.6:
            inc*=-1

except KeyboardInterrupt:
    GPIO.cleanup()