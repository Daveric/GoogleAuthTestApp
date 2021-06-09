using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Google.Authenticator;

namespace GoogleAuthTestApp
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml  SuperSecretKeyGoesHere
  /// </summary> 
  public partial class MainWindow : Window
  {
    private BitmapImage _bi { get; set; }

    private double _screenDpiScale = 0;

    public MainWindow()
    {
      InitializeComponent();
      this.DpiChanged += On_DpiChanged;
    }

    /// <summary>
    /// Called when window is moved to a different DPI monitor
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void On_DpiChanged(object sender, DpiChangedEventArgs e)
    {      
      _screenDpiScale = e.NewDpi.DpiScaleX;
      this.Title = ($"New DPI: {_screenDpiScale}");
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
      var userEnter = AuthCodeTextBox.Text;

      TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();

      bool isCorrectPIN = tfa.ValidateTwoFactorPIN("1234556789", userEnter, TimeSpan.FromSeconds(30));
      if (isCorrectPIN == true)
      {
        WriteLine("<------------------------------->");
        WriteLine("Validating Authentication Code");
        WriteLine("Validation was a success...!");
        WriteLine("Validating process finished.");
        WriteLine("<------------------------------->");
        WriteLine(string.Empty);
        WriteLine(string.Empty);
      }
      else
      {

        WriteLine("   <------------------------------->");
        WriteLine("   Validating Authentication Code");
        WriteLine("   Validation was a failure...!");
        WriteLine("   Validating process finished.");
        WriteLine("   <------------------------------->");
        WriteLine(string.Empty);
        WriteLine(string.Empty);
      }
    }

    private void WriteLine(string msg)
    {
      var run = new Run(msg + Environment.NewLine)
      {
        Foreground = Brushes.Snow
      };
      ScrollViewer.ScrollToEnd();
      ResultsTextBlock.Inlines.Add(run);
    }

    private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
      TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();

      var setupInfo = tfa.GenerateSetupCode("Name of the app", "More info about the App", "1234556789", 256, 256); //the width and height of the Qr Code in pixels

      string qrCodeImageUrl = setupInfo.QrCodeSetupImageUrl; //  assigning the Qr code information + URL to string

      string manualEntrySetupCode = setupInfo.ManualEntryKey; // show the Manual Entry Key for the users that don't have app or phone


      _bi = new BitmapImage();
      _bi.BeginInit();
      _bi.UriSource = new Uri(qrCodeImageUrl);
      _bi.EndInit();

      Image1.Source = _bi;// showing the qr code on the page "linking the string to image element"

      Label1.Content = manualEntrySetupCode; // showing the manual Entry setup code for the users that can not use their phone
    }
  }
}
