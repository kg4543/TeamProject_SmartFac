# Raspberry Pi code

```python
from serial import Serial
import time

ser = Serial('/dev/ttyACM0', 9600)

while True:
    val = input()

    if val == '1': #아두이노 스크린에서 1를 입력하면 실행된다.
        val = val.encode('utf-8')
        ser.write(val)
        print("Hi")
        time.sleep(1)
```



# Arduino Uno Code

```c
int input;
void setup() {
  Serial.begin(9600);
}

void loop() {
  while(Serial.available())
  {
    input = Serial.read();
  }
  if(input == '1')
  {
    Serial.println("수신");
    return;
  }
}
```



### 아두이노 DC모터 시리얼 통신 테스트용 라즈베리파이 코드

```python
from serial import Serial
import time

ser = Serial('/dev/ttyACM0', 9600)

while True:
    val = input()

    if val == '1':
        val = val.encode('utf-8')
        ser.write(val)
        print("Let's Go")
        time.sleep(1)
        
    elif val == '2':
        val = val.encode('utf-8')
        ser.write(val)
        print("Stop")
        time.sleep(1)
        
    elif val == '3':
        val = val.encode('utf-8')
        ser.write(val)
        print("Turn Right")
        time.sleep(1)
        
    elif val == '4':
        val = val.encode('utf-8')
        ser.write(val)
        print("Turn Left")
        time.sleep(1)
        
    elif val == '5':
        val = val.encode('utf-8')
        ser.write(val)
        print("BackWard")
        time.sleep(1)
        
```



# 실제 아두이노 DC모터 제어에 사용한 코드

```c
#include <SoftwareSerial.h>
#include <AFMotor.h>       // L293D 모터 드라이브 라이브러리

AF_DCMotor motor_1(1);     // 모터 1
AF_DCMotor motor_2(2);     // 모터 2
AF_DCMotor motor_3(3);     // 모터 3
AF_DCMotor motor_4(4);     // 모터 4


void setup() {
  Serial.begin(9600);

  motor_1.setSpeed(250); //set motor1 speed
  //motor_1.run(RELEASE); //motor1 stop 

  motor_2.setSpeed(250); //set motor2 speed 
  //motor_2.run(RELEASE); //motor2 stop

  motor_3.setSpeed(250); //set motor3 speed 
  //motor_3.run(RELEASE); //motor3 stop

  motor_4.setSpeed(250); //set motor4 speed  
  //motor_4.run(RELEASE); //motor4 stop

  Serial.println("Number for Motor Control"); // Initial words
  Serial.println("1. Let's Go");
  Serial.println("2. Stop");
  Serial.println("3. Turn Right");
  Serial.println("4. Turn Left");
  Serial.println("5. BackWard");
}

void loop() {
  int input;

  while(Serial.available())
  {
    input = Serial.read();
  }

  if(input == '1') 
  { 
    Serial.println("Let's Go");
    GO_FORWARD();
    delay(2000); 
  }

  else if(input == '2')
  {
    Serial.println("Stop");
    RELEASE_ALL(); // stop slowly
    delay(2000);
  }

  else if(input == '3')

  {
    Serial.println("Turn Right");
    TURN_RIGHT(); //Right
    delay(2000);
  }

  else if(input == '4')
  {
    Serial.println("Turn Left");
    TURN_LEFT(); //left
    delay(2000);
  }

  else if(input == '5')
  {
    Serial.println("BackWard");
    GO_BACK();
    delay(2000);
  }
  else{
  }
}

void GO_FORWARD() {
  motor_1.run(FORWARD);
  motor_2.run(FORWARD);
  motor_3.run(FORWARD);
  motor_4.run(FORWARD);
}

void TURN_RIGHT() {
  motor_1.run(BACKWARD);
  motor_2.run(BACKWARD);
  motor_3.run(FORWARD);
  motor_4.run(FORWARD);
}

void TURN_LEFT() {
  motor_1.run(FORWARD);
  motor_2.run(FORWARD);
  motor_3.run(BACKWARD);
  motor_4.run(BACKWARD);
}

void GO_BACK() {
  motor_1.run(BACKWARD);
  motor_2.run(BACKWARD);
  motor_3.run(BACKWARD);
  motor_4.run(BACKWARD);
}  

// 부드럽게 속도 줄이는 함수
void RELEASE_ALL() {
  delay(5000); //5초 대기
  motor_1.run(RELEASE);
  motor_2.run(RELEASE);
  motor_3.run(RELEASE);
  motor_4.run(RELEASE);
  delay(5000);
}
```

