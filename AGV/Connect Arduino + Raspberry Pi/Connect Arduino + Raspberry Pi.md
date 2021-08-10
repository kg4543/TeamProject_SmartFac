# 라즈베리랑 아두이노 연동해서 사용하기

저는 아두이노 IDE를 설치해주고, 라즈베리에서 파이썬을 이용하여 아두이노를 제어하였습니다.



#### 먼저 아두이노 IDE를 라즈베리파이에 설치하였습니다.

1. 설치 전 라즈베리파이 패키지 업데이트 진행

```
~$ sudo apt-get update
~$ sudo apt-get upgrade
```

\* update와 upgrade의 차이

update는 라즈베리파이가 사용가능한 패키지의 목록을 업데이트 해주는 명령어이고,

upgrade는 라즈베리파이에 설치된 패키지들의 버젼을 최신버젼으로 만들어주는 명령어입니다.



2. 업데이트 진행이 끝나면 아두이노 IDE를 라즈베리파이에 설치하기   

```
~$ sudo apt-get install arduino
```

​	설치 중에 [y/n] 질문이 나타나며, y 를입력하면 이제 설치가 진행됩니다.



이제 설치가 완료되면 아두이노 IDE가 생성이 됩니다.



아두이노 IDE가 생성이 완료 후, 그림과 같이 아두이노의 usb 포트를 라즈베리에 연결해주면 됩니다.

![KakaoTalk_20210810_144453559](https://user-images.githubusercontent.com/77951853/128822047-707ec639-c6a4-4232-8880-18bb93061d90.jpg)


3.  연결이 완료가 되면 vnc에서 아두이노를 열고, 시리얼 포트를 확인하였습니다.

<img src="C:\Users\PKNU\AppData\Roaming\Typora\typora-user-images\image-20210810145005725.png" alt="image-20210810145005725" style="zoom: 67%;" />

포트는 ACM0으로 선택해주고, 보드는 본인이 사용하는 아두이노 보드를 선택해줍니다.

저는 아두이노 우노를 사용하였습니다.

<img src="C:\Users\PKNU\AppData\Roaming\Typora\typora-user-images\image-20210810145204643.png" alt="image-20210810145204643" style="zoom: 67%;" /> <img src="C:\Users\PKNU\AppData\Roaming\Typora\typora-user-images\image-20210810145628003.png" alt="image-20210810145628003" style="zoom:50%;" />



이렇게 설치 한 후 아두이노 단독으로 사용할 때처럼 실행시키면 똑같이 작동을 합니다.

사실 이렇게 진행하면 라즈베리랑 연동이 되긴 하지만 아두이노 단독으로 사용할 때와 큰 차이점은 없습니다.



#### 그래서 이번에는 라즈베리파이에서 파이썬을 이용하여 아두이노를 제어해보았습니다.



이 방법을 사용하려면 아두이노 스케치에 Firmata 코드를 업로드 해주어야 하며, 

이건 예제에 있는 Firmata -> StandardFirmata 코드를 아두이노에 업로드 해주었습니다. <img src="C:\Users\PKNU\AppData\Roaming\Typora\typora-user-images\image-20210810150423782.png" alt="image-20210810150423782" style="zoom: 67%;" /><img src="C:\Users\PKNU\AppData\Roaming\Typora\typora-user-images\image-20210810150454263.png" alt="image-20210810150454263" style="zoom: 80%;" />



업로드가 완료되면, 이제 아두이노는 라즈베리파이로부터 들어오는 데이터를 기다리게 됩니다.

여기서 방금 사용한 **'Firmata Protocol'**은

#### 사용자의 컴퓨터에서 아두이노 IDE를 사용하지 않고, 시리얼 통신을 이욯새 아두이노에게 명령을 직접 전달하는 프로토콜입니다.

그래서 아두이노와 사용자의 컴퓨터가 연결되면, 제가 원하는 언어 *ex) python  으로 아두이노를 제어할 수 있습니다.



여기까지 아두이노부분은 끝이 났고 이제 라즈베리파이 부분을 설정합니다.



먼저 만약 라즈베리에 파이썬이 없는 경우 파이썬을 설치해줘야합니다.

```
~$ sudo apt-get install python3
```



그리고, 라즈베리파이에서도 pyFirmata라는 라이브러리를 사용해야하며, 내부적으로 PySerial 라이브러리를 사용하기 때문에 PySerial를  설치해주었습니다.

여기서 PySerial은 깃허브를 이용해 소스를 관리하는 라이브러리이기 때문에 라즈베리파이에 git을 추가 설치해주어야 합니다.

다음 명령어들 차례대로 입력합니다.

```
~ $ sudo apt-get install python-serial //PySerial 라이브러리 설치
~ $ sudo apt get inatall git // git 설치
~ $ git clone https://github.com/tino/pyFirmata.git
~ $ cd pyFirmata
~ $ sudo python setup.py install
```

![](C:\Users\PKNU\AppData\Roaming\Typora\typora-user-images\image-20210810152238717.png) 

![image-20210810152318114](C:\Users\PKNU\AppData\Roaming\Typora\typora-user-images\image-20210810152318114.png) 

사진처럼 완료되면, 라즈베리파이로 아두이노를 제어할 준비는 끝이 납니다.



이제 라즈베리파이에서 터미널 창을 열어, 터미널 환경에서 python IDLE를 실행합니다. 

```
~$ sudo Python
```

그 후 다음과 같이 입력해줍니다.

![image-20210810153051857](C:\Users\PKNU\AppData\Roaming\Typora\typora-user-images\image-20210810153051857.png) 

```
>>> import pyfirmata
>>> board = pyfirmata.Arduino('/dev/ttyACM0')
```



여기까지 입력하였다면, 이제 핀모드를 결정하는 코드를 입력해줍니다. 

``` 
>>> Motor = board.get_pin('d:원하는 핀번호:o')
```

 여기서 

#### 'd'는 디지털를 의미하고 이곳에 'a'가 들어가면 아날로그를 의미합니다.

#### o/i/p는 순서대로 출력(OUTPUT), 입력(INPUT), PWM을 의미합니다.



이제 핀모드를 모두 결정하였다면, 다음과 같이 입력해서 제어할 수 있습니다.

```
>>> Motor.write(1); // Motor start
>>> Motor.write(0); // Motor stop
```

다음과 같이 1은 모터가 실행되고, 0는 모터가 정지할수 있게 제어할 수 있습니다.



** 물론 여러줄의 코드를 사용할 때 에는 python 파일을 만들어 사용하는것이 더 효과적이여서 실행 전 미리 .py 파일을 만들어서 사용하였습니다.

그리고 파일을 실행할 때에는 다음과 같이 명령어를 입력하여 실행시켰습니다. 

```
~$ sudo python3[파일이름]
```



