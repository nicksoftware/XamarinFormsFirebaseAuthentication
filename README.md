## Xamarin Forms Firebase Authentication

Authenticating with firebase with xamarin can be a bit of a challenge Especially when every article out there only show one way of doing it, so lets simplify the process.
## Getting Started 

Before we get started there  are few things we should keep in mind ,

* firstly that is our development our dev environment, we should have all the required xamarin libraries and sdks to create mobile applications
* Secondly If the packages get updated  and have breaking  changes then you will have consult the firebase docs.
* Last but not least if you need a simple demonstration of  this you can download the source code on my repo here is there  project [XamarinFormsFirebaseAuthentication](https://github.com/hnicolus/)

Lets get started : 
  so in order to communicate the firebase server "severless backend services"  endpoint api's we need a few sdks.
  
  lets start with the Android application lets install the following packages from nuget:
   - Xamarin.Firebase.Auth
   - Xamarin.Firebase.Common
   - Xamarin.Firebase.Core
   - Xamarin.Firebase.Firestore
   - Xamarin.Google.Guava
   
   Next lets do same and installed some required packages on our iOS.

   - Xamarin.Firebase.iOS.Auth
   - Xamarin.Firebase.iOS.CloudFirestore
   


Now we done downloading the libraries we will need to  implement firebase authentication.We working with two different  platforms. We need to create  an interface that will   have the declarations of  the  related functionalities that our Authenticator  classes on each platform will implement.

Interfaces are one of my favourite features in c# because they allow  us to create highly decoupled systems.An interface is basically a contract , it doesn’t have any implementation it can only contain  method declarations.Like a real world contract once you sign it you need to abide by its rules, similarly 
the classes that implement the interface **MUST** implement the  methods **declared** in an interface.
Well for us this means that the signature of the methods in the classes that implement our interface will be the same so when when we call a method declared in the interface with the DependencyService, every class that has implemented the interface and is registerd as a dependency in the container will invoke that method.

The great part is the actual implementation (logic inside the methods ) will  depend on the platform or the logic placed inside the methods.

With this in mind lets create our interface contract:

`  
using System;
using System.Text;
using System.Threading.Tasks;

namespace APPNAME.Interfaces.FirebaseAuthentication
{

    public interface IFirebaseAuthenticator
    {
        Task<string> SignInWithEmailAndPasswordAsync(string email, string password);
        Task<string> SignUpWithEmailPasswordAsync(string email, string password);
        Task ForgotPasswordAsync(string email);
    }

}
`
In this scenario we want our classes  to have to implement the SignIn ,Signup ,and forgotPassword methods with the signatures described above, 

Next, now we have to do the actual implementation on each platform.

Let's start with the android  application

depending on where you like to place the code in this case we created a folder for Authentication/Firebase, create a class that implements our IFirebaseAuthenticator and register this concrate class as a dependency


## Before we go ahead I need to clarify this  for the next part , there are two ways of doing this 99% of the implementation  will be same ,the only difference will be on how we setup everything to work. 

Most Articles show the first approach which is simpler and quicker but it has a flaw which I will talk about later the second approach it requires a little more code , If you find yourself in scenario where the first option isnt working for you you could try the second approach.


Before we do the implementations we need to go to to firebase 

 1. Sign in
 2. create or add a new project for the app
 3. choose to add firebase to android app
 4. Add you android package name where its required
 5. (Important!) Download the google-services.json file
 
 ### The First way of Implementing Firebase on android is done as follows :
 
 So the json file we downloaded from Firebase contains all the settings we need to communicate with the server, it contains the apikey, the project id  etc,
 so we copy  it into the android project and make sure its included int project file. you can read more about on the firebase guide . [FirebaseAuth](https://developers.google.com/android/reference/com/google/firebase/auth/FirebaseAuth) is The entry point of the Firebase Authentication SDK.
 
 Once we have this  instance we will be able to  call all the appropriate methods we need to authenticate such as SignInWithEmailAndPasswordAsync
 
 so to get started we need to do the following :
 
  1. copy th google-services.json file into your project
  
 And then we Create an Implementation class which implements the interface we created in the shared project, REMEMER to Register this class in the Dependency 
 Container this is archieved by decorating the class with the  `[assembly: Dependency(typeof(FirebaseAuthenticator)))]` decorator. and then we provide the class implementation of the interface we created above  The final code will look like this : 

```
[assembly: Dependency(typeof(AuthDroid))]

namespace NAMEOFAPP.Authentication.Droid
{
    public class FirebaseAuthenticator : IFirebaseAuthenticator
    {
        public async Task<string> SignInWithEmailAndPasswordAsync(string email, string password)
        {
            try 
            {
              //now we use the staic method in  FirebaseAuth to get an instance of FirebaseAuth
              
                var user = await FirebaseAuth.Instance?.SignInWithEmailAndPasswordAsync(email, password);
                var token = await user.User.GetIdTokenAsync(false);
                return token.Token;
            } 
            catch(FirebaseAuthInvalidUserException e)
            {
               //.... do somtheing the exception it contains useful information of how the request was handled 
                return "";
            }
        }
        
       public async Task<string> SignUpWithEmailPasswordAsync(string email, string password)
        {

            var user = await FirebaseAuth.Instance?.CreateUserWithEmailAndPasswordAsync(email, password);
            var token = await user.User.GetIdTokenAsync(false);
            return token.Token;
        }
        
        public async Task ConfirmPasswordResetAsync(string code,string newPassword)
        {
            await FirebaseAuth.Instance?.ConfirmPasswordResetAsync(code, newPassword);
        }
        
        public async Task ForgotPasswordAsync(string email)
        {
            await FirebaseAuth.Instance?.SendPasswordResetEmailAsync(email);

        }

        public async Task VerifyPasswordResetCodeAsync(string code)
        {
            await FirebaseAuth.Instance?.VerifyPasswordResetCodeAsync(code);
        }
    }

```
## Second Approach 

Thats it we are done with the android implementation  usually this works but if not ☹ dont cry , 
try this second approach.

In this approach wil will need to do what was automated above manually so  instead of hoping that the FirebaseAuth will hopefully find the json file and create and initialise FirebaseAuth using the applications current context.
We actually write the code to do it.This  is archieved the following way  first we will need to navigate to the MainActivity.cs file

1. We create a static public Field of type FirebaseAuth call it Auth  and
the code will look like this Create two static variables.
```
 public static FirebaseAuth Auth;
```

2. Secondly lets create a method will will be invoked in the constructor of our application when the application starts , this method will contain the  logic to initial and create a the FirebaseAuth instance
we will call this method InitFirebaseAuth since it does just that. inside this method.
we have to build an options object which will have all the settings we need to communicate with the firebase server,we will get these settings from the json file we downloaded from firebase

we will build the complex options object with the builder in the FirebaseOptions class once we have configured the builder we build our options object an store it in a variable
called options .

The next step is to initialize the FirebaseApp and get the instance of app we pass in the current instance of MainActivity and the options we created
 .Then Finally to  FirebaseAuth we use the FirebaseAuth.GetInstance(app) static method and pass in app  as an argument for the method,  
 the code will look like this 

```
  private void InitFirebaseAuth()
  {

      var options = new FirebaseOptions.Builder()
          .SetProjectId("YOUR_PROJECT_ID")
          .SetApplicationId("YOUR_APPLICATION_ID")
          .SetApiKey("YOUR_FIREBASE_API_KEY")
          .SetDatabaseUrl("YOUR_DATABASEURL")
          .SetStorageBucket("YOUR_STORAGEBUCKET")
          .Build();
          
     var  _fireApp = FirebaseApp.InitializeApp(this, options);
    
      Auth = FirebaseAuth.GetInstance(_fireApp);
  }
```

at this point we done with the code to all we have to do is call the the InitFirebaseAuth() method in the constructor. We will do this above the LoadApplication method so the code will actually look like this

```
  Xamarin.Essentials.Platform.Init(this, savedInstanceState);
  global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
  ...
  InitFirebaseAuth(); //Create an instance of FirebaseAuth
  LoadApplication(new App());
```

Finally we can now create a class that  implements the IFirebaseAuthenticator , like how we did on the first approach, we have to  register this class as dependency using the decorator.
The only difference between this class  the previous one on the first approach  is in this one we get  the instance of Firebase from the static Auth Field we created in the MainActivity class.
the finally code will look something like this .. 
```
using APPNAME.Droid.Implementations.Auth;
using APPNAME.ViewModels.Auth;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(FirebaseAuthenticator))]
namespace APPNAME.Droid.Authentication.Firebase
{
    public class FirebaseAuthenticator : IFirebaseAuthenticator
    {
        public async Task<string> LoginWithEmailPasswordAsync(string email, string password)
        {

            var user = await MainActivity.Auth.SignInWithEmailAndPasswordAsync(email, password);
            var token = await user.User.GetIdTokenAsync(false);

            return token.Token;
        }

        public async Task<string> SignUpWithEmailPasswordAsync(string email, string password)
        {

            var user = await MainActivity.Auth.CreateUserWithEmailAndPasswordAsync(email, password);
            var token = await user.User.GetIdTokenAsync(false);
            return token.Token;
        }
        
        public async Task ConfirmPasswordResetAsync(string code,string newPassword)
        {
            await MainActivity.Auth.ConfirmPasswordResetAsync(code, newPassword);
        }
        
        public async Task ForgotPasswordAsync(string email)
        {
            await MainActivity.Auth.SendPasswordResetEmailAsync(email);

        }

        public async Task VerifyPasswordResetCodeAsync(string code)
        {
            await MainActivity.Auth.VerifyPasswordResetCodeAsync(code);
        }

    }
}
```
You can go  on as adding validatiors  to check if Auth isnt null on both approaches  to avoid null exceptions  at run time. 
and  MAKE sure you import the namespaces. for the required classes
Create two static variables



### Next we move onto the iOS application 


 1. Sign in your Firebase
 3. choose to add firebase to ios app to the project you created
 4. do the configurations
 5. (Important!) Download the GoogleService-Info.plist file
 6. Copy file into the project 
 7. Change the files Build Action to Bundle Resource 

For the iOS application its straight forward what you  basically need to do is in your app Delegate file 
you need to add the following line of code before you return base.FinisedLaunching(app,options);  
```
 Firebase.Core.App.Configure();
```

and then go ahead create a new file implement the IAuthenticator Interface we created in the shared project

The final code will look like this 

```
[assembly: Dependency(typeof(FirebaseAuthenticator))]

namespace OFIForexSignalApp.iOS.Implementations.Auth
{

    class FirebaseAuthenticator : IFirebaseAuthenticator
    {

        public async Task<string> LoginWithEmailPassword(string email, string password)
        {

            var authDataResult = await Firebase.Auth.Auth.DefaultInstance.SignInWithPasswordAsync(
                email,
                password);

            return await authDataResult.User.GetIdTokenAsync();
        }

        public async Task<string> SignupWithEmailPassword(string email, string password)
        {
            var authDataResult = await Firebase.Auth.Auth.DefaultInstance.CreateUserAsync(
            email,
            password);

            return await authDataResult.User.GetIdTokenAsync();
        }
    }
}
```


at this point we are done with implementation on both platforms now what we need to this is use our shared class library to right the to perform authentication ..e.g on your LoginViewModel or LoginPage Code behind you can just add the logic login  the example below

```
 private async Task LoginClicked()
        {
            if (!(string.IsNullOrEmpty(Email) && string.IsNullOrEmpty(Password)))
            {
                try
                {
                    var token = await DependencyService.Get<IFirebaseAuthenticator>().LoginWithEmailPasswordAsync(Email, Password);

                    if (token == null)
                    {
                        await DIsplayError();
                    }
                    else
                    {
                        App.UserToken = token;
                        CrossToastPopUp.Current.ShowToastSuccess("Login Successful");
                        await Shell.Current.GoToAsync(SIGNALS_PAGE);
                    }
                }
                catch (System.Exception ex)
                {
                    CrossToastPopUp.Current.ShowToastError(ex.Message);
                    Debug.WriteLine(ex.Message);
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Error", "Please fill all fields", "Ok");
            }
        }
				....
```

What basically happens is we use the dependencyService to get any class that is registerd as a in the dependacy container that implements IFirebasAuthenticator and it invokes the login method of  this classes and passes in the email and password we retrieve from the UI
