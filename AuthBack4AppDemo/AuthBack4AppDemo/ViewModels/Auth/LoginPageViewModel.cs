using AuthBack4AppDemo.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AuthBack4AppDemo.ViewModels.Auth
{
    /// <summary>
    /// Author: Hlulani N. Maluleke
    /// </summary>
    public class LoginPageViewModel : BaseViewModel
    {
        #region Fields
        private string _email;
        private string _password;
        #endregion

        #region Properties
 
        /// <summary>
        /// Gets or sets Email
        /// </summary>
        public string Email
        {
            get { return _email; }
            set 
            {
                if (_email == value)
                    return;

                _email = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// gets or set password
        /// </summary>
        public string Password
        {
            get { return _password; }
            set
            {
                if (_password == value)
                    return;

                _password = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or Sets Login button command
        /// </summary>
        public ICommand LoginCommand { get; set; }


        /// <summary>
        /// Gets or Sets Register button command
        /// </summary>
        public ICommand RegisterCommand { get; set; }
        #endregion

        /// <summary>
        /// Initializes Commands and page Services 
        /// </summary>
        #region Constructors
        public LoginPageViewModel()
        {
            PageService = new PageService();
            LoginCommand = new Command(OnLoginButtonClicked);
            RegisterCommand = new Command(OnRegisterButtonClicked);
        }

        #endregion

        #region Methods
        

        private void OnRegisterButtonClicked()
        {
            PageService.PushAsync(new RegistrationPage());
        }

        private async void OnLoginButtonClicked()
        {
            if (!(string.IsNullOrEmpty(Email) && string.IsNullOrEmpty(Password)))
            {
                var token = await DependencyService.Get<IFirebaseAuthenticator>().LoginWithEmailPassword(Email, Password);
                await PageService.DisplayAlert("Success", "User Created with token "+ token, "Ok");
            }
            else
            {
                await PageService.DisplayAlert("Error", "Please fill all fields", "Ok");
            }
        }

        #endregion
    }
}
