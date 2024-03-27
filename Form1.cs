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
            string url = $"https://www.3bmeteo.com/meteo/{luogo}";




            ScrapingBrowser browser = new ScrapingBrowser();
            browser.AllowAutoRedirect = true;
            browser.AllowMetaRedirect = true;

            {
                WebPage webpage = await browser.NavigateToPageAsync(new Uri(url));

                var wrapper = webpage.Html.OwnerDocument.DocumentNode.CssSelect("div#wrapper");
                var main = wrapper.CssSelect("section#main");
                var box = main.CssSelect("div.box").ToList()[1];
                var slider = box.CssSelect("div.slider");
                var navTab = slider.CssSelect("div.navTab");
                var days = navTab.CssSelect("div.navDays").ToList()[1];
                var child = days.ChildNodes.Skip(1).ToList()[0];
                var child1 = child.ChildNodes.Skip(3).ToList()[0].GetAttributeValue("src");

                pictureBox2.Load(child1);

            }

            {
                WebPage webpage = await browser.NavigateToPageAsync(new Uri(url));

                var wrapper = webpage.Html.OwnerDocument.DocumentNode.CssSelect("div#wrapper");
                var main = wrapper.CssSelect("section#main");
                var box = main.CssSelect("div.box").ToList()[1];
                var slider = box.CssSelect("div.slider");
                var navTab = slider.CssSelect("div.navTab");
                var days = navTab.CssSelect("div.navDays").ToList()[1];
                var child = days.ChildNodes.Skip(1).ToList();
               var child1 = child[0].ChildNodes.Skip(2).ToList()[3];
                var min = child1.CssSelect("span.switchcelsius").ToList()[0] ;
                var max = child1.CssSelect("span.switchcelsius").ToList()[1];



                label2.Text = $"{min.InnerText} °C  {max.InnerText} °C";
                

            }

         }
    
    }
}

