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
