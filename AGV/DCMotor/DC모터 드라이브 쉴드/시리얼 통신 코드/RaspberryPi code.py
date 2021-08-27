from serial import Serial #시리얼 통신
import time

#아두이노 USB연결 
ser = Serial('/dev/ttyACM0', 9600)

while True:
    val = input() #입력값을 val에 저장

    if val == '1': #입력 값이 1일 때
        val = val.encode('utf-8')
        ser.write(val) #val값을 아두이노에게 전송!
        print("Let's Go") #전송 후 출력
        time.sleep(0.01)
        
    elif val == '2':
        val = val.encode('utf-8')
        ser.write(val)
        print("Stop")
        time.sleep(0.01)
        
    elif val == '3':
        val = val.encode('utf-8')
        ser.write(val)
        print("Turn Right")
        time.sleep(0.01)
        
    elif val == '4':
        val = val.encode('utf-8')
        ser.write(val)
        print("Turn Left")
        time.sleep(0.01)
        
    elif val == '5':
        val = val.encode('utf-8')
        ser.write(val)
        print("BackWard")
        time.sleep(0.01)
        

