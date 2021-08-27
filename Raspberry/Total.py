import RPi.GPIO as GPIO
import time
import datetime as dt
from typing import OrderedDict, Sequence
import random
import paho.mqtt.client as mqtt
import json

convey = 4 # Raspberry pi PIN 4
triggerPin = 20
echoPin = 21
motor = 26
sortmotor = 16
s2 = 23 # Raspberry Pi Pin 23
s3 = 24 # Raspberry Pi Pin 24
out = 25 # sensing pin 25

GPIO.setwarnings(False)
GPIO.setmode(GPIO.BCM)
GPIO.setup(convey, GPIO.OUT)
GPIO.setup(triggerPin, GPIO.OUT)
GPIO.setup(echoPin, GPIO.IN)
GPIO.setup(motor, GPIO.OUT)
GPIO.setup(sortmotor, GPIO.OUT)
GPIO.setup(out,GPIO.IN, pull_up_down=GPIO.PUD_UP)
GPIO.setup(s2,GPIO.OUT)
GPIO.setup(s3,GPIO.OUT)
cycles = GPIO.PWM(motor, 50)
sort = GPIO.PWM(sortmotor, 50)

dev_id = 'MACHINE01'
broker_address = '210.119.12.92'
pub_topic = 'factory1/machine1/data/'

#mqtt inti
print('MQTT Client')
client2 = mqtt.Client(dev_id)
client2.connect(broker_address)
print('MQTT Client connected')

global end

def Sensing():
    try:
        while True:
            GPIO.output(triggerPin,1)
            time.sleep(0.00001)
            GPIO.output(triggerPin,0)

            while GPIO.input(echoPin) == 0:
                start = time.time()
            while GPIO.input(echoPin) == 1:
                stop = time.time()

            rtTotime = stop - start
            distance = round((rtTotime * (34000/2)),2)
            time.sleep(0.5)
            print(distance)

            if (distance < 7):
                GPIO.output(convey, 0)
                print("가공 중")
                startTime, endTime, workTime, prepareTime = Work()
                print("가공 완료")
                print("불량검수")
                defect = Defect()
                print("불량검수 완료")
                GPIO.output(convey, 1)
                print(startTime, endTime, workTime, prepareTime, defect)
                send_data(startTime, endTime, workTime, prepareTime, defect)
                time.sleep(7)
                if(defect != "Sucess"):
                    Sorting()
                

    except KeyboardInterrupt:
        print("종료")
        End()

def Work():
        global end
        start = time.time()
        startTime = dt.datetime.now().strftime('%Y-%m-%d %H:%M:%S')
        prepareTime = round(start - end,2) - 3
        num = random.randrange(2,4)
        for i in range(num):
            cycles.start(5)
            time.sleep(2)
            cycles.ChangeDutyCycle(11)
            time.sleep(2)
        end = time.time()
        endTime = dt.datetime.now().strftime('%Y-%m-%d %H:%M:%S')
        workTime = round(end - start,2)
        return startTime, endTime, workTime, prepareTime

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
    return result

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
    sort.ChangeDutyCycle(7)
    time.sleep(1)
    sort.ChangeDutyCycle(10)
    time.sleep(1)


def send_data(startTime, endTime, workTime, prepareTime, defect):
    
    currtime = dt.datetime.now().strftime('%Y-%m-%d %H:%M:%S')
    #json data gen
    raw_data = OrderedDict()
    raw_data['DEV_ID'] = dev_id
    raw_data['PRC_TIME'] = currtime
    raw_data['PRC_Start'] = startTime
    raw_data['PRC_End'] = endTime
    raw_data['PRC_Work'] = workTime
    raw_data['PRC_Prepare'] = prepareTime
    raw_data['PRC_Total'] = workTime + prepareTime
    raw_data['PRC_Defect'] = defect

    pub_data = json.dumps(raw_data, ensure_ascii=False, indent='\t')
    print(pub_data)
    #mqtt_publish
    client2.publish(pub_topic, pub_data)

def End():
    GPIO.output(convey, 0)
    GPIO.output(triggerPin, 0)
    GPIO.cleanup()

if(__name__=='__main__'):
    end = time.time()
    GPIO.output(convey, 1)
    sort.start(10)
    Sensing()