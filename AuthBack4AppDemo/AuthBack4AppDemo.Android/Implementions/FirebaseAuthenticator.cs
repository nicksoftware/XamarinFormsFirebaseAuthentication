using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AuthBack4AppDemo.Droid.Implementions;
using AuthBack4AppDemo.ViewModels.Auth;
using Firebase.Auth;
using Org.Apache.Http.Authentication;
using Xamarin.Forms;
using AuthBack4AppDemo.Droid;
[assembly: Dependency(typeof(FirebaseAuthenticator))]
namespace AuthBack4AppDemo.Droid.Implementions
{
    public class FirebaseAuthenticator : IFirebaseAuthenticator
    {
        public async Task<string> LoginWithEmailPassword(string email, string password)
        {
            var user = await MainActivity.Auth.SignInWithEmailAndPasswordAsync(email, password);
            var token = await user.User.GetIdTokenAsync(false);

            return token.Token;
        }

        public async Task<string> SignupWithEmailPassword(string email, string password)
        {
            var user = await MainActivity.Auth.CreateUserWithEmailAndPasswordAsync(email, password);
            var token = await user.User.GetIdTokenAsync(false);
            return token.Token;
        }
    }
}