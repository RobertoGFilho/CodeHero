namespace CodeHero;

public partial class HeroDetailsPage : ContentPage
{
	public HeroDetailsPage()
	{
		InitializeComponent();
		BindingContext = new HeroDetailsViewModel();
    }
}