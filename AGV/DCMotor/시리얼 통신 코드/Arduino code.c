#include <SoftwareSerial.h>
#include <AFMotor.h>       // L293D 모터 드라이브 라이브러리

AF_DCMotor motor_1(1);     // 모터 A
AF_DCMotor motor_2(2);     // 모터 B
AF_DCMotor motor_3(3);     // 모터 C
AF_DCMotor motor_4(4);     // 모터 D

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

  Serial.println("Number for Motor Control"); //선택 목록
  Serial.println("1. Let's Go");
  Serial.println("2. Stop");
  Serial.println("3. Turn Right");
  Serial.println("4. Turn Left");
  Serial.println("5. BackWard");
}

void loop() {
  int input;

  while(Serial.available()) //serial값이 있을때 while문 실행
  {
    input = Serial.read();
	// 라즈베리파이에서 오는 serial값을 받아서 input에 대입
  }

  if(input == '1') //input값이 1일때
  { 
    Serial.println("Let's Go"); //출력
    GO_FORWARD(); // 전진
    //delay(2000); 
  }

  else if(input == '2')//input값이 2일때
  {
    Serial.println("Stop");
    RELEASE_ALL(); // 천천히 멈추기
    //delay(2000);
  }

  else if(input == '3')//input값이 3일때

  {
    Serial.println("Turn Right");
    TURN_RIGHT(); //우회전
    //delay(2000);
  }

  else if(input == '4')//input값이 4일때
  {
    Serial.println("Turn Left");
    TURN_LEFT(); //좌회전
    //delay(2000);
  }

  else if(input == '5')//input값이 5일때
  {
    Serial.println("BackWard");
    GO_BACK(); //후진
    //delay(2000);
  }
  else{
  }
}

// 전진
void GO_FORWARD() { 
  motor_1.run(FORWARD);
  motor_2.run(FORWARD);
  motor_3.run(FORWARD);
  motor_4.run(FORWARD);
}

//우회전
void TURN_RIGHT() { 
  motor_1.run(BACKWARD);
  motor_2.run(BACKWARD);
  motor_3.run(FORWARD);
  motor_4.run(FORWARD);
}

//좌회전
void TURN_LEFT() {  
  motor_1.run(FORWARD);
  motor_2.run(FORWARD);
  motor_3.run(BACKWARD);
  motor_4.run(BACKWARD);
}


//후진
void GO_BACK() { 	
  motor_1.run(BACKWARD);
  motor_2.run(BACKWARD);
  motor_3.run(BACKWARD);
  motor_4.run(BACKWARD);
}  

// 부드럽게 속도 줄이는 함수
//정지
void RELEASE_ALL() {	
  motor_1.run(RELEASE);
  motor_2.run(RELEASE);
  motor_3.run(RELEASE);
  motor_4.run(RELEASE);
}
