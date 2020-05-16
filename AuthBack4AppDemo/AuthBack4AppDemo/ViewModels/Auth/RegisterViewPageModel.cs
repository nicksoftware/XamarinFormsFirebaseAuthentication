using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace AuthBack4AppDemo.ViewModels.Auth
{
    /// <summary>
    /// author: Hlulani N. Maluleke
    /// </summary>
    public class RegisterViewPageModel : BaseViewModel
    {
        #region Fields
        private string _email;

        private string _username;

        private string _password;

        private string _confirmPassword;

        #endregion
        #region Properties


        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                if (_confirmPassword == value)
                    return;
                _confirmPassword = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value; OnPropertyChanged();
            }
        }

        public string Username
        {
            get { return _username; }
            set { _username = value; OnPropertyChanged(); }
        }

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


        public ICommand RegisterCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        #endregion

        #region Constructors 
        public RegisterViewPageModel()
        {
            PageService = new PageService();
            RegisterCommand = new Command(SignUpButton_Clicked);
            LoginCommand = new Command(LoginButton_Clicked);
        }

        private void LoginButton_Clicked(object obj)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Methods
        public async void SignUpButton_Clicked()
        {

            if (!(string.IsNullOrEmpty(Email) && string.IsNullOrEmpty(Username) && string.IsNullOrEmpty(Password)))
            {
                var token = await DependencyService.Get<IFirebaseAuthenticator>().SignupWithEmailPassword(Email, Password);
                await PageService.DisplayAlert("Success", "User Created with token "+token, "Ok");
            }
            else
            {
                await PageService.DisplayAlert("Error", "Please fill all fields", "Ok");
            }

        }
        #endregion
    }
}
