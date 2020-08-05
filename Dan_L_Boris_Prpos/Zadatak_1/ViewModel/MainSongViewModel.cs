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
    class MainSongViewModel : ViewModelBase
    {
        Entity context = new Entity();
        MainSongView mainSongView;
        public MainSongViewModel(MainSongView mainSongOpen)
        {
            mainSongView = mainSongOpen;
            Song = new tblSong();
            SongList = GetSongs();
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
        private List<tblSong> songList;
        public List<tblSong> SongList
        {
            get
            {
                return songList;
            }
            set
            {
                songList = value;
                OnPropertyChanged("SongList");
            }
        }
        private ICommand delete;
        public ICommand Delete
        {
            get
            {
                if (delete == null)
                {
                    delete = new RelayCommand(param => DeleteExecute(), param => CanDeleteExecute());
                }
                return delete;
            }

        }
        private void DeleteExecute()
        {
            try
            {
                tblSong songToDelete = (from r in context.tblSongs where r.SongID == Song.SongID select r).FirstOrDefault();
                MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure? Song will be deleted", "Delete Confirmation", MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    context.tblSongs.Remove(songToDelete);
                    context.SaveChanges();
                    //refreshing list afterwards
                    SongList = GetSongs();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanDeleteExecute()
        {
            if (Song==null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private ICommand add;
        public ICommand Add
        {
            get
            {
                if (add==null)
                {
                    add = new RelayCommand(param => AddExecute(), param => CanAddExecute());
                }
                return add;
            }
        }
        private void AddExecute()
        {
            try
            {
                CreateSong createSong = new CreateSong();
                createSong.ShowDialog();

                if ((createSong.DataContext as CreateSongViewModel).Update==true)
                {
                    SongList = GetSongs();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanAddExecute()
        {
            return true;
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
            mainSongView.Close();
        }
        private bool CanCloseExecute()
        {
            return true;
        }

        private List<tblSong> GetSongs()
        {
            List<tblSong> list = new List<tblSong>();

            list = context.tblSongs.ToList();
            return list;
        }
    }
}
