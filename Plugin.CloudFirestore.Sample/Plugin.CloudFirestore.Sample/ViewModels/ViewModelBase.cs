using Prism.AppModel;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.CloudFirestore.Sample.ViewModels
{
    public abstract class ViewModelBase : BindableBase, INavigationAware, IInitialize, IDestructible
    {
        public static readonly string ParameterKey = "parameter";

        protected INavigationService NavigationService { get; private set; }

        private string? _title;
        public string? Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        protected ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public virtual void Initialize(INavigationParameters parameters)
        {

        }

        public virtual void Destroy()
        {
        }
    }

    public abstract class ViewModelBase<TParameer> : ViewModelBase
    {
        protected ViewModelBase(INavigationService navigationService) : base(navigationService)
        {
        }

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            var parameter = (TParameer)parameters[ParameterKey];

            Prepare(parameter);
        }

        public abstract void Prepare(TParameer parameer);
    }
}
