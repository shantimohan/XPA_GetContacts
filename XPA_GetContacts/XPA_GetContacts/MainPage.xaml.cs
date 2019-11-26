using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
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
        bool CanReadContacts = false;

        public MainPage()
        {
            InitializeComponent();
            App.SetAppTitleAndToday(this.AppTitle, this.TodayIs);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (Device.RuntimePlatform == Device.Android)
            {
                if (await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Contacts) == PermissionStatus.Granted)
                {
                    CanReadContacts = true;
                }
                else
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Contacts);

                    if (results[Permission.Contacts] == PermissionStatus.Granted)
                        CanReadContacts = true;
                }
            }
            else
                CanReadContacts = true;

            if (CanReadContacts)
            {
                contacts = await Plugin.ContactService.CrossContactService.Current.GetContactListAsync();
                lvSearchedContacts.ItemsSource = contacts;
            }
        }

        private void sbSearchContacts_TextChanged(object sender, TextChangedEventArgs e)
        {
            lvSearchedContacts.ItemsSource = contacts.Where(x=>(x.Numbers.Count > 0 || x.Emails.Count > 0) && x.Name.ToLower().Contains(sbSearchContacts.Text.ToLower()));
        }

        private void lvSearchedContacts_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Plugin.ContactService.Shared.Contact contact = (Plugin.ContactService.Shared.Contact)lvSearchedContacts.SelectedItem;

            // Populate phone numbers list
            List<string> numbers = new List<string>();

            foreach(string strNumber in contact.Numbers)
            {
                string[] n = strNumber.Split('=');

                if (Device.RuntimePlatform == Device.iOS)
                    numbers.Add($"{(n[2].Contains("null") ? "" : n[2])}{n[1].Split(',')[0]}");
                else
                    numbers.Add($"{n[0]}");
            }

            lvNumbers.ItemsSource = numbers;

            // Populate E-Mails list
            lvEmails.ItemsSource = contact.Emails;
        }
    }
}
