using FileCopyGUI.Model;
using FileCopyGUI.ViewModel;
using System.Collections.Generic;
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
            if (File.Exists(Config.SourcePath))
            {
                File.Copy(Config.SourcePath, Config.DestinationPath, overwrite: true);
            }
            else
            {
                Message = "Source File is not exist.";
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
    }

}
