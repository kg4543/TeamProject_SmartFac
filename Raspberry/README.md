# 가상공정 라인

<kbd>[![OEE](/Capture/OEEMonitoring.gif "OEE")](https://github.com/kg4543/TeamProject_SmartFac/blob/main/Raspberry/Total.py)</kbd> </br>

## RaspberryPi IoT 제어

- 물체 인식 후 컨베이어 제어 및 공정 실행
```Python
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
```

- 공정 데이터 추출 (공정 시간)
```python
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

```

- 공정 데이터 추출 (불량 검출)
```python
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
```

- paho.mqtt.client 라이브러리 활용한 데이터 송신
```python
dev_id = 'MACHINE01'
broker_address = '210.119.12.92'
pub_topic = 'factory1/machine1/data/'

#mqtt inti
print('MQTT Client')
client2 = mqtt.Client(dev_id)
client2.connect(broker_address)
print('MQTT Client connected')

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
```

## C# Monitor
- M2Mqtt 라이브러리를 활용한 데이터 수신
```C#
 MqttClient client;

private void InitConnectMqttBroker()
{
    var brokerAddress = IPAddress.Parse("210.119.12.92");
    client = new MqttClient(brokerAddress);
    client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
    client.Connect("Monitor");
    client.Subscribe(new string[] { "factory1/machine1/data/" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
}

Dictionary<string, string> currentData = new Dictionary<string, string>();

private void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
{
    var message = Encoding.UTF8.GetString(e.Message);
    currentData = JsonConvert.DeserializeObject<Dictionary<string, string>>(message);
    UpdateData(currentData);
    ShowData();
}
```
