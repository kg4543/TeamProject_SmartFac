import RPi.GPIO as GPIO
import time

s2 = 23 # Raspberry Pi Pin 23
s3 = 24 # Raspberry Pi Pin 24
out = 25 # sensing pin 25
sortmotor = 16
# NUM_CYCLES = 10

GPIO.setmode(GPIO.BCM)
GPIO.setup(out,GPIO.IN, pull_up_down=GPIO.PUD_UP)
GPIO.setup(s2,GPIO.OUT)
GPIO.setup(s3,GPIO.OUT)
GPIO.setup(sortmotor, GPIO.OUT)
sort = GPIO.PWM(sortmotor, 50)
 
def Defect():
   result = ""

   while True:
    r = read_value(GPIO.LOW, GPIO.LOW)
    time.sleep(0.1)
    g = read_value(GPIO.HIGH, GPIO.HIGH)
    time.sleep(0.1)
    b = read_value(GPIO.LOW, GPIO.HIGH)

    print('red = {0}, green = {1}, blue = {2}'.format(r, g, b))

    if r > 200000 and g > 200000 and b > 180000:
        result = "BLACK"
    else:
        if (b < g) and (b < r):
            result = "Sucess" #Blue
        elif (g < b) and (g < r):
            result = "Fail" #Green
        elif (r < g) and (r < b):
            result = "Fail" #Red
    print(result)
    if(result == "Sucess"):
        Sorting()

def read_value(a0, a1):
   GPIO.output(s2, a0)
   GPIO.output(s3, a1)
   # 센서를 조정할 시간을 준다
   time.sleep(0.1)
   # 전체주기 웨이팅(전체주기로 계산됨)
   GPIO.wait_for_edge(out, GPIO.FALLING)
   GPIO.wait_for_edge(out, GPIO.RISING)
   start = time.time()
   GPIO.wait_for_edge(out, GPIO.FALLING)
   return (time.time() - start) * 1000000

#모터로 불량품 쳐내기
def Sorting():
    sort.ChangeDutyCycle(5)
    time.sleep(3)
    sort.ChangeDutyCycle(10)
    time.sleep(1)

if __name__=='__main__':
   try:
       sort.start(5)
       result = Defect()
   except KeyboardInterrupt:
       GPIO.cleanup()