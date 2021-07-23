# ERP Program

<kbd>[![ERP](/Capture/ERP.gif "ERP")](https://github.com/kg4543/MiniProject_ERP/tree/main/ERPAPP/ERPAPP)</kbd> </br>

- ERP(Enterprise Resource Planning)는 전사적 자원 관리로서 회사의 자금,회계,구매,생산,출하 등 모든 업무 프로세스를 통합하여 비즈니스 프로세스를 좀 더 효율적으로 관리될 수 있도록 도와주는 프로그램이다.
- 이 프로그램에서는 회계부분을 제외한 오더, 생산, 출하, 자원관리(공장/직원/제품)를 구현하였다. 

# Login

<kbd>[![Login](/Capture/Login.PNG "Login")](https://github.com/kg4543/MiniProject_ERP/blob/main/ERPAPP/ERPAPP/View/LoginView.xaml.cs)</kbd> </br>
(Click the Image)

- Entity Framework를 활용하여 DB연동
- ID / Password 정보가 일치하는지 비교하여 로그인 허용
- 로그인 유저 정보를 Main 화면에서도 알 수 있도록 public 속성으로 정보를 받음
```C#
 string userId = TxtUserId.Text; //아이디 입력 값
string userPassword = TxtUserPassword.Password; //패스워드 입력 값

int userNum = DataAcess.Getusers() // 조건(아이디 & 패스워드 일치)에 맞는 아이디가 있는지 체크
            .Where(u => u.UserId.Equals(userId) && u.UserPassword.Equals(userPassword)).Count();

if (userNum == 0) //일치하는 아이디가 존재하지 않을 경우
{
    LblResult.Visibility = Visibility.Visible;
    LblResult.Content = "사용자가 존재하지 않습니다.!!!";
    Common.logger.Warn("아이디/패스워드 불일치");
    return;
}
else //로그인한 아이디 정보를 받아옴
{
    Common.LOGINED_USER = DataAcess.Getusers().Where(u => u.UserId.Equals(userId)).FirstOrDefault();
    LblResult.Visibility = Visibility.Hidden;
    Common.logger.Info($"{userId} 접속성공");
    Close();
}
```

# Main Menu

<kbd>[![Menu](/Capture/Menu.PNG "Menu")](https://github.com/kg4543/MiniProject_ERP/blob/main/ERPAPP/ERPAPP/MainWindow.xaml.cs)</kbd> </br>
(Click the Image)

- 로그인 정보의 권한에 따라 메뉴 활성화
```C#
if(Common.LOGINED_USER != null)
{
    BtnLogin.Content = "LogOut";
    BtnReport.IsEnabled = true;

    if (Common.LOGINED_USER.RItem == true)
        BtnItem.IsEnabled = true;
    if (Common.LOGINED_USER.ROrder == true)
        BtnOrder.IsEnabled = true;
    if (Common.LOGINED_USER.RProduction == true)
        BtnProduction.IsEnabled = true;
    if (Common.LOGINED_USER.RMaterial == true)
        BtnMRP.IsEnabled = true;
    if (Common.LOGINED_USER.RFactory == true)
        BtnFactory.IsEnabled = true;
    if (Common.LOGINED_USER.RMES == true)
        BtnMES.IsEnabled = true;
}
```
- 로그아웃 시 메뉴 전체 비활성화

# ITEM

<kbd>[![ITEM](/Capture/ITEM.PNG "ITEM")](https://github.com/kg4543/MiniProject_ERP/tree/main/ERPAPP/ERPAPP/View/ITEM)</kbd> </br>
(Click the Image)

- Entity Framework를 활용하여 DB연동
- Item 사진의 경로 위치를 DB에 저장하여 불러옴
```C#
if (selectedItem.ItemImage != null)
{
    BitmapImage bitmapImage = new BitmapImage(new Uri(selectedItem.ItemImage, UriKind.RelativeOrAbsolute));
    ImgItem.Source = bitmapImage;
}
else
{
    BitmapImage bitmapImage = new BitmapImage(new Uri("/Resources/No_Picture.jpg", UriKind.RelativeOrAbsolute));
    ImgItem.Source = bitmapImage;
}
```
- FileDialog를 활용하여 사진을 다운로드 및 업로드
```
private void BtnDownload_Click(object sender, RoutedEventArgs e)
{
    SaveFileDialog imageFile = new SaveFileDialog();
    imageFile.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif|Png Image|*.png";
    imageFile.FileName = Path.GetFileName(ImgItem.Source.ToString());
    imageFile.ShowDialog();
    BitmapImage bitmapImage = new BitmapImage(new Uri(ImgItem.Source.ToString(), UriKind.RelativeOrAbsolute));
    using (FileStream fs = new FileStream(imageFile.FileName, FileMode.Create
        ,FileAccess.ReadWrite, FileShare.None))
    {
        JpegBitmapEncoder encoder = new JpegBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(bitmapImage.UriSource));
        encoder.Save(fs);
    }
}

private async void BtnUpload_Click(object sender, RoutedEventArgs e)
{
    OpenFileDialog openfile = new OpenFileDialog();
    if (openfile.ShowDialog() == true) // file open
    {
        if (File.Exists(openfile.FileName)) //file이 있을 경우
        {
            //이미지 확장자 체크
            if (System.IO.Path.GetExtension(openfile.FileName) == ".jpg" |
                System.IO.Path.GetExtension(openfile.FileName) == ".png" |
                System.IO.Path.GetExtension(openfile.FileName) == ".gif")
            {
                //파일 소스를 이미지 소스로 바꿔줌
                BitmapImage bitmapImage = new BitmapImage(new Uri(openfile.FileName, UriKind.RelativeOrAbsolute));
                ImgItem.Source = bitmapImage;
                imgSrc = openfile.FileName;
            }
            else
            {
                await this.ShowMessageAsync("이미지 등록", "지원하는 확장자 파일이 아닙니다.");
            }
        }
    }
```
- 데이터무결성을 위홰 유효성 검사 후 데이터 추가 및 선택 수정

# Order

<kbd>[![Order](/Capture/Order2.PNG "Order")](https://github.com/kg4543/MiniProject_ERP/tree/main/ERPAPP/ERPAPP/View/Order)</kbd> </br>
(Click the Image)

- Entity Framework를 활용하여 DB연동
- DatePicker를 활용하여 날짜 선택
- Linq를 활용하여 DB 검색 (Trim()을 써서 뒤 공백을 없애줌)
```C#
private void BtnSearch_Click(object sender, RoutedEventArgs e)
{
    //검색 내용
    string searchCode = TxtSearchCode.Text;
    string searchDate = DtpShipment.Text;

    try
    {
        if (string.IsNullOrEmpty(searchDate))
        {
            DataContext = DataAcess.GetOrders().Where(i => i.OrderCode.Trim().Contains(searchCode)).ToList();
        }
        else
        {
            DataContext = DataAcess.GetOrders().Where(i => i.OrderCode.Trim().Contains(searchCode)
                                                & i.ShipDate.Equals(DateTime.Parse(searchDate))).ToList();
        }
    }
    catch (Exception ex)
    {
        Common.logger.Error($"검색 로드 Error : {ex}");
        throw ex;
    }
}
```
- 브랜드 선택 시 그 브랜드에 해당한 아이템들을 콤보박스에 불러옴
```C#
if (CmbBrand.SelectedItem != null)
{
    CmbItem.Items.Clear();
    CmbItem.SelectedItem = null;

    // 브랜드 선택시 해당 브랜드에 속한 아이템 리스트 로드
    var items = DataAcess.GetItems().Where(i => i.BrandCode.Equals(CmbBrand.SelectedItem.ToString())).ToList();
    foreach (var item in items)
        CmbItem.Items.Add(item.ItemCode);
}
```
- 아이템 선택 시 아이템 사진을 로드하여 보여줌 
- 사진이 없거나 아이템 선택이 안 되어 있는 경우 'No_image' 사진으로 대체
```C#
private void CmbItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
{
    if (CmbItem.SelectedItem != null)
    {
        selectedItem = DataAcess.GetItems().Where(i => i.ItemCode.Equals(CmbItem.SelectedItem.ToString())).FirstOrDefault();

        if (selectedItem.ItemImage != null)
        {
            BitmapImage bitmapImage = new BitmapImage(new Uri(selectedItem.ItemImage, UriKind.RelativeOrAbsolute));
            ImgItem.Source = bitmapImage;
        }
        else
        {
            BitmapImage bitmapImage = new BitmapImage(new Uri("/Resources/No_Picture.jpg", UriKind.RelativeOrAbsolute));
            ImgItem.Source = bitmapImage;
        }
    }
    else
    {
        BitmapImage bitmapImage = new BitmapImage(new Uri("/Resources/No_Picture.jpg", UriKind.RelativeOrAbsolute));
        ImgItem.Source = bitmapImage;
    }
}
```
- 데이터무결성을 위홰 유효성 검사 후 데이터 추가 및 선택 수정

# Factory

<kbd>[![Factory](/Capture/Factory.PNG "Factory")](https://github.com/kg4543/MiniProject_ERP/tree/main/ERPAPP/ERPAPP/View/Factory)</kbd> </br>
(Click the Image)

- 공장 선택 시 해당 공장에 소속된 직원 및 기계 수량 표기
```C#
 if (GrdData.SelectedItem != null)
{
    Common.SELECT_Factory = GrdData.SelectedItem as tblFactory;
    var selectedItem = Common.SELECT_Factory;
    var WorkerQty = DataAcess.GetWorker().Where(i => i.FactoryCode.Trim().Equals(selectedItem.FactoryCode.Trim().ToString())).Count();
    var MachineQty = DataAcess.GetMachine().Where(i => i.FactoryCode.Trim().Equals(selectedItem.FactoryCode.Trim().ToString())).Count();

    TxtCode.Text = selectedItem.FactoryCode.ToString();
    TxtName.Text = selectedItem.FactoryName.ToString();
    TxtWorkerQty.Text = $"{WorkerQty} 명";
    TxtMachineQty.Text = $"{MachineQty} 대";
}
```
