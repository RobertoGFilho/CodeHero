﻿namespace CodeHero
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(HeroDetailsPage), typeof(HeroDetailsPage));
        }
    }
}
