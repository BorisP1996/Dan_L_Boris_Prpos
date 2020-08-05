using System;
using System.Windows;
using System.Windows.Input;
using Zadatak_1.Command;
using Zadatak_1.Model;
using Zadatak_1.View;

namespace Zadatak_1.ViewModel
{
    class CreateSongViewModel : ViewModelBase
    {
        CreateSong createSong;
        Entity context = new Entity();

        public CreateSongViewModel(CreateSong createSongOpen)
        {
            createSong = createSongOpen;
            Song = new tblSong();
        }

        private tblSong song;
        public tblSong Song
        {
            get
            {
                return song;
            }
            set
            {
                song = value;
                OnPropertyChanged("Song");
            }
        }
        private bool update;
        public bool Update
        {
            get
            {
                return update;
            }
            set
            {
                update = value;
                OnPropertyChanged("Update");
            }
        }
        private ICommand add;
        public ICommand Add
        {
            get
            {
                if (add == null)
                {
                    add = new RelayCommand(param => AddExecute(), param => CanAddExecute());
                }
                return add;
            }
        }
        /// <summary>
        /// Saving new song
        /// </summary>
        private void AddExecute()
        {
            try
            {
                tblSong newSong = new tblSong();
                newSong.Title = Song.Title;
                newSong.Author = Song.Author;
                newSong.Duration_s = Song.Duration_s;
                context.tblSongs.Add(newSong);
                context.SaveChanges();
                MessageBox.Show("Song is saved in databse");
                Song = new tblSong();
                //sending signal to the list
                Update = true;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        /// <summary>
        /// If field is empty=can not click save button
        /// </summary>
        /// <returns></returns>
        private bool CanAddExecute()
        {
            if (String.IsNullOrEmpty(Song.Title) || String.IsNullOrEmpty(Song.Author) || String.IsNullOrEmpty(Song.Duration_s.ToString()) || Song.Duration_s < 0)
            {
                return false;
            }
            else
            {
                return true;
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
            createSong.Close();
        }
        private bool CanCloseExecute()
        {
            return true;
        }
    }
}
