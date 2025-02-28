using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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

        private bool _isNewFace;
        public bool IsNewFace
        {
            get { return _isNewFace; }
            set
            {
                _isNewFace = value;
                OnPropertyChanged(nameof(IsNewFace));
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

        private int _selectedFaceId;
        public int SelectedFaceId
        {
            get { return _selectedFaceId; }
            set
            {
                _selectedFaceId = value;
                OnPropertyChanged(nameof(SelectedFaceId));
            }
        }

        private ObservableCollection<KeyValuePair<int, string>> _facesList;
        public ObservableCollection<KeyValuePair<int, string>> FacesList
        {
            get { return _facesList; }
            set
            {
                _facesList = value;
                OnPropertyChanged(nameof(FacesList));
            }
        }

        private KeyValuePair<int, string> _selectedFace;
        public KeyValuePair<int, string> SelectedFace
        {
            get { return _selectedFace; }
            set
            {
                _selectedFace = value;
                SelectedFaceId = value.Key;
                OnPropertyChanged(nameof(SelectedFace));
                LoadSelectedFaceData();
            }
        }

        private void LoadSelectedFaceData()
        {
            if (SelectedFaceId > 0)
            {
                _dataManager.LoadSelectedFaceData(this, SelectedFaceId);
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
                int faceId = -1;

                if (IsNewFace)
                {
                    faceId = _dataManager.SaveFaceData(this);
                }
                else
                {
                    _dataManager.UpdateFaceData(this, SelectedFaceId);
                    faceId = SelectedFaceId;
                }
                LoadFacesToComboBox();
                SelectedTabIndex = 2;

                if (faceId > 0)
                {
                    var faceToSelect = FacesList.FirstOrDefault(f => f.Key == faceId);

                    if (!faceToSelect.Equals(default(KeyValuePair<int, string>))) 
                    {
                        SelectedFace = faceToSelect;
                    }
                }

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

        //Create load data method that populates the properties of the model.
        private void LoadFacesToComboBox()
        {
            var faces = _dataManager.LoadFacesComboBox();
            FacesList.Clear();

            foreach (var face in faces)
            {
                FacesList.Add(face);
            }

        }

        public void nextTab()
        {
            SelectedTabIndex++;
        }

        public void prevTab()
        {
            SelectedTabIndex--;
        }

        public ICommand NextTabCommand { get; }
        public ICommand PrevTabCommand { get; }
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
        public ICommand EditFaceCommand { get; }
        public ICommand LoadFaceCommand { get; }

        public ICommand UpdateCanvasTestCommand { get; }
        public ICommand NewFaceCommand { get; }

        public ViewModel()
        {
            _dataManager = new DataManager();
            IsNewFace = true;

            FacesList = new ObservableCollection<KeyValuePair<int, string>>();

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
            SaveFaceCommand = new CommandHandler(() => {
                SaveFace();
                }, true);
            ClearFaceDataCommand = new CommandHandler(() => {
                FaceBuilder.ClearCanvas();
                ClearData();
                SelectedTabIndex = 0;
            }, true);
            NextTabCommand = new CommandHandler(() => nextTab(), true);
            PrevTabCommand = new CommandHandler(() => prevTab(), true);
            EditFaceCommand = new CommandHandler(() => {
                SelectedTabIndex = 0;
                IsNewFace = false;
            }, true);
            LoadFaceCommand = new CommandHandler(() => {
                LoadFacesToComboBox();
                SelectedTabIndex = 2;
            }, true);
            NewFaceCommand = new CommandHandler(() =>
            {
                FaceBuilder.ClearCanvas();
                ClearData();
                IsNewFace = true;
                SelectedTabIndex = 0;
                }, true);

            // Testing new implementation
            UpdateCanvasTestCommand = new CommandHandler(() => FaceBuilder.UpdateCanvas(), true);

            OccupationOptions = new ObservableCollection<string> { "", "Developer", "Artist", "Designer", "Scientist" };
            HobbyOptions = new ObservableCollection<string> { "", "Gaming", "Reading", "Sports", "Hiking" };

        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
