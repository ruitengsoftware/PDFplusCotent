using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aspose.Pdf;

namespace PDFplusContent.Controller
{
    public class ControllerForm
    {

        //获得单个文件
        public List<string> GetSingleFile()
        {
            List<string> list = new List<string>();
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                list.AddRange(ofd.FileNames.ToArray());
            }
            return list;
        }

        //获得整个文件夹

        public string GetDir()
        {
            string mydir = string.Empty;
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                mydir = fbd.SelectedPath;
            }
            return mydir;
        }
        //获得文件夹下所有文件

        /// <summary>
        /// 获得文件夹下所有的文件
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public FileInfo[] GetChildFile(string dir)
        {
            List<string> list = new List<string>();
            DirectoryInfo mydirinfo = new DirectoryInfo(dir);
            var result = mydirinfo.GetFiles();
            return result;
        }


        public Bitmap ConvertPdf2Bitmap(string file)
        {
            try
            {
                Aspose.Pdf.Document mypdf = new Document(file);

                var device = new Aspose.Pdf.Devices.JpegDevice();
                MemoryStream ms = new MemoryStream();
                device.Process(mypdf.Pages[1], ms);
                return new Bitmap(ms);

            }
            catch { return null; }
        }



    }
}
