#include <SoftwareSerial.h>
#include <AFMotor.h>       // L293D 모터 드라이브 라이브러리

AF_DCMotor motor_1(1);     // 모터 1
AF_DCMotor motor_2(2);     // 모터 2
AF_DCMotor motor_3(3);     // 모터 3
AF_DCMotor motor_4(4);     // 모터 4


void setup() {
  motor_1.setSpeed(250);    // 모터 1 속도 설정
  //motor_1.run(RELEASE);     // 모터 1 돌리지 않는 상태

  motor_2.setSpeed(250);    // 모터 2 속도 설정
  //motor_2.run(RELEASE);     // 모터 2 stop

  motor_3.setSpeed(250);    // 모터 3 속도 설정
  //motor_3.run(RELEASE);     // 모터 3 stop

  motor_4.setSpeed(250);    // 모터 4 속도 설정
  //motor_4.run(RELEASE);     // 모터 4 stop
}

void loop() {
  GO_FORWARD(); //전진
 // RELEASE_ALL(); // 정지

  TURN_LEFT(); //좌회전
 // RELEASE_ALL(); //정지

  TURN_RIGHT(); //우회전
  //RELEASE_ALL(); //정지

  GO_BACK(); //후진
//  RELEASE_ALL(); //정지

  delay(2000);

}


void GO_FORWARD() {
  motor_1.run(FORWARD);
  motor_2.run(FORWARD);
  motor_3.run(FORWARD);
  motor_4.run(FORWARD);
}

void TURN_LEFT() {
  motor_1.run(FORWARD);
  motor_2.run(FORWARD);
  motor_3.run(FORWARD);
  motor_4.run(FORWARD);
}

void TURN_RIGHT() {
  motor_1.run(FORWARD);
  motor_2.run(FORWARD);
  motor_3.run(FORWARD);
  motor_4.run(FORWARD);
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
