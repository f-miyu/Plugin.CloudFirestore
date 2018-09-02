using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Threading.Tasks;

namespace Plugin.CloudFirestore.Sample.Extensions
{
    public static class NavigationExtensions
    {
        public static Task NavigateAsync<TViewModel>(this INavigationService navigationService, NavigationParameters parameters = null, bool? useModalNavigation = null, bool animated = true, bool wrapInNavigationPage = false, bool noHistory = false) where TViewModel : BindableBase
        {
            var name = typeof(TViewModel).Name.Replace("ViewModel", "");

            if (wrapInNavigationPage)
            {
                name = "NavigationPage/" + name;
            }

            if (noHistory)
            {
                name = "/" + name;
            }

            return navigationService.NavigateAsync(name, parameters, useModalNavigation, animated);
        }
    }
}
