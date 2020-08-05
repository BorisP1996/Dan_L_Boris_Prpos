using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Zadatak_1.Command;
using Zadatak_1.Model;
using Zadatak_1.View;

namespace Zadatak_1.ViewModel
{
    class MainSongViewModel : ViewModelBase
    {
        static CountdownEvent countdown = new CountdownEvent(1);
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
        /// <summary>
        /// click on this button just calls bg worker
        /// </summary>
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
        /// <summary>
        /// Method that worker does
        /// </summary>
        /// <param name="s"></param>
        private void WriteToFile(tblSong s)
        {
            try
            {
                //writes data about the song that is parameter
                string path = @"..\..\file.txt";
                StreamWriter sw = new StreamWriter(path,true);
                sw.WriteLine("{0},Reproduction time START:{1}, Duration(s):{2}, Title:{3}", s.SongID, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"), s.Duration_s, s.Title);
                sw.Close();
                MessageBox.Show("Title: " + " " + s.Title + " \nAuthor:" + s.Author + " " + "\nSong has started");

                //counting when song if finished =>duration in seconds is deceremnted each second
                int counter = s.Duration_s.GetValueOrDefault();
                while (counter!=0)
                {
                    counter -= 1;
                    Thread.Sleep(1000);
                }
                countdown.Signal();
                //signalin that song is over and writing that to file and showing notification to the user
                if (countdown.IsSet)
                {
                    StreamWriter sw1 = new StreamWriter(path, true);
                    sw1.WriteLine("{0},Reproduction time END:{1}, Duration(s):{2}, Title:{3}", s.SongID, DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"), s.Duration_s, s.Title);
                        sw1.Close();
                        MessageBox.Show("Title: "+" "+s.Title+" \nAuthor:"+s.Author + " " +"\nSong has finished");
                    countdown.Reset();
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
        /// <summary>
        /// Method for deleting
        /// </summary>
        private void DeleteExecute()
        {
            try
            {
                tblSong songToDelete = (from r in context.tblSongs where r.SongID == Song.SongID select r).FirstOrDefault();
                MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure? Song will be deleted", "Delete Confirmation", MessageBoxButton.YesNo);
                //user confirmation
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
        /// <summary>
        /// Opens window for new song adding
        /// </summary>
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
        /// <summary>
        /// Gets list of the song from database
        /// </summary>
        /// <returns></returns>
        private List<tblSong> GetSongs()
        {
            List<tblSong> list = new List<tblSong>();

            list = context.tblSongs.ToList();
            return list;
        }
    }
}
