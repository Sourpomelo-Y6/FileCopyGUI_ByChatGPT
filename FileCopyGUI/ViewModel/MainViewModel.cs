using FileCopyGUI.Model;
using FileCopyGUI.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FileCopyGUI.ViewModel
{
    class MainViewModel : ObservableObject
    {
        public ObservableCollection<FileMapping> FileMappings { get; set; } = new ObservableCollection<FileMapping>();

        public MainViewModel()
        {
            TestText = "Hello,world!";
            LoadConfig();
        }


        private string testText;

        public string TestText
        {
            get => testText;
            set
            {
                testText = value;
                OnPropertyChanged("TextText");
            }
        }

        private Config _config;

        public Config Config
        {
            get { return _config; }
            set
            {
                _config = value;
                OnPropertyChanged(nameof(Config));
            }
        }

        private readonly ConfigService _configService = new ConfigService();


        private RelayCommand saveCommand;

        public ICommand SaveCommand
        {
            get
            {
                if (saveCommand == null)
                {
                    saveCommand = new RelayCommand(SaveConfig);
                }

                return saveCommand;
            }
        }

        public void SaveConfig()
        {
            Config.FileMappings = new List<FileMapping>(FileMappings);
            _configService.SaveConfig(Config);
        }


        private RelayCommand loadCommand;

        public ICommand LoadCommand
        {
            get
            {
                if (loadCommand == null)
                {
                    loadCommand = new RelayCommand(LoadConfig);
                }

                return loadCommand;
            }
        }

        public void LoadConfig()
        {
            Config = _configService.LoadConfig();
            FileMappings = new ObservableCollection<FileMapping>(Config.FileMappings);
        }

        private RelayCommand copyCommand;

        public ICommand CopyCommand
        {
            get
            {
                if (copyCommand == null)
                {
                    copyCommand = new RelayCommand(CopyFile);
                }

                return copyCommand;
            }
        }

        public void CopyFile()
        {
            foreach (var mapping in FileMappings)
            {
                if (File.Exists(mapping.SourcePath))
                {
                    // 宛先ディレクトリが存在しない場合は、ディレクトリを作成
                    var directory = Path.GetDirectoryName(mapping.DestinationPath);
                    if (directory != "" && !Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    // ファイルをコピー
                    File.Copy(mapping.SourcePath, mapping.DestinationPath, overwrite: true);
                }
                else
                {
                    // ファイルが存在しない場合の処理（エラーメッセージの表示など）
                }
            }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        private RelayCommand addMappingCommand;

        public ICommand AddMappingCommand
        {
            get
            {
                if (addMappingCommand == null)
                {
                    addMappingCommand = new RelayCommand(AddMapping);
                }

                return addMappingCommand;
            }
        }

        private void AddMapping()
        {
            FileMappings.Add(new FileMapping());
        }

        private RelayCommand removeMappingCommand;

        public ICommand RemoveMappingCommand
        {
            get
            {
                if (removeMappingCommand == null)
                {
                    removeMappingCommand = new RelayCommand(RemoveMapping, CanRemoveMapping);
                }

                return removeMappingCommand;
            }
        }

        private void RemoveMapping()
        {
            if (SelectedMapping != null)
            {
                FileMappings.Remove(SelectedMapping);
            }
        }

        private bool CanRemoveMapping()
        {
            return SelectedMapping != null;
        }

        // SelectedMapping プロパティも追加する必要があります
        private FileMapping _selectedMapping;
        public FileMapping SelectedMapping
        {
            get { return _selectedMapping; }
            set
            {
                _selectedMapping = value;
                OnPropertyChanged(nameof(SelectedMapping));
            }
        }




    }

}
