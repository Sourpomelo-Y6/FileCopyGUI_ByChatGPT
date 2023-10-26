using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCopyGUI.Model
{
    public class Config
    {
        public List<FileMapping> FileMappings { get; set; } = new List<FileMapping>();
    }

    public class FileMapping
    {
        public string SourcePath { get; set; }
        public string DestinationPath { get; set; }
    }
}
