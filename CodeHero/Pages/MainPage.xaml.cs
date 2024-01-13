namespace CodeHero
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new HerosViewModel();
        }

        private HerosViewModel viewModel;

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.OnAppearing();
        }

    }
}
