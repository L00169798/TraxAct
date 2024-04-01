using TraxAct.ViewModels;

namespace TraxAct.Views;

public partial class ExercisePage : ContentPage
{
    public ExercisePage()
    {
        InitializeComponent();
        BindingContext = new ExerciseViewModel(Navigation);
        var navigationPage = new NavigationPage(new ExercisePage());
        Application.Current.MainPage = navigationPage;
    }
}