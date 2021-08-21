#include <Servo.h> // 헤더파일 포함

Servo Gripper; //Gripper 변수 선언
Servo Tilt; //Tilt 변수 선언
Servo Lift; //Lift 변수 선언
int angle; // 앵글 변수 만들고 0으로 초기화

// 입출력 핀 설정
int Gripper = 3;
int Tilt = 5;
int Lift = 7;

void setup() {
	Gripper.attach(Gripper); //servo1에 입출력 3번핀 지정
	Tilt.attach(Tilt); //servo2에 입출력 5번핀 지정
	Lift.attach(Lift); // servo3에 입출력 7번핀 지정

	// 초기 각도값 설정
	Gripper.write(90);
	Tilt.write(90);
	Lift.write(90);
}

void loop() {
	
	//0.01초 마다 모터 각도를 1씩 증가시킵니다.
	for (int i = 0; i <= 180; i++) {
		Gripper.write(i);
		delay(10);
	}
}