### Raspberry Pi Code

```python
from serial import Serial
import paho.mqtt.client as mqtt
import RPi.GPIO as GPIO
import time
import socket
import os

ser= Serial('/dev/ttyACM1', 9600)

#mqtt이용한 통신 메서드(서버로부터 publish message를 받을 때 호출되는 콜백)
def on_message(client, userdata, message):
    # Topic에 연결하여 메세지를 수신한다(Subscribe).
    topic=str(message.topic)                        
    message = str(message.payload.decode("utf-8"))
    print(topic+message)
    
    # 해당 Topic의 메세지에 대한 동작 수행
    if message == 'G':          # 전진
        sendData("8")
        
    elif message == 'B':        # 복귀
        sendData("2")
        
    elif message == 'S':        # 정지
        sendData("5")
        
    elif message == 'L':        # 좌회전
        sendData("4")
    elif message == 'R':        # 우회전
        sendData("6")
        
    elif message == '1':	# 뒤로 좌회전
        sendData("1")
        
    elif message == '2':	# 뒤로 우회전
        sendData("3")

    else: pass


### mqtt 통신 위한 객체 생성 및 설정
broker_address='210.119.12.92'  # broker address
pub_topic = 'MOTOR/TEST/'       # topic
print("creating new instance")
client=mqtt.Client("P1")        # create new instance
print("connecting to broker")
client.connect(broker_address)  # connect to broker
client.subscribe(pub_topic)     # subscribe topic


# 콜백 설정
# client.on_connect = on_connect
# client.on_disconnect = on_disconnect
client.on_message = on_message

def sendData(val):
    val = val.encode('utf-8')
    ser.write(val)
    time.sleep(0.01)

try:
    client.loop_forever()
    print("AGV Start! [ CTRL+C to exit ]")
    time.sleep(1)
    print("ready") 

except KeyboardInterrupt:
    print("quit")
    GPIO.cleanup()
```





---

### Arduino Code

```C
// 시리얼 통신
#include <SoftwareSerial.h>

//모터 1
int MA1 = 3; //~3
int MA2 = 4; // 4

//모터 2
int MB1 = 6; //~6
int MB2 = 7; //7

//모터 3
int MC1 = 5; //~5
int MC2 = 8; // 8

//모터4
int MD1 = 11; // ~11
int MD2 = 12; // 12


void setup() {
  Serial.begin(9600);
  
  //핀모드 설정
  pinMode(MA1, OUTPUT);
  pinMode(MA2, OUTPUT);
  
  pinMode(MB1, OUTPUT);
  pinMode(MB2, OUTPUT);

  pinMode(MC1, OUTPUT);
  pinMode(MC2, OUTPUT);

  pinMode(MD1, OUTPUT);
  pinMode(MD2, OUTPUT);

  // 숫자키 선택 목록
  Serial.println("Number for Motor Control");
  Serial.println("8. Go"); //전진
  Serial.println("5. Stop"); //멈춤
  Serial.println("2. BackWard"); //후진
  Serial.println("6. Turn Right"); //우회전
  Serial.println("4. Turn Left"); //좌회전
  Serial.println("1.Back Left"); //왼쪽으로 후진
  Serial.println("3.Back Right"); //오른쪽으로 후진

}

void loop() {
  char input;
  
  while(Serial.available()) // serial 값이 있을때 While문 실행
  {
    input = Serial.read();
    // 라즈베리파이에서 오는 Serial값을 받아서 input에 대입
  }

  if(input == '8') // input값이 8일때
  {
    Serial.println("Go"); // 출력
    GO_Forward(); // 전진
  }
  else if(input == '5')//input값이 5일때
  {
    Serial.println("Stop");
    Stop();//브레이크
  }
  else if(input == '2')//input값이 2일때
  {
     Serial.println("BackWard");
     GO_Back(); // 후진
  }
  else if(input == '6') //input값이 6일때
  {
    Serial.println("Turn Right"); 
    Turn_Right(); //우회전
  }
  else if(input == '4')//input값이 4일때
  {
    Serial.println("Turn Left");
    Turn_Left(); //좌회전
  }
  else if(input == '1')//input값이 1일때
  {
    Serial.println("Back_Left"); 
    Back_Left(); //왼쪽으로 후진
  }
  else if(input == '3') //input값이 3일때
  {
    Serial.println("Back_Right"); 
    Back_Right(); //오른쪽으로 후진
  }
}

//정회전(전진)
void GO_Forward(){
  digitalWrite(MA1, HIGH);
  digitalWrite(MA2, LOW);
  
  digitalWrite(MB1, HIGH);
  digitalWrite(MB2, LOW);
  
  digitalWrite(MC1, HIGH);
  digitalWrite(MC2, LOW);

  digitalWrite(MD1, HIGH);
  digitalWrite(MD2, LOW);
}

// 멈추기(정지)
void Stop(){
  digitalWrite(MA1, LOW);
  digitalWrite(MA2, LOW);

  digitalWrite(MB1, LOW);
  digitalWrite(MB2, LOW);

  digitalWrite(MC1, LOW);
  digitalWrite(MC2, LOW);

  digitalWrite(MD1, LOW);
  digitalWrite(MD2, LOW);
}

// 후진
void GO_Back(){
  digitalWrite(MA1, LOW);
  digitalWrite(MA2, HIGH);
  
  digitalWrite(MB1, LOW);
  digitalWrite(MB2, HIGH);

  digitalWrite(MC1, LOW);
  digitalWrite(MC2, HIGH);

  digitalWrite(MD1, LOW);
  digitalWrite(MD2, HIGH);
}

// 우회전
//A,B = 정지
//C,D = 정회전
void Turn_Right(){
  digitalWrite(MA1, LOW);
  digitalWrite(MA2, LOW);
  
  digitalWrite(MB1, LOW);
  digitalWrite(MB2, LOW);

  digitalWrite(MC1, HIGH);
  digitalWrite(MC2, LOW);

  digitalWrite(MD1, HIGH);
  digitalWrite(MD2, LOW);

}

// 좌회전
//A,B = 정회전
//C,D = 정지
void Turn_Left(){
  digitalWrite(MA1, HIGH);
  digitalWrite(MA2, LOW);

  digitalWrite(MB1, HIGH);
  digitalWrite(MB2, LOW);

  digitalWrite(MC1, LOW);
  digitalWrite(MC2, LOW);

  digitalWrite(MD1, LOW);
  digitalWrite(MD2, LOW);
}


// 왼쪽으로 후진
//A,B = 역회전
//C,D = 정지
void Back_Left(){
  digitalWrite(MA1, LOW);
  digitalWrite(MA2, HIGH);
  
  digitalWrite(MB1, LOW);
  digitalWrite(MB2, HIGH);

  digitalWrite(MC1, LOW);
  digitalWrite(MC2, LOW);

  digitalWrite(MD1, LOW);
  digitalWrite(MD2, LOW);
}

// 오른쪽으로 후진
//A,B = 정지
//C,D = 역회전
void Back_Right(){
  digitalWrite(MA1, LOW);
  digitalWrite(MA2, LOW);

  digitalWrite(MB1, LOW);
  digitalWrite(MB2, LOW);

  digitalWrite(MC1, LOW);
  digitalWrite(MC2, HIGH);

  digitalWrite(MD1, LOW);
  digitalWrite(MD2, HIGH);
}
```

