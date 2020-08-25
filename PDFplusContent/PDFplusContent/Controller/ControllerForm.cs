using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aspose.Pdf;
using Aspose.Pdf.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;

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

        /// <summary>
        /// 将文pdf文件转换成emf格式的图片
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public System.Drawing.Image ConvertPdf2Bitmap(string file)
        {
            Aspose.Pdf.Document mypdf = new Aspose.Pdf.Document(file);
            var device = new Aspose.Pdf.Devices.JpegDevice(1000,600);//emf格式不能很好的显示预览
            //var device = new Aspose.Pdf.Devices.EmfDevice(100, 60);
            //MemoryStream ms = new MemoryStream();
            //string savepath = $"{System.Environment.CurrentDirectory}\\temp.emf";
            //FileStream fs = new FileStream(savepath, FileMode.OpenOrCreate);
            MemoryStream ms = new MemoryStream();
            device.Process(mypdf.Pages[1], ms);
            //System.Drawing.Image myimg = System.Drawing.Image.FromStream(ms);
            System.Drawing.Image img = new Bitmap(ms);
            
            return img;
            //return myimg;
            //System.Drawing.Image result = System.Drawing.Image.FromFile(savepath);
            //return result;

        }




        public void SavePdf(System.Drawing.Image mybp, string savepath)
        {
            MemoryStream ms = new MemoryStream();
            string temppath = Environment.CurrentDirectory + @"\temp2.emf";

            mybp.Save(temppath, ImageFormat.Emf);

            //System.Drawing.Image bm = new System.Drawing.Bitmap(500,300,PixelFormat.Format32bppArgb);
            ////新建一个画板
            //System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bm);
            ////设置高质量插值法  
            //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
            ////设置高质量,低速度呈现平滑程度  
            //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            //g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
            ////消除锯齿
            //g.SmoothingMode = SmoothingMode.AntiAlias;
            ////在指定位置并且按指定大小绘制原图片的指定部分
            //g.DrawImage(mybp, 0, 0, 500, 300);
            ////g.DrawImage(mybp, new System.Drawing.Rectangle(0,0,5000,3000), 0, 0, mybp.Width, mybp.Height, GraphicsUnit.Pixel);
            ////以jpg格式保存缩略图
            //bm.Save(temppath, System.Drawing.Imaging.ImageFormat.Bmp);



            //用itextsharp打开
            iTextSharp.text.Document mydocument = new iTextSharp.text.Document(new iTextSharp.text.Rectangle(148.8f, 84.2f));
            iTextSharp.text.pdf.PdfWriter.GetInstance(mydocument, new FileStream(savepath, FileMode.Create, FileAccess.ReadWrite));
            mydocument.Open();
            var imageStream = new FileStream(temppath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            mybp.Save(imageStream, ImageFormat.Png);
            var image = iTextSharp.text.Image.GetInstance(imageStream);
            iTextSharp.text.Rectangle Rec = new iTextSharp.text.Rectangle(148.8f, 84.2f);
            //image.ScaleAbsolute(Rec);
            image.ScaleToFit(Rec);
            image.SetAbsolutePosition(0f, 0f);
            image.Alignment = iTextSharp.text.Image.ALIGN_MIDDLE;
            mydocument.Add(image);
            mydocument.Close();




            // PdfReader pdfReader = new PdfReader(@"C:\Users\瑞腾软件\Desktop\标签码\X002M8DMEZ 起车订 16.pdf");//读pdf
            // PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(savepath, FileMode.Create, FileAccess.Write, FileShare.None));
            //BaseFont baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\Arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);//获取系统的字体
            ////BaseFont bf=BaseFont.font

            ////BaseFont baseFont = BaseFont.CreateFont() ;

            // iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont,9);
            // Phrase p = new Phrase("我的测试", font);
            // PdfContentByte over = pdfStamper.GetOverContent(1);//PdfContentBye类，用来设置图像和文本的绝对位置
            // ColumnText.ShowTextAligned(over, Element.ALIGN_CENTER, p, 50, 50, 0);
            // pdfStamper.Close();





            //Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument();
            //doc.LoadFromFile(@"C:\Users\瑞腾软件\Desktop\标签码\X002M8DMEZ 起车订 16.pdf");

            //Spire.Pdf.Graphics.PdfFont font = new Spire.Pdf.Graphics.PdfFont();

            //string text = "HelloWorld";

            //PointF point = new PointF(10, 10);
            //PdfPageBase page = doc.Pages[0];
            //page.Canvas.DrawString(text, font, PdfBrushes.Red, point);
            //doc.SaveToFile(savepath);




        }


        public List<TreeNode> GetNodeList(TreeNode node)
        {
            List<TreeNode> list = new List<TreeNode>();
            if (node.Nodes.Count > 0)
            {
                foreach (TreeNode item in node.Nodes)
                {
                    list.AddRange(GetNodeList(item));
                }

            }
            else
            {
                list.Add(node);
            }
            return list;
        }


        public List<TreeNode> ConvertCollectionToList(TreeNode mynode)
        {
            List<TreeNode> list = new List<TreeNode>();
            foreach (TreeNode item in mynode.Nodes)
            {
                list.Add(item);
            }
            return list;

        }

        public System.Drawing.Image DrawImage(System.Drawing.Image smallImg, string strinfo, bool bold, string fontname, decimal fontsize, System.Drawing.Color color, decimal X, decimal Y)
        {
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(smallImg))
            {
                //添加头像
                //g.DrawImage(smallImg, 0, 0, smallImg.Width, smallImg.Height);
                //SolidBrush drawBush = new SolidBrush(Color.Red);
                //Font drawFont = new Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Millimeter);
                //string newPath = path + "\\" + filename + ".png";
                //写汉字
                var style = bold ? System.Drawing.FontStyle.Bold : System.Drawing.FontStyle.Regular;

                g.DrawString(strinfo, new System.Drawing.Font(fontname, Convert.ToSingle(fontsize), style), new SolidBrush(color), Convert.ToSingle(X), Convert.ToSingle(Y));
                //bigImage.Save(newPath, System.Drawing.Imaging.ImageFormat.Png);

            }
            return smallImg;
        }




    }
}
