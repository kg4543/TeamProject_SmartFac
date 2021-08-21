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



