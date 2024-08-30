namespace MauiApplication;

public partial class MainMenuPage : ContentPage
{
    public MainMenuPage()
    {
        InitializeComponent();
    }

    private void OnDisplayMembersClicked(object sender, EventArgs e)
    {
        //Navigation.PushAsync(new DisplayMembersPage());
    }

    private void OnDisplayScoresClicked(object sender, EventArgs e)
    {
        //Navigation.PushAsync(new DisplayScoresPage());
    }

    private void OnManageSchedulesClicked(object sender, EventArgs e)
    {
        //Navigation.PushAsync(new ManageSchedulesPage());
    }

    private void OnLogoutClicked(object sender, EventArgs e)
    {
        Navigation.PopToRootAsync(); // Return to the Login page
    }
}
