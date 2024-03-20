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

            string outputFilePath = "resized_image.jpg";

            WebClient client = new WebClient();
            byte[] imageData = client.DownloadData(img1);


            using (var imageStream = new System.IO.MemoryStream(imageData))
            {
                using (var resultStream = new System.IO.MemoryStream())
                {

                    int newWidth = 400;
                    int newHeight = 400;


                    ImageBuilder.Current.Build(
                        new ImageJob(
                            imageStream,
                            resultStream,
                            new Instructions($"width={newWidth}&height={newHeight}&mode=max"),
                            false,
                            true));


                    System.IO.File.WriteAllBytes(outputFilePath, resultStream.ToArray());
                }
            }

            pictureBox1.Image = Image.FromFile(outputFilePath);
        }
      








    }
}
