using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace CodeHero
{
    public class HerosViewModel : ObservableObject
    {
        public HerosViewModel()
        {
            Items = new ObservableCollection<Character>();
            Pages = new ObservableCollection<int>();
            SelectCommand = new Command(OnSelect);
            SearchCommand = new Command(OnSearch);
            NextCommand = new Command(OnNext);
            PreviousCommand = new Command(OnPrevious);
        }

        private Character selectedItem;
        private string search;
        private int selectedPage;
        private int totalPages;
        private int totalItems;
        private bool isBusy;

        public ObservableCollection<Character> Items { get; set; }
        public ObservableCollection<int> Pages { get; set; }
        
        public Character SelectedItem
        {
            get => selectedItem;
            set => SetProperty(ref selectedItem, value);
        }
        
        public int SelectedPage
        {
            get => selectedPage;
            set => SetProperty(ref selectedPage, value);
        }

        public string Search
        {
            get => search;
            set => SetProperty(ref search, value);
        }

        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }

        public ICommand SelectCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand NextCommand { get; }
        public ICommand PreviousCommand { get; }

        public async Task OnAppearing()
        {
            if (Items.Count == 0)
            {
                await GetCharacteres();
                SetPageIndicators();
            }
        }

        private void SetPageIndicators()
        {
            var div = (double)totalItems / (double)4;
            totalPages = (int)Math.Ceiling(div);
            Pages.Clear();

            for (int i = 0; i < totalPages; i++)
                Pages.Add(i + 1);

            SelectedPage = 1;
        }

        private async void OnSearch()
        {
            await GetCharacteres();
            SetPageIndicators();
        } 
        
        private async Task GetCharacteres(int offset = 0)
        {
            try
            {
                if ( IsBusy)
                    return;

                IsBusy = true;
                await Task.Delay(100);

                var herosService = new MarvelApiClient();
                var result = await herosService.GetCharactersAsync(offset, 4, Search);

                if (result?.data?.results == null)
                    return;

                totalItems = result.data.total;
                Items.Clear();

                foreach (var character in result?.data?.results)
                    Items.Add(character);

                IsBusy = false;
            }
            catch (Exception)
            {
                IsBusy = false;
                throw;
            }
        } 

        private async void OnPrevious()
        {
            if (SelectedPage > 1)
            {
                SelectedPage--;
                await GetCharacteres((SelectedPage -1) * 4);
            }
        }

        private async void OnNext()
        {
            if (SelectedPage < totalPages)
            {
                await GetCharacteres(SelectedPage * 4);
                SelectedPage++;
            }
        }

        private async void OnSelect()
        {
            if (SelectedItem == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(HeroDetailsPage)}");
            var viewModel = Shell.Current.CurrentPage.BindingContext as HeroDetailsViewModel;
            viewModel.Item = SelectedItem;

            SelectedItem = null;
        }
    }

}
