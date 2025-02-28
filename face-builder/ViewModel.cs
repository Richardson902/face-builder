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

        // ------ Face Data Model Properties ------

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

        // ------ Navigation ------
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

        // ------- Tracking Selected Face Id------
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

        // ------- Faces List ------
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

        // ------- Search ------
        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        // ------- Is New Face Flag ------
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

        // ------- Selected Face ------
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


        // ------- Methods ------
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

        private void LoadFacesToComboBox()
        {
            var faces = _dataManager.LoadFacesComboBox();
            FacesList.Clear();

            foreach (var face in faces)
            {
                FacesList.Add(face);
            }

            if (FacesList.Count > 0)
            {

                SelectedTabIndex = 2;
            }
            else
            {
                SelectedTabIndex = 0;
            }

        }

        private void DeleteFace()
        {
            if (SelectedFaceId > 0)
            {
                _dataManager.DeleteFace(SelectedFaceId);

                //Reload face list after deletion
                LoadFacesToComboBox();

                if (FacesList.Count > 0)
                {
                    //Select first face in the list
                    SelectedFace = FacesList[0];
                }
                else
                {
                    ClearData();
                    FaceBuilder.ClearCanvas();

                    //Reset selected face to avoid invalid reference
                    SelectedFaceId = -1;

                    // Create default empty key value pair
                    SelectedFace = new KeyValuePair<int, string>(-1, string.Empty);

                    IsNewFace = true; // Just in case anything crazy happens

                    SelectedTabIndex = 0;

                }
            }
        }

        private void SearchFace()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                MessageBox.Show("Please enter a name to search");
                return;
            }

            if (FacesList.Count == 0)
            {
                LoadFacesToComboBox();
            }

            var matchingFaces = FacesList.Where(face => face.Value.ToLower().Contains(SearchText.ToLower())).ToList();

            if (matchingFaces.Count > 0)
            {
                SelectedFace = matchingFaces[0];

                // Can show quantity of matching faces later i guess
            }
            else
            {
                MessageBox.Show("No matching faces found.");
            }

            SearchText = string.Empty;
        }

        public void nextTab()
        {
            SelectedTabIndex++;
        }

        public void prevTab()
        {
            SelectedTabIndex--;
        }

        // ------ Facial Features ------
        public ICommand HairNextCommand { get; }
        public ICommand HairPrevCommand { get; }
        public ICommand EyesNextCommand { get; }
        public ICommand EyesPrevCommand { get; }
        public ICommand NoseNextCommand { get; }
        public ICommand NosePrevCommand { get; }
        public ICommand MouthNextCommand { get; }
        public ICommand MouthPrevCommand { get; }

        // ------ Actions ------
        public ICommand RandomizeCommand { get; }
        public ICommand ClearFaceCommand { get; }

        // ------ Help ------
        public ICommand HelpKeybindsCommand { get; }
        public ICommand HelpAboutCommand { get; }
        public ICommand HelpImagesCommand { get; }

        // ------ Navigation ------
        public ICommand NextTabCommand { get; }
        public ICommand PrevTabCommand { get; }

        // ------ Face Data ------
        public ICommand SaveFaceCommand { get; }
        public ICommand ClearFaceDataCommand { get; }
        public ICommand EditFaceCommand { get; }
        public ICommand LoadFaceCommand { get; }
        public ICommand NewFaceCommand { get; }
        public ICommand DeleteFaceCommand { get; }
        public ICommand SearchFaceCommand { get; }

        public ViewModel()
        {
            _dataManager = new DataManager();

            IsNewFace = true;

            FacesList = new ObservableCollection<KeyValuePair<int, string>>();

            // Facial features
            HairNextCommand = new CommandHandler(() => FaceBuilder.HairNext(), true);
            HairPrevCommand = new CommandHandler(() => FaceBuilder.HairPrev(), true);
            EyesNextCommand = new CommandHandler(() => FaceBuilder.EyesNext(), true);
            EyesPrevCommand = new CommandHandler(() => FaceBuilder.EyesPrev(), true);
            NoseNextCommand = new CommandHandler(() => FaceBuilder.NoseNext(), true);
            NosePrevCommand = new CommandHandler(() => FaceBuilder.NosePrev(), true);
            MouthNextCommand = new CommandHandler(() => FaceBuilder.MouthNext(), true);
            MouthPrevCommand = new CommandHandler(() => FaceBuilder.MouthPrev(), true);

            // Actions
            RandomizeCommand = new CommandHandler(() => FaceBuilder.Randomize(), true);
            ClearFaceCommand = new CommandHandler(() => FaceBuilder.ClearCanvas(), true);


            // Help
            HelpKeybindsCommand = new CommandHandler(() => HelpManager.DisplayKeyBindings(), true);
            HelpAboutCommand = new CommandHandler(() => HelpManager.DisplayAbout(), true);
            HelpImagesCommand = new CommandHandler(() => HelpManager.DisplayAddImages(), true);

            // Navigation
            NextTabCommand = new CommandHandler(() => nextTab(), true);
            PrevTabCommand = new CommandHandler(() => prevTab(), true);

            // Face Data
            SaveFaceCommand = new CommandHandler(() => SaveFace(), true);
            LoadFaceCommand = new CommandHandler(() => LoadFacesToComboBox(), true);
            DeleteFaceCommand = new CommandHandler(() => DeleteFace(), true);
            SearchFaceCommand = new CommandHandler(() => SearchFace(), true);

            ClearFaceDataCommand = new CommandHandler(() => 
            {
                FaceBuilder.ClearCanvas();
                ClearData();
                SelectedTabIndex = 0;

            }, true);

            EditFaceCommand = new CommandHandler(() => 
            {
                SelectedTabIndex = 0;
                IsNewFace = false;

            }, true);

            NewFaceCommand = new CommandHandler(() =>
            {
                FaceBuilder.ClearCanvas();
                ClearData();
                IsNewFace = true;
                SelectedTabIndex = 0;

            }, true);

            // Combo Box Data
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
