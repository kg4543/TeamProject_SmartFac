# AGV Control APP

<kbd>[![AGVControl](/Capture/AGVCotroler.jpg "AGVControl")](https://github.com/kg4543/TeamProject_SmartFac/tree/main/AGV/Control/AGVControl)</kbd> </br>

## 1. UI

1. GridView를 활용한 (전/후/좌/우) Button
```C#
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="AGVControl.MainPage">

    <StackLayout>
        <Frame BackgroundColor="#2196F3" Padding="24" CornerRadius="0">
            <Label Text="Start!" HorizontalTextAlignment="Center" TextColor="White" FontSize="36"
                   x:Name="LblStart"/>
        </Frame>
        <Entry x:Name="TxtServer"/>
        <Grid>
            <Grid.RowDefinitions>
                <RowDefinition/>
                <RowDefinition/>
                <RowDefinition/>
            </Grid.RowDefinitions>
            <Grid.ColumnDefinitions>
                <ColumnDefinition/>
                <ColumnDefinition/>
                <ColumnDefinition/>
            </Grid.ColumnDefinitions>
            <Button x:Name="BtnGo" Text="▲" 
                    Grid.Row="0" Grid.Column="1"
                    FontSize="20" Margin="5"
                    Background="skyblue" TextColor="White"
                    Clicked="BtnGo_Clicked"/>
            <Button x:Name="BtnStop" Text="■" 
                    Grid.Row="1" Grid.Column="1"
                    FontSize="20" Margin="5"
                    Background="skyblue" TextColor="White"
                    Clicked="BtnStop_Clicked"/>
            <Button x:Name="BtnLefg" Text="◀" 
                    Grid.Row="1" Grid.Column="0"
                    FontSize="20" Margin="5"
                    Background="skyblue" TextColor="White"
                    Clicked="BtnLefg_Clicked"/>
            <Button x:Name="BtnRight" Text="▶" 
                    Grid.Row="1" Grid.Column="2"
                    FontSize="20" Margin="5"
                    Background="skyblue" TextColor="White"
                    Clicked="BtnRight_Clicked"/>
            <Button x:Name="BtnBack" Text="▼" 
                    Grid.Row="2" Grid.Column="1"
                    FontSize="20" Margin="5"
                    Background="skyblue" TextColor="White"
                    Clicked="BtnBack_Clicked"/>
        </Grid>
    </StackLayout>

</ContentPage>
```

## 2. Logic

1. MQTT 데이터 전달 
```C#
private static async Task SendData(string server, string command)
        {
            var mqttClient = new MqttFactory().CreateMqttClient();

            var options = new MqttClientOptionsBuilder().WithTcpServer(server, 1883).Build();

            var message = new MqttApplicationMessageBuilder().WithTopic("MOTOR/TEST/").WithPayload(command).WithExactlyOnceQoS().Build();

            var result = await mqttClient.ConnectAsync(options, CancellationToken.None);

            if (result.ResultCode == MqttClientConnectResultCode.Success)
            {
                await mqttClient.PublishAsync(message);
            }
        }
```
