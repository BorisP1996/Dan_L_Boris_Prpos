using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Zadatak_1.Command;
using Zadatak_1.Model;
using Zadatak_1.View;

namespace Zadatak_1.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {
        MainWindow main;
        Entity context = new Entity();

        public MainWindowViewModel(MainWindow mainOpen)
        {
            main = mainOpen;
            User = new tblUser();
        }
        private tblUser user;
        public tblUser User
        {
            get
            {
                return user;
            }
            set
            {
                user = value;
                OnPropertyChanged("User");
            }
        }

        private string username;
        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
                OnPropertyChanged("Username");
            }
        }

        private string password;
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }

        private ICommand close;
        public ICommand Close
        {
            get
            {
                if (close == null)
                {
                    close = new RelayCommand(param => CloseExecute(), param => CanCloseExecute());
                }
                return close;
            }
        }
        private void CloseExecute()
        {
            main.Close();
        }
        private bool CanCloseExecute()
        {
            return true;
        }

        private ICommand login;
        public ICommand Login
        {
            get
            {
                if (login == null)
                {
                    login = new RelayCommand(param => LoginExecute(), param => CanLoginExecute());
                }
                return login;
            }
        }
        private void LoginExecute()
        {


            try
            {

                tblUser newUser = new tblUser();
                newUser.Username = Username;
                newUser.Pasword = Password;
                if (UserExist(Username, Password) == true)
                {

                    MessageBox.Show("We recognize you! \nWelcome!");
                    MainSongView mainSong = new MainSongView();
                    mainSong.ShowDialog();
                }
                if (CheckUsername(Username)==true && PasswordValidation(Password)==true && UserExist(Username,Password)==false)
                {
                    context.tblUsers.Add(newUser);
                    context.SaveChanges();
                    MessageBox.Show("User is saved in database");
                    MainSongView mainSong = new MainSongView();
                    mainSong.ShowDialog();
                }
                if(CheckUsername(Username) == true || PasswordValidation(Password) == true)
                {
                    MessageBox.Show("Username must be unique\nPassword must contain 2 uppercase characters and can't be shorter than 6 characters");
                }
            
              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }
        }
        private bool UserExist(string user,string pas)
        {
            
            List<tblUser> useList = context.tblUsers.ToList();
            foreach (tblUser item in useList)
            {
                if (item.Username==user && item.Pasword==pas)
                {
                    return true;
                }
            }
            return false;
         
            
        }
        private bool CanLoginExecute()
        {
            if (String.IsNullOrEmpty(Username) || String.IsNullOrEmpty(Password) )
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool CheckUsername(string usernameInput)
        {
            List<tblUser> userList = context.tblUsers.ToList();

            List<string> usernameList = new List<string>();

            foreach (tblUser item in userList)
            {
                usernameList.Add(item.Username);
            }

            if (usernameList.Contains(usernameInput))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private bool PasswordValidation(string paswordInput)
        {
            char[] array = paswordInput.ToCharArray();

            int counter = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (Char.IsUpper(array[i]))
                {
                    counter++;
                }
            }

            if (counter>=2 && paswordInput.Length>=6)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
