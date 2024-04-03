using ScrapySharp.Extensions;
using ScrapySharp.Network;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using System.Net;
using System.Text.RegularExpressions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Net.WebRequestMethods;
using ImageResizer;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Drawing.Imaging;

namespace scraping
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private struct programmazione
        {
            public string canale;
            public string titolo;
            public string orario;
            public string descrizione;

            public override string ToString()
            {
                return $"{orario} * {canale} * {titolo} * {descrizione}";
            }


        }
        List<programmazione> elencoProgrammi = new List<programmazione>();
        programmazione programmaAttuale;

        private int num = 0;




        private async void button1_Click(object sender, EventArgs e)
        {
            var luogo = textBox1.Text;
            string url = $"https://www.dovesciare.it/webcam/{luogo}";

            label1.Text = url;


            ScrapingBrowser browser = new ScrapingBrowser();
            browser.AllowAutoRedirect = true;
            browser.AllowMetaRedirect = true;

            WebPage webpage = await browser.NavigateToPageAsync(new Uri(url));

            var webcam = webpage.Html.OwnerDocument.DocumentNode.CssSelect("div.row").ToList()[1];
            var urlImg = webpage.Html.OwnerDocument.DocumentNode.CssSelect("section.col-sm-12");
            var region = urlImg.CssSelect("div.region");
            var webc = region.CssSelect("article.webcam");
            var jumbotron = webc.CssSelect("div.jumbotron");
            var container = jumbotron.CssSelect("div.container");
            var row = container.CssSelect("div.row");
            var blocco = row.CssSelect("div#webcam_intro_left").ToList();
            var img = blocco.CssSelect("img.img-responsive").First().GetAttributeValue("src");
            var img1 = $"https://www.dovesciare.it{img}";
            var request = WebRequest.Create(img1);

            try
            {
               
                using (WebClient webClient = new WebClient())
                using (MemoryStream originalStream = new MemoryStream(webClient.DownloadData(img1)))
                {
                    
                    using (Image originalImage = Image.FromStream(originalStream))
                    {
                       
                        int newWidth = 400; 
                        int newHeight = 400; 

                        // Resize the image
                        using (Image resizedImage = new Bitmap(newWidth, newHeight))
                        using (Graphics graphics = Graphics.FromImage(resizedImage))
                        {
                            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            graphics.DrawImage(originalImage, 0, 0, newWidth, newHeight);

                            
                            Bitmap bitmap = new Bitmap(resizedImage);

                            
                            pictureBox1.Image = bitmap;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
           
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            var luogo = textBox1.Text;
            string url = $"https://www.dovesciare.it/localita/{luogo}";




            ScrapingBrowser browser = new ScrapingBrowser();
            browser.AllowAutoRedirect = true;
            browser.AllowMetaRedirect = true;

            {
                WebPage webpage = await browser.NavigateToPageAsync(new Uri(url));

                var tot = webpage.Html.OwnerDocument.DocumentNode.CssSelect("div.dialog-off-canvas-main-canvas");
                var main = tot.CssSelect("div.main-container");
                var row = main.CssSelect("div.row");
                var col = row.CssSelect("section.col-sm-12");
                var region = col.CssSelect("div.region");
                var nomar = region.CssSelect("div#locadett_dettagli");
                var conteiner = nomar.CssSelect("div.container");

                var row1 = conteiner.CssSelect("div.row");





            }
        }
    
    }
}

