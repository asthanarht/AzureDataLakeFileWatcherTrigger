using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdlsFileWatcherTriggerExtension
{
    public class AdlsFileChangeEvent
    {
        public string FileName { get; set; }

        public string FileFullPath { get; set; }

        public Stream FileStram { get; set; }


    }
}
