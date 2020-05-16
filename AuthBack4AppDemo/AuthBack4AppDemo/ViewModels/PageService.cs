using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AuthBack4AppDemo.ViewModels
{    
    /// <summary>
     /// Author: Hlulani N. Maluleke
     /// </summary>
    public class PageService : IPageService
    {
        public Page page { get; set; } = Application.Current.MainPage;

        public PageService()
        {
            if (page == null)
                page = Application.Current.MainPage;
        }
        public async Task PushAsync(Page page)
        {
            await page.Navigation.PushAsync(page);
        }
        public async Task PopAsync()
        {
            await page.Navigation.PopAsync();
        }
        public async Task DisplayAlert(string title, string message, string cancel)
        {
            await page.DisplayAlert(title, message, cancel);
        }
        public async Task DisplayAlert(string title, string message, string confirm, string cancel)
        {
            await page.DisplayAlert(title, message, confirm, cancel);
        }
    }
}
