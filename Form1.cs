
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
using System.Net.Http;
using System.Runtime.InteropServices;

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



            // Inizializza il WebDriver di Chrome
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--start-maximized"); // Massimizza la finestra del browser
            IWebDriver driver = new ChromeDriver(options);

            // Naviga verso la pagina web
            driver.Navigate().GoToUrl($"https://www.3bmeteo.com/meteo/{luogo}");



            try
            {
                // Trova il bottone tramite il selettore CSS, XPath o altri metodi di localizzazione
                var button = driver.FindElement(By.CssSelector("div#iubenda-cs-banner"));
                var button1 = button.FindElement(By.CssSelector("div.iubenda-cs-container"));
                var button2 = button1.FindElement(By.CssSelector("div.iubenda-cs-content"));
                var button3 = button2.FindElement(By.CssSelector("div.iubenda-cs-rationale"));
                var button4 = button3.FindElement(By.CssSelector("div.iubenda-cs-opt-group"));
                var button5 = button4.FindElement(By.CssSelector("div.iubenda-cs-opt-group-consent"));
                var button6 = button5.FindElement(By.CssSelector("button.iubenda-cs-accept-btn"));


                // Esegui il clic sul bottone
                button6.Click();

                IWebElement divElement = driver.FindElement(By.XPath("//div[@id='wrapper']")); // Cambia 'tuo_id_div' con l'id effettivo del tuo div
                IWebElement child = divElement.FindElement(By.XPath(".//section[@id='main']")); // Cambia 'tuo_id_div' con l'id effettivo del tuo div
                IWebElement child1 = child.FindElement(By.XPath(".//div[@class='box noMarg']")); // Cambia 'tuo_id_div' con l'id effettivo del tuo div


                // Ora puoi catturare uno screenshot del quinto figlio div
                Screenshot screenshot = ((ITakesScreenshot)child1).GetScreenshot();
                screenshot.SaveAsFile($"div_screenshot{num}.png");
            }

            catch (NoSuchElementException ex)
            {
                Console.WriteLine("Uno dei div non è stato trovato: " + ex.Message);
            }

            finally
            {
                // Chiudi il WebDriver
                driver.Quit();
                pictureBox2.Image = Image.FromFile($"div_screenshot{num}.png");

                num++;
            }
        }
    }
}
