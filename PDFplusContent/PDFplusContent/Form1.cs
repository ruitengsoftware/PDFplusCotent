
using Aspose.Pdf;
using iTextSharp.text;
using PDFplusContent.Controller;
using RuiTengDll;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.SymbolStore;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDFplusContent
{
    public partial class Form1 : Form
    {
        ControllerForm _mycontroller = new ControllerForm();
        public Form1()
        {
            InitializeComponent();
        }

        private void lbl_addwenjian_Click(object sender, EventArgs e)
        {
            //获得文件
            var list = _mycontroller.GetSingleFile();
            //添加到mytreeview中
            foreach (string  item in list)
            {
                TreeNode mynode = new TreeNode();
                mynode.Name = item;
                mynode.Text = Path.GetFileName(item);

                mytreeview.Nodes.Add(mynode);

            }



        }

        private void label2_Click(object sender, EventArgs e)
        {
            //获得文件夹名称
            string dir = _mycontroller.GetDir();
            //构造主节点
            TreeNode mynode = new TreeNode();
           
            mynode.Text = dir;
            mynode.Name = dir;
            //给主节点复制子节点
            var files = _mycontroller.GetChildFile(dir);
            foreach (var item in files)
            {
                TreeNode childnode = new TreeNode();
                childnode.Text = item.Name;
                childnode.Name = item.FullName;
                mynode.Nodes.Add(childnode);
            }

            //tree添加子节点
            mytreeview.Nodes.Add(mynode);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            mytreeview.Nodes.Clear();
        }

        private void mytreeview_AfterCheck(object sender, TreeViewEventArgs e)
        {
            //通过鼠标或者键盘触发事件，防止修改节点的Checked状态时候再次进入
            if (e.Action == TreeViewAction.ByMouse || e.Action == TreeViewAction.ByKeyboard)
            {
                SetChildNodeCheckedState(e.Node, e.Node.Checked);
                SetParentNodeCheckedState(e.Node, e.Node.Checked);
            }
        }
        //设置子节点状态
        private void SetChildNodeCheckedState(TreeNode currNode, bool isCheckedOrNot)
        {
            if (currNode.Nodes == null) return; //没有子节点返回
            foreach (TreeNode tmpNode in currNode.Nodes)
            {
                tmpNode.Checked = isCheckedOrNot;
                SetChildNodeCheckedState(tmpNode, isCheckedOrNot);
            }
        }

        //设置父节点状态
        private void SetParentNodeCheckedState(TreeNode currNode, bool isCheckedOrNot)
        {
            if (currNode.Parent == null) return; //没有父节点返回
            if (isCheckedOrNot) //如果当前节点被选中，则设置所有父节点都被选中
            {
                currNode.Parent.Checked = isCheckedOrNot;
                SetParentNodeCheckedState(currNode.Parent, isCheckedOrNot);
            }
            else //如果当前节点没有被选中，则当其父节点的子节点有一个被选中时，父节点被选中，否则父节点不被选中
            {
                bool checkedFlag = false;
                foreach (TreeNode tmpNode in currNode.Parent.Nodes)
                {
                    if (tmpNode.Checked)
                    {
                        checkedFlag = true;
                        break;
                    }
                }
                currNode.Parent.Checked = checkedFlag;
                SetParentNodeCheckedState(currNode.Parent, checkedFlag);
            }
        }

        private void mytreeview_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
           


        }

        private void mytreeview_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //获得当前节点文件名
            string filename = mytreeview.SelectedNode.Name;
            //转化成bitmap

            var bitmap = _mycontroller.ConvertPdf2Bitmap(filename);
            //显示再窗体上
            pb_display.Image = bitmap;

        }

        private void label5_Click(object sender, EventArgs e)
        {
            //获得图片
            
            string pdffile = mytreeview.SelectedNode.Name;
            //获得添加的文字信息
            string strinfo = mytextbox.Text;

            //再图片上花花
            System.Drawing.Image smallImg = _mycontroller.ConvertPdf2Bitmap(pdffile);
           
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(smallImg))
                {
                //添加头像
                //g.DrawImage(smallImg, 0, 0, smallImg.Width, smallImg.Height);
                //SolidBrush drawBush = new SolidBrush(Color.Red);
                //Font drawFont = new Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Millimeter);
                //string newPath = path + "\\" + filename + ".png";
                //写汉字
                var style = cb_bold.Checked ? System.Drawing.FontStyle.Bold : System.Drawing.FontStyle.Regular;

                    g.DrawString(strinfo,new System.Drawing.Font(tb_ziti.Text,Convert.ToSingle( numericsize.Value),style ), new SolidBrush(lbl_color.BackColor),Convert.ToSingle( numericX.Value), Convert.ToSingle(numericY.Value));
                    //bigImage.Save(newPath, System.Drawing.Imaging.ImageFormat.Png);
                    
                } 

            //显示图片
            pb_display.Image = smallImg;



        }
        /// <summary>
        /// 点击开始按钮时出发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label7_Click(object sender, EventArgs e)
        {
            //获得pbdisplay 中的图片
          MemoryStream ms = new MemoryStream();
            string temppath = Environment.CurrentDirectory + @"\temp.jpg";
          pb_display.Image.Save(temppath, ImageFormat.Jpeg);
            //用aspose打开
            iTextSharp.text.Document mydocument = new iTextSharp.text.Document();
            iTextSharp.text.pdf.PdfWriter.GetInstance(mydocument, new FileStream(@"C:\Users\瑞腾软件\Desktop\testpdf.pdf", FileMode.Create, FileAccess.ReadWrite));

           // iTextSharp.text.pdf.PdfWriter.GetInstance(mydocument, new FileStream(@"C:\Users\瑞腾软件\Desktop\testpdf.pdf", FileMode.Create, FileAccess.ReadWrite));
            mydocument.Open();
            var imageStream = new FileStream(temppath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            pb_display.Image.Save(imageStream,ImageFormat.Jpeg);
            var image = iTextSharp.text.Image.GetInstance(imageStream);
            image.Alignment = iTextSharp.text.Image.ALIGN_MIDDLE;
            mydocument.Add(image);
            mydocument.Close();
        }

        private void lbl_color_Click(object sender, EventArgs e)
        {
            ColorDialog mycd = new ColorDialog();
            if (mycd.ShowDialog()==DialogResult.OK)
            {
                lbl_color.BackColor = mycd.Color;


            }
        }

        private void lbl_style_Click(object sender, EventArgs e)
        {
            FontDialog myfd = new FontDialog();
            if (myfd.ShowDialog()==DialogResult.OK)
            {
                //获得字体名称给，大小，粗体
                string fontname = myfd.Font.Name;
                decimal fontsize =Convert.ToDecimal( myfd.Font.Size);
                bool bold = myfd.Font.Bold;
                tb_ziti.Text = fontname;
                numericsize.Value = fontsize;
                cb_bold.Checked = bold;

            }
        }
        UIHelper _myui = new UIHelper();
        private void lbl_addwenjian_Paint(object sender, PaintEventArgs e)
        {
            _myui.DrawRoundRect((Control)sender);
        }

        private void lbl_addwenjian_MouseEnter(object sender, EventArgs e)
        {
            int margin = ((Control)sender).Margin.Top;
            _myui.UpdateCSize((Control)sender,new Padding(margin-1));
            _myui.UpdateCC((Control)sender, System.Drawing.Color.OrangeRed, System.Drawing.Color.White);
        }

        private void lbl_addwenjian_MouseLeave(object sender, EventArgs e)
        {
            int margin = ((Control)sender).Margin.Top;
            _myui.UpdateCSize((Control)sender, new Padding(margin +1));
            _myui.UpdateCC((Control)sender, System.Drawing.Color.Tomato, System.Drawing.Color.White);

        }
    }
}
