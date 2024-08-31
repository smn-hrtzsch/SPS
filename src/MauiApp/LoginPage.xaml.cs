namespace MauiApplication;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private void OnLoginButtonClicked(object sender, EventArgs e)
    {
        string email = EmailEntry.Text;
        string password = PasswordEntry.Text;

        var member = App.PredictionGame?.Members.FirstOrDefault(m => m.GetEmailAddress() == email);

        if (member != null && member.GetPassword() == password)
        {
            MessageLabel.Text = "Login successful!";
            // Navigate to the main page or another page after successful login
            Navigation.PushAsync(new MainMenuPage());
        }
        else
        {
            MessageLabel.Text = "Invalid email or password. Please try again.";
        }
    }
}
