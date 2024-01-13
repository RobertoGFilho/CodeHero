using CommunityToolkit.Mvvm.ComponentModel;

namespace CodeHero
{
    public class HeroDetailsViewModel: ObservableObject
    {
        Character item;

        public Character Item
        {
            get => item;
            set => SetProperty(ref item, value);
        }
    }
}
