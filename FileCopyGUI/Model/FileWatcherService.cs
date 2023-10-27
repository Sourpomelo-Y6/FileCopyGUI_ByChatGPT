using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCopyGUI.Model
{
    public class FileWatcherService
    {
        private List<FileSystemWatcher> watchers = new List<FileSystemWatcher>();

        public IEnumerable<FileMapping> _mappings { get; private set; }

        public FileWatcherService(IEnumerable<FileMapping> mappings)
        {
            _mappings = mappings;

            foreach (var _mapping in _mappings) 
            {
                var sourceDirectoryPath = Path.GetDirectoryName(_mapping.SourcePath);

                if (sourceDirectoryPath == "")
                {
                    sourceDirectoryPath = Directory.GetCurrentDirectory();
                }

                var SourcePath = Path.Combine(sourceDirectoryPath, _mapping.SourcePath);
                _mapping.SourcePath = SourcePath;

                var destinationDirectoryPath = Path.GetDirectoryName(_mapping.DestinationPath);

                if (destinationDirectoryPath == "")
                {
                    destinationDirectoryPath = Directory.GetCurrentDirectory();
                }

                var DestinationPath = Path.Combine(destinationDirectoryPath,_mapping.DestinationPath);
                _mapping.DestinationPath = DestinationPath;

                var watcher = new FileSystemWatcher
                {
                    Path = sourceDirectoryPath,
                    Filter = Path.GetFileName(_mapping.SourcePath),
                    NotifyFilter = NotifyFilters.LastWrite
                };

                if (watcher.Path == "") 
                {
                    watcher.Path = Directory.GetCurrentDirectory();
                }

                watcher.Changed += (sender, e) =>
                {
                    // ファイル変更時の処理をここに記述
                    Console.WriteLine($"{e.FullPath} has been changed.");

                    // この例では、変更が検出されたらそのファイルを目的地にコピーする
                    foreach (var mapping in _mappings)
                    {
                        if (mapping.SourcePath == e.FullPath)
                        {
                            File.Copy(e.FullPath, mapping.DestinationPath, true);
                            Console.WriteLine($"copy {e.FullPath} > {mapping.DestinationPath}");
                        }
                    }
                };

                watchers.Add(watcher);
            }
        }

        

        public void StartWatching()
        {
            foreach (var watcher in watchers)
            {
                watcher.EnableRaisingEvents = true;
            }
        }

        public void StopWatching()
        {
            foreach (var watcher in watchers)
            {
                watcher.EnableRaisingEvents = false;
            }
        }
    }
}
