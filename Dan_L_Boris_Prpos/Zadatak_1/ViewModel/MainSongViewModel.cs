using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
        private readonly BackgroundWorker worker = new BackgroundWorker();
        Entity context = new Entity();
        MainSongView mainSongView;
        public MainSongViewModel(MainSongView mainSongOpen)
        {
            mainSongView = mainSongOpen;
            Song = new tblSong();
            SongList = GetSongs();
            worker.DoWork += WorkerOnDoWork;
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
        private ICommand play;
        public ICommand Play
        {
            get
            {
                if (play==null)
                {
                    play = new RelayCommand(param => PlayExecute(), param => CanPlayExecute());
                }
                return play;
            }
        }
        private void PlayExecute()
        {
            try
            {
                if (!worker.IsBusy)
                {
                    worker.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private void WorkerOnDoWork(object sender, DoWorkEventArgs e)
        {

            WriteToFile(Song);
        }
        private void WriteToFile(tblSong s)
        {
            try
            {
                string path = @"..\..\file.txt";
                StreamWriter sw = new StreamWriter(path,true);
                sw.WriteLine("{0},Reproduction time START:{1}, Duration(s):{2}, Title:{3}", s.SongID, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"), s.Duration_s, s.Title);
                sw.Close();
                MessageBox.Show("Song has started");
                int counter = 0;
                Thread.Sleep(1000);
                counter++;
                if (counter==s.Duration_s)
                {
                    sw.WriteLine("{0},Reproduction time END:{1}, Duration(s):{2}, Title:{3}", s.SongID, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"), s.Duration_s, s.Title);
                    sw.Close();
                    MessageBox.Show("Song has finished");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private bool CanPlayExecute()
        {
            if (Song!=null)
            {
                return true;
            }
            else
            {
                return false;
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
