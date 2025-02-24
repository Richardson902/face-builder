using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace face_builder
{
    public class ViewModel : INotifyPropertyChanged
    {
        private readonly DataManager _dataManager;

        private int _selectedTabIndex;
        public int SelectedTabIndex
        {
            get { return _selectedTabIndex; }
            set
            {
                _selectedTabIndex = value;
                OnPropertyChanged(nameof(SelectedTabIndex));
            }
        }

        private string _firstName;

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set
            {
                _address = value;
                OnPropertyChanged(nameof(Address));
            }
        }

        private string _selectedOccupation;

        public ObservableCollection<string> OccupationOptions { get; }

        public string SelectedOccupation
        {
            get { return _selectedOccupation; }
            set
            {
                _selectedOccupation = value;
                OnPropertyChanged(nameof(SelectedOccupation));
            }
        }

        private string _selectedHobby;

        public ObservableCollection<string> HobbyOptions { get; }
        public string SelectedHobby
        {
            get { return _selectedHobby; }
            set
            {
                _selectedHobby = value;
                OnPropertyChanged(nameof(SelectedHobby));
            }
        }

        private bool _isDogLover;

        public bool IsDogLover
        {
            get { return _isDogLover; }
            set
            {
                _isDogLover = value;
                OnPropertyChanged(nameof(IsDogLover));
            }
        }

        private bool _isCatLover;

        public bool IsCatLover
        {
            get { return _isCatLover; }
            set
            {
                _isCatLover = value;
                OnPropertyChanged(nameof(IsCatLover));
            }
        }

        public bool CanSaveFace()
        {
            return !string.IsNullOrEmpty(FirstName) &&
                    !string.IsNullOrEmpty(LastName) &&
                    !string.IsNullOrEmpty(Address) &&
                    !string.IsNullOrEmpty(SelectedOccupation) &&
                    !string.IsNullOrEmpty(SelectedHobby) &&
                    FaceBuilder.IsUpdateHair &&
                    FaceBuilder.IsUpdateEyes &&
                    FaceBuilder.IsUpdateNose &&
                    FaceBuilder.IsUpdateMouth;
        }

        private void SaveFace()
        {
            if (CanSaveFace())
            {

                _dataManager.SaveFaceData(this);
            }
            else
            {
                MessageBox.Show("Error. Please make sure all fields are populated and face is built.");
            }
        }

        private void ClearData()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Address = string.Empty;
            SelectedOccupation = string.Empty;
            SelectedHobby = string.Empty;
            IsDogLover = false;
            IsCatLover = false;
        }


        public ICommand HairNextCommand { get; }
        public ICommand HairPrevCommand { get; }
        public ICommand EyesNextCommand { get; }
        public ICommand EyesPrevCommand { get; }
        public ICommand NoseNextCommand { get; }
        public ICommand NosePrevCommand { get; }
        public ICommand MouthNextCommand { get; }
        public ICommand MouthPrevCommand { get; }
        public ICommand RandomizeCommand { get; }
        public ICommand ClearFaceCommand { get; }
        public ICommand HelpKeybindsCommand { get; }
        public ICommand HelpAboutCommand { get; }
        public ICommand HelpImagesCommand { get; }
        public ICommand SaveFaceCommand { get; }
        public ICommand ClearFaceDataCommand { get; }

        public ViewModel()
        {
            _dataManager = new DataManager();

            HairNextCommand = new CommandHandler(() => FaceBuilder.HairNext(), true);
            HairPrevCommand = new CommandHandler(() => FaceBuilder.HairPrev(), true);
            EyesNextCommand = new CommandHandler(() => FaceBuilder.EyesNext(), true);
            EyesPrevCommand = new CommandHandler(() => FaceBuilder.EyesPrev(), true);
            NoseNextCommand = new CommandHandler(() => FaceBuilder.NoseNext(), true);
            NosePrevCommand = new CommandHandler(() => FaceBuilder.NosePrev(), true);
            MouthNextCommand = new CommandHandler(() => FaceBuilder.MouthNext(), true);
            MouthPrevCommand = new CommandHandler(() => FaceBuilder.MouthPrev(), true);
            RandomizeCommand = new CommandHandler(() => FaceBuilder.Randomize(), true);
            ClearFaceCommand = new CommandHandler(() => FaceBuilder.ClearCanvas(), true);
            HelpKeybindsCommand = new CommandHandler(() => HelpManager.DisplayKeyBindings(), true);
            HelpAboutCommand = new CommandHandler(() => HelpManager.DisplayAbout(), true);
            HelpImagesCommand = new CommandHandler(() => HelpManager.DisplayAddImages(), true);
            SaveFaceCommand = new CommandHandler(() => SaveFace(), true);
            ClearFaceDataCommand = new CommandHandler(() => {
                FaceBuilder.ClearCanvas();
                ClearData();
                SelectedTabIndex = 0;
            }, true);

            OccupationOptions = new ObservableCollection<string> { "Developer", "Artist", "Designer", "Scientist" };
            HobbyOptions = new ObservableCollection<string> { "Gaming", "Reading", "Sports", "Hiking" };

        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
