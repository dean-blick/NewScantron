## New Scantron Program

 Remake of old scantron program
 Written by Dean Blickenstaff

# Summary:

An in depth explanation of how a lot of the roots of the program work can be found here: [Old Wiki Page](https://github.com/jwebster7/scantron-dev/wiki)

The previous program has been rewritten to be much more simple to work on and use. The previous program was working around the limitations of Windows Forms.

Having taken the class CIS 400 will help you immensely while working on this program.

You will need to download a driver in order for the computer to recognize the Scantron machine. [Drivers](https://www.scantron.com/product-updates/)

It is **extremely** recommended that you use Visual Studio (the purple one) to work on this program.

# To Do:

1. Bug testing
2. Improve UI?


# Important notes:

Assembly name and default namespace updated in properties

System.IO.Ports Nuget package -> Right click on project -> manage nuget packages -> browse and search for System.IO.Ports and install

User interface will be managed through the MainWindow.xaml.cs and UIViewModel. The datacontext of the main window is set to be an instance of the UIViewModel which will store all of the necessary info for the user interface, with MainWindow.xaml.cs handling button clicks and inputs etc.




### Extra code blocks:

For using an image:
```xml
<StackPanel>
<Image UseLayoutRounding="True" RenderOptions.BitmapScalingMode="HighQuality">
    <Image.Source>
        <BitmapImage UriSource=".\Media\NewScantronImage.jpeg" Rotation="Rotate270"/>
    </Image.Source>
</Image>
</StackPanel>

```

Old start page instructions
```xml
<TextBlock FontSize="16" Text="Step 1:" FontWeight="Bold" Margin="5"/>
<TextBlock FontSize="14" TextWrapping="Wrap" Text="Set your Scantron cards in the tray by following the pictures to the right." Margin="5"/>
<TextBlock FontSize="16" Text="Step 2:" FontWeight="Bold" Margin="5"/>
<TextBlock FontSize="14" Text="Click Reset." Margin="5"/>
<TextBlock FontSize="16" Text="Step 3:" FontWeight="Bold" Margin="5"/>
<TextBlock FontSize="14" TextWrapping="Wrap" Text="Enter the exam name, number of versions, and number of questions." Margin="5"/>
<TextBlock FontSize="16" Text="Step 4:" FontWeight="Bold" Margin="5"/>
<TextBlock FontSize="14" TextWrapping="Wrap" Text="If you would like a .csv gradebook file, check &quot;Grade with this program?&quot;." Margin="5"/>
```
Grading prompt and checkbox for start tab
```xml
<StackPanel Orientation="Horizontal" Margin="5" VerticalAlignment="Center">
    <TextBlock FontSize="14" Text="Grade with this program?" FontWeight="Bold" VerticalAlignment="Center" Margin="5"/>
    <CheckBox IsChecked="{Binding Path=IsGrading, Mode=TwoWay}" x:Name="IsGrading" VerticalAlignment="Center" Margin="5" Checked="IsGrading_Checked"/>
</StackPanel>
```
Scanning instructions
```xml
<StackPanel>
<TextBlock FontWeight="Bold" FontSize="16" Margin="0,10,0,0" Text="Step 1:"/>
<TextBlock FontSize="14" TextWrapping="Wrap" Text="Click Ready, then press the Start button on the Scantron machine."/>
<TextBlock FontWeight="Bold" FontSize="16" Margin="0,10,0,0" Text="Step 2:"/>
<TextBlock FontSize="14" TextWrapping="Wrap" Text="Once the machine is done scanning, click Done."/>
<TextBlock FontWeight="Bold" FontSize="16" Margin="0,10,0,0" Text="Step 3:"/>
<TextBlock FontSize="14" TextWrapping="Wrap" Text="You can edit WIDs, test versions, and sheet numbers."/>
<TextBlock FontWeight="Bold" FontSize="16" Margin="0,10,0,0" Text="Step 4:"/>
<TextBlock FontSize="14" TextWrapping="Wrap" Text="Click Save Changes. then click Continue."/>
</StackPanel>
```
