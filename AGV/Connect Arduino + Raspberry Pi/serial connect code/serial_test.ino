#include <SoftwareSerial.h>
#include <AFMotor.h>       // L293D 모터 드라이브 라이브러리

AF_DCMotor motor_1(1);     // 모터 1
AF_DCMotor motor_2(2);     // 모터 2
AF_DCMotor motor_3(3);     // 모터 3
AF_DCMotor motor_4(4);     // 모터 4

void setup(){
  Serial.begin(9600);
  motor_1.setSpeed(250); //set motor1 speed 
  motor_2.setSpeed(250); //set motor2 speed 
  motor_3.setSpeed(250); //set motor3 speed 
  motor_4.setSpeed(250); //set motor4 speed 
}

void loop(){
  char ch;
  if(Serial.available()){ //if serial value
    ch = Serial.read();
    if(ch == 'S') {
      GO_FORWARD(); //전진
    }
    else if(ch == 'T'){
      RELEASE_ALL();
    }
    else if(ch == 'R'){
      TURN_RIGHT(); //우회전
    }
    else if(ch == 'L'){
      TURN_LEFT(); //좌회전
    }
    else if(ch == 'O'){
      RELEASE_ALL(); // 부드럽게 정지
    }
    else if(ch=='R'){
      GO_BACK(); //후진
    }
  }
}
  
void GO_FORWARD() {
  motor_1.run(FORWARD);
  motor_2.run(FORWARD);
  motor_3.run(FORWARD);
  motor_4.run(FORWARD);
}

void TURN_LEFT() {
  motor_1.run(BACKWARD);
  motor_2.run(BACKWARD);
  motor_3.run(FORWARD);
  motor_4.run(FORWARD);
}

void TURN_RIGHT() {
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