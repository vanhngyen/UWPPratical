using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPPratical
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        string path;
        SQLite.Net.SQLiteConnection connect;
        public class Contact
        {
            [PrimaryKey, AutoIncrement]
            public int Id { get; set; }
            public string Name { get; set; }
            [Unique]
            public string PhoneNumber { get; set; }

        }
        public MainPage()
        {
            this.InitializeComponent();
            path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "db.sqlite");
            connect = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            connect.CreateTable<Contact>();
        }


        private void Add_Contact(object sender, RoutedEventArgs e)
        {
            try
            {
                var querryadd = connect.Insert(new Contact()
                {
                    Name = Name.Text,
                    PhoneNumber = Phone.Text,
                });
            }
            catch (Exception ex)
            {

            }
        }
        private void Show_Contact(object sender, RoutedEventArgs e)
        {
            var querryshow = connect.Table<Contact>();
            string Name = "";
            string PhoneNumber = "";
            foreach (var q in querryshow)
            {
                Name = Name + " " + q.Name;
                PhoneNumber = PhoneNumber + " " + q.PhoneNumber;
            }
            Result.Text = "Name: " + Name + "\nPhoneNumber: " + PhoneNumber;
        }

        private void SearchContact_Click(object sender, RoutedEventArgs e)
        {
            string NameValue = Name.Text;
            string PhoneNumberValue = Phone.Text;
            var querrysearch = connect.Query<Contact>("select * from Contact where Name = ? and PhoneNumber = ?", NameValue, PhoneNumberValue);
            string NameResult = "";
            string PhoneResult = "";
            foreach (var q in querrysearch)
            {
                NameResult = NameResult + " " + q.Name;
                PhoneResult = PhoneResult + " " + q.PhoneNumber;
            }
            Result.Text = "Name: " + NameResult + "\nPhoneNumber: " + PhoneResult;
        }
        private void DeleteContact(object sender, RoutedEventArgs e)
        {
            connect.Execute("DELETE FROM Contact");
        }
    }
}
