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

GPIO.setwarnings(False)
GPIO.setmode(GPIO.BCM)
GPIO.setup(convey, GPIO.OUT)
GPIO.setup(triggerPin, GPIO.OUT)
GPIO.setup(echoPin, GPIO.IN)
GPIO.setup(motor, GPIO.OUT)
cycles = GPIO.PWM(motor, 50)

dev_id = 'MACHINE01'
broker_address = '210.119.12.92'
pub_topic = 'factory1/machine1/data/'

#mqtt inti
print('MQTT Client')
client2 = mqtt.Client(dev_id)
client2.connect(broker_address)
print('MQTT Client connected')

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
            #print(distance)

            if (distance < 10):
                GPIO.output(convey, 0)
                print("가공 중")
                Work()
                print("가공 완료")
                GPIO.output(convey, 1)
                time.sleep(2)

    except KeyboardInterrupt:
        print("종료")
        End()

def Work():
        start = time.time()
        startTime = dt.datetime.now().strftime('%Y-%m-%d %H:%M:%S')
        num = random.randrange(2,4)
        for i in range(num):
            cycles.start(5)
            time.sleep(2)
            cycles.ChangeDutyCycle(11)
            time.sleep(2)
        end = time.time()
        endTime = dt.datetime.now().strftime('%Y-%m-%d %H:%M:%S')
        workTime = round(end - start,2)
        send_data(startTime, endTime, workTime)
        #cycles.stop()

def send_data(startTime, endTime, workTime):
    
    currtime = dt.datetime.now().strftime('%Y-%m-%d %H:%M:%S')
    #json data gen
    raw_data = OrderedDict()
    raw_data['DEV_ID'] = dev_id
    raw_data['PRC_TIME'] = currtime
    raw_data['PRC_Start'] = startTime
    raw_data['PRC_End'] = endTime
    raw_data['PRC_Work'] = workTime
    raw_data['PRC_Defect'] = "Work"

    pub_data = json.dumps(raw_data, ensure_ascii=False, indent='\t')
    print(pub_data)
    #mqtt_publish
    client2.publish(pub_topic, pub_data)

def End():
    GPIO.output(convey, 0)
    GPIO.output(triggerPin, 0)
    GPIO.cleanup()

if(__name__=='__main__'):
    GPIO.output(convey, 1)
    Sensing()