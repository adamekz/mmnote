//NETOCR by Adam Żyliński --- po oddaniu projektu wywalić polskie komentarze
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;

using System.IO;

using System.Drawing;
using System.Drawing.Imaging;

using AForge;
using AForge.Imaging;

using Emgu.CV.OCR;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.UI;
using Emgu.CV.CvEnum;
using FreeTextBoxControls.Design;

namespace NETOCR
{
    public partial class _Default : System.Web.UI.Page
    {
        public static String pic_file = "";
        public static Tesseract Do_ocr;
        public FileClassesDataContext files_base;
        protected void Page_Load(object sender, EventArgs e)
        {
            files_base = new FileClassesDataContext();
           /* Bitmap tmp = (Bitmap)Bitmap.FromFile("J:\\ocr_pages\\page0001.jpg");
            Emgu.CV.Image<Bgr, Byte> test = new Emgu.CV.Image<Bgr, byte>(tmp);     */       
        }
        
        //przygotowanie do procesu ustawiania pozycji obrazu przez użytkownika
        public void pre_ocr(String file)
        {
            Bitmap source = (Bitmap)Bitmap.FromFile(file);
            int resX = 0;
            int resY = 0;
            //rozróżnianie obrazów poziomych od pionowych
            if (source.Width >= source.Height)
            {
                resX = 200;
                resY = (int)((source.Height * 200) / source.Width);
            }
            else
            {
                resX = (int)((source.Width * 200) / source.Height);
                resY = 200;
            }
            AForge.Imaging.Filters.ResizeBilinear resTool = new AForge.Imaging.Filters.ResizeBilinear(resX, resY);
            source = resTool.Apply(source);
            //source.Dispose();
            source.Save(Server.MapPath("~/img/prev.bmp"));
            
            //source.Dispose();
            Image1.Width = resX;
            Image1.Height = resY;
            Image1.ImageUrl = "img/prev.bmp";
            
            Panel1.Visible = true;
        }
        //główne operacje na obrazie
        public void ocr()
        {

            //otworzenie pliku
            FileStream srcstream = new FileStream(pic_file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            //stworzenie bitmapy
            Bitmap source = new Bitmap(srcstream);
            //zmiana ustawień webform
           
            Panel1.Visible = false;
            Image1.Dispose();

            Label2.Text = "Processing...";
            Panel3.Visible = true;
            

            //Preperation code
            Bitmap ext = source;

            //AForge.Imaging.Filters.
            //Przekształcenie obrazu na skalę odcieni szarości - testować dla obrazów o różnej kolorystyce(opracować system wyznaczania parametrów filtru na podstawie RGB zdjęcia)
            AForge.Imaging.Filters.Grayscale grScl = new AForge.Imaging.Filters.Grayscale(0.2125, 0.0154, 0.0721 );            
            source = grScl.Apply(source);
            //Zwiększenie kontrastu
            AForge.Imaging.Filters.ContrastStretch conCor = new AForge.Imaging.Filters.ContrastStretch();
            
            source = conCor.Apply(source);
            //Wyostrzenie
            AForge.Imaging.Filters.Sharpen shp = new AForge.Imaging.Filters.Sharpen();
            source = shp.Apply(source);
                 

            //Segmentation code
            bool procesed = false;
           // Image2.Width = 350;
           // Image2.Height = (int)((source.Height * 200) / source.Width);
            

            try
            {
                Emgu.CV.Image<Bgr, Byte> to_rec = new Emgu.CV.Image<Bgr, byte>(source);
                Do_ocr = new Tesseract("tessdata", "eng", Tesseract.OcrEngineMode.OEM_DEFAULT);
                try
                {
                    Do_ocr.Recognize<Bgr>(to_rec);
                    //recognizedText.Text = ocr.GetText();
                    PastOCRBox.Text = Do_ocr.GetText();
                   // StatusBox.Text = "Finished! Ready for next one...";
                    Do_ocr.Dispose();
                    to_rec.Dispose();
                }
                catch (Exception exp)
                {
                    Label2.Text = "Recognition error! " + exp.Message;
                    Do_ocr.Dispose();
                    return;
                }
            }
            catch (Exception exp)
            {
                Label2.Text = "OCR engine failed! " + exp.Message;
                return;
            }
            

            
            
            //czyszczenie z plików tymczasowych
            
          //  source.Save("D:\\test.bmp");
          //  ext.Save("D:\\testcor.bmp");
            source.Dispose();
            srcstream.Close();
            srcstream.Dispose();
            //System.IO.File.Delete(pic_file);
            System.IO.File.Delete(Server.MapPath("~/img/prev.bmp"));
            System.IO.File.Delete(Server.MapPath("~/img/tmp.bmp"));
            //przygotować wygląd strony po rozpoznawaniu
            Panel3.Visible = false;
            Label1.Visible = false;
            Panel0.Visible = false;
            Panel5.Visible = false;
            
            Panel4.Visible = true;

            
        }
        //wywołanie właściwej części aplikacji
        protected void Button2_Click(object sender, EventArgs e)
        {
            ocr();
        }
        //procedura obracania pliku graficznego i pliku z podglądem
        public void rotation(double ang)
        {
            //inicjalizacja filtra
            AForge.Imaging.Filters.RotateBilinear rot = new AForge.Imaging.Filters.RotateBilinear(ang);
            //otwarcie plików
            FileStream srcstream = new FileStream(pic_file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            FileStream prevstream = new FileStream(Server.MapPath("~/img/prev.bmp"), FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            //stworzenie bitmap           
            Bitmap source = new Bitmap(srcstream);
            Bitmap prev = new Bitmap(prevstream);
            //użycie filtra
            source = rot.Apply(source);
            //zapisanie wyniku
            source.Save(Server.MapPath("~/img/tmp1.bmp"));
            //zwolnienie bitmapy
            ((IDisposable)source).Dispose();
            //zamknięcie strumienia
            srcstream.Close();
            
            prev = rot.Apply(prev);
            int resX = 0;
            int resY = 0;
            //rozróżnianie obrazów poziomych od pionowych
            if (prev.Width >= prev.Height)
            {
                resX = 200;
                resY = (int)((prev.Height * 200) / prev.Width);
            }
            else
            {
                resX = (int)((prev.Width * 200) / prev.Height);
                resY = 200;
            }            
            prev.Save(Server.MapPath("~/img/prev1.bmp"));
            ((IDisposable)prev).Dispose();
            prev = null;
            prevstream.Close();
            //ustalenie rozmiarów image1
            Image1.Width = resX;
            Image1.Height = resY;             
        }
        //rozwiązanie sprawy tymczasowych plików na serwerze
        public void filemg()
        {
            System.IO.File.Replace(Server.MapPath("~/img/tmp1.bmp"), pic_file, Server.MapPath("~/img/tmp.bmp"), true);
            System.IO.File.Replace(Server.MapPath("~/img/prev1.bmp"), Server.MapPath("~/img/prev.bmp"), Server.MapPath("~/img/tmp1.bmp"), true);
            System.IO.File.Delete(Server.MapPath("~/img/tmp1.bmp"));
            System.IO.File.Delete(Server.MapPath("~/img/tmp.bmp"));
            Image1.ImageUrl = "img/prev.bmp";
        }
        //obrót w lewo
        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            rotation(90);
            filemg();
        }
        //obrót w prawo
        protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        {
            rotation(-90);
            filemg();
        }


        protected void DwnButton_Click(object sender, EventArgs e)
        {
            if (TitleBox.Text == "")
            {
                PastStatus.Text = "Write file title first!";
                return;
            }
            PastStatus.Text = "";
            string txt = PastOCRBox.Text;
            string file = Server.MapPath("~/img/") + TitleBox.Text + ".txt";
            StreamWriter tmpstr = new StreamWriter(file);
            tmpstr.Write(txt);
            tmpstr.Close();
            tmpstr.Dispose();
            String FileName = TitleBox.Text + ".txt";
            String FilePath = file; //Replace this
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();
            response.ContentType = "text/plain";
            response.AddHeader("Content-Disposition", "attachment; filename=" + FileName + ";");
            response.TransmitFile(FilePath);
            response.Flush();
            response.End();
            System.IO.File.Delete(file);
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (TitleBox.Text == "")
            {
                PastStatus.Text = "Write file title first!";
                return;
            }
            PastStatus.Text = "";
            string txt = PastOCRBox.Text;
            string file = Server.MapPath("~/storage/") + TitleBox.Text + ".txt";
            StreamWriter tmpstr = new StreamWriter(file);
            tmpstr.Write(txt);
            tmpstr.Close();
            tmpstr.Dispose();
            byte[] input = System.Text.Encoding.ASCII.GetBytes(PastOCRBox.Text);   
            
            MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(input);
            string shash = "";

            foreach (byte x in hash) shash += x.ToString("X2");
            


            File to_file = new File
            {
                Name = TitleBox.Text,
                Path = file,
                Hash = shash,
                date = DateTime.Now,
                Tags = TagsBox.Text
            };
            files_base.Files.InsertOnSubmit(to_file);

            files_base.SubmitChanges();
            SaveButton.Enabled = false;    

        }  

       
    }
}
