using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

#if OFFLINE_SYNC_ENABLED
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;  // offline sync
using Microsoft.WindowsAzure.MobileServices.Sync;         // offline sync
#endif

namespace BambuShootProject.WPF
{
 
    /// <summary>
    /// Interaction logic for AddUser.xaml
    /// </summary>
    public partial class AddUser : Window
    {
       
        private MobileServiceCollection<Users, Users> UserTableItems;
#if OFFLINE_SYNC_ENABLED
           private IMobileServiceSyncTable<Reports> ReportsTable = App.MobileService.GetSyncTable<Reports>(); // offline sync
           private IMobileServiceSyncTable<Users> UserTable = App.MobileService.GetSyncTable<Users>(); // offline sync
#else

       
        private IMobileServiceTable<Users> UserTable = App.MobileService.GetTable<Users>();
#endif

        //Window constructor that initializes all components
        public AddUser()
        {
            InitializeComponent();
        }

        // Inserts an User to the Database
        private async Task InsertUserItem(Users UsersItem)
        {
            // This code inserts a new Item into the database. After the operation completes
            // and the mobile app backend has assigned an id, the item is added to the CollectionView.

            await UserTable.InsertAsync(UsersItem);
            UserTableItems.Add(UsersItem);

#if OFFLINE_SYNC_ENABLED
            await App.MobileService.SyncContext.PushAsync(); // offline sync
#endif
        }

        //Assign the textboxes parameters to the database item before inserting it
        private async void button_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text == null || String.IsNullOrEmpty(passwordBox.Password))
            {
                MessageBox.Show(" Please fill the parameters");
            }
            else
            {
                await RefreshUserItems();
                var UserList = await UserTable.ToListAsync();
                int count = UserList.Count + 1;
                Users UserItem = new Users();

                UserItem.id = count.ToString();
                UserItem.username = textBox.Text;
                UserItem.password = passwordBox.Password;
                await InsertUserItem(UserItem);
                this.Hide();
                passwordBox.Clear();
            }
        }

        //Removes an item from the local table
        private async Task UpdateUserItem(Users UserItems)
        {
            // This code takes a freshly completed TodoItem and updates the database.
            // After the MobileService client responds, the item is removed from the list.
            await UserTable.UpdateAsync(UserItems);
            UserTableItems.Remove(UserItems);
          

#if OFFLINE_SYNC_ENABLED
            await App.MobileService.SyncContext.PushAsync(); // offline sync
#endif
        }

        //Refresh local table with database so it can be used befre inserting a new item to avoid collision 
        private async Task RefreshUserItems()
        {
            MobileServiceInvalidOperationException exception = null;
            try
            {
                // This code refreshes the entries in the list view by querying the TodoItems table.
                // The query excludes completed TodoItems.
                UserTableItems = await UserTable.ToCollectionAsync();
            }
            catch (MobileServiceInvalidOperationException e)
            {
                exception = e;
            }

            if (exception != null)
            {
               
            }
            else
            {
               
                this.button.IsEnabled = true;
            }
        }
    }
}
