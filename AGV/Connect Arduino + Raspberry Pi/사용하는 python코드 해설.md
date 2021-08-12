라즈베리에서 파이썬 코드 작성하기

```python
from pyfirmata import Arduion,until #pyfirmata쪽에서 아두이노, 유틸 관련된 라이브러리를 불러온다
import time # time를 불러온다

board=Arduion('아두이노 시리얼 포트명')
pin_button = board.get_pin('d:8:i') # 입력값을 받을 모듈 핀 ex) 버튼
it=util.lterator(board) 
it.start()
pin_button.enable_reporting()
#핀버튼 

while True:
    if pin_button.read():
        board.digital[13].write(1)
    else:
        board.digital[13].write(0)
        
    time.sleep(0.01)
```



---

```python
from pyfirmata import Arduino, util
```

이건 pyfirmata모듈에서 Arduino, util를 불러온다는것.



---



```python
board = Arduino('/dev/ttyACM0')
```

Arduino 관련 클래스를 초기화하는 명령어

('/dev/ttyACM0') = 아두이노 시리얼 포트명



---



```python
pin_button = board.get_pin('d:8:i')
```

Arduino.get_pin = 아두이노의 특정 핀과 관련된 클래스를 가지고 오는 명령어

('d:8:i') => '디지털(d) or 아날로그(a) : 핀번호 : 입력(i), 출력(o), PWM(p)'



---



```python
it=util.lterator(board)
it.start()
pin_button.enable_reportiong()
pin_button.read()
```

pyFirmata를 이용해 디지철이나 아날로그의 입력값을 읽을 때 이 코드를 넣어줘야한다.

#### 그리고 만약 더 이상 입력 핀에서 값을 읽지 않는다면 다음 명령어를 호출한다.

```python
pin_button.disable_reporting()
```