using Prism;
using Prism.Ioc;
using Plugin.CloudFirestore.Sample.ViewModels;
using Plugin.CloudFirestore.Sample.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Prism.DryIoc;
using Plugin.CloudFirestore.Sample.Extensions;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Plugin.CloudFirestore.Sample
{
    public partial class App : PrismApplication
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync<MainPageViewModel>(wrapInNavigationPage: true);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage>();
            containerRegistry.RegisterForNavigation<NewTodoItemPage>();
            containerRegistry.RegisterForNavigation<TodoItemDetailPage>();
        }
    }
}
