using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XPA_GetContacts
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        IList<Plugin.ContactService.Shared.Contact> contacts;

        public MainPage()
        {
            InitializeComponent();
            App.SetAppTitleAndToday(this.AppTitle, this.TodayIs);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            contacts = await Plugin.ContactService.CrossContactService.Current.GetContactListAsync();
            lvSearchedContacts.ItemsSource = contacts;
        }

        private void sbSearchContacts_TextChanged(object sender, TextChangedEventArgs e)
        {
            lvSearchedContacts.ItemsSource = contacts.Where(x=>(x.Numbers.Count > 0 || x.Emails.Count > 0) && x.Name.Contains(sbSearchContacts.Text));
        }

        private void lvSearchedContacts_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Plugin.ContactService.Shared.Contact contact = (Plugin.ContactService.Shared.Contact)lvSearchedContacts.SelectedItem;

            // Populate phone numbers list
            List<string> numbers = new List<string>();

            foreach(string strNumber in contact.Numbers)
            {
                string[] n = strNumber.Split('=');

                numbers.Add($"{(n[2].Contains("null") ? "" : n[2])}{n[1].Split(',')[0]}");
            }

            lvNumbers.ItemsSource = numbers;

            // Populate E-Mails list
            lvEmails.ItemsSource = contact.Emails;
        }
    }
}
