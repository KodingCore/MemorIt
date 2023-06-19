using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MemorIt_by_IMLL
{
    internal class Sonns
    {
        public static string[] Listsonneries()
        {
            int lastDotIndex = 0;
            string fileName = "";
            string[] tableauSonneries = new string[15];
            string[] files = Directory.GetFiles(Application.StartupPath + "\\Sonneries\\", "*.mp3");
            for (int i = 0; i < files.Length; i++)
            {
                lastDotIndex = files[i].LastIndexOf('\\');
                fileName = files[i].Substring(lastDotIndex + 1);
                tableauSonneries[i] = fileName;
            }
            return tableauSonneries;
        }
    }
}