using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthBack4AppDemo.ViewModels.Auth;
using Foundation;
using UIKit;
using Firebase.Auth;
using Firebase;
using Xamarin.Forms;
using AuthBack4AppDemo.iOS.Auth.Implementation;

[assembly: Dependency(typeof(FirebaseAuthenticator))]

namespace AuthBack4AppDemo.iOS.Auth.Implementation
{
    /// <summary>
    /// Author: Hlulani N. Maluleke
    /// </summary>
    class FirebaseAuthenticator : IFirebaseAuthenticator
    {

        /// <summary>
        ///  Login iOS user into firebase 
        /// </summary>
        /// <param name="email">user email</param>
        /// <param name="password">user password</param>
        /// <returns></returns>
        public async Task<string> LoginWithEmailPassword(string email, string password)
        {

            var authDataResult = await Firebase.Auth.Auth.DefaultInstance.SignInWithPasswordAsync(
                email,
                password);

            return await authDataResult.User.GetIdTokenAsync();
        }
        /// <summary>
        /// Signup iOS user 
        /// </summary>
        /// <param name="email">user email</param>
        /// <param name="password">user password</param>
        /// <returns></returns>
        public async Task<string> SignupWithEmailPassword(string email, string password)
        {
            var authDataResult = await Firebase.Auth.Auth.DefaultInstance.CreateUserAsync(
            email,
            password);

            return await authDataResult.User.GetIdTokenAsync();
        }
    }
    
}