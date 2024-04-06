using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flora.ViewModel
{
    class BackupVM : Utilities.ViewModelBase
    {
        public string BackupPath { get; set; }
        public class BackupFile
        {
            public string FileName { get; set; }
        }
        public ObservableCollection<BackupFile> BackupFiles { get; set; }
        public BackupVM()
        {
            BackupPath = System.IO.Directory.GetCurrentDirectory();
            BackupFiles = new ObservableCollection<BackupFile>();
            var files = System.IO.Directory.GetFiles(BackupPath + "\\Backup");
            foreach (var file in files)
            {
                var fileName = System.IO.Path.GetFileName(file);
                BackupFiles.Add(new BackupFile { FileName = fileName });
            }

        }
    }
}
