using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WindowsFormsApplication4
{
    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();
        }

        public List<Data> Parse()
        {
            string url = "";
            url = "https://iothook.com/api/v1.2/datas/?data=all"; // for all data

            var webRequest = (HttpWebRequest)WebRequest.Create(url);

            webRequest.Method = "GET";
            webRequest.ContentType = "application/json";
            webRequest.UserAgent = "Mozilla/5.0 (Windows NT 5.1; rv:28.0) Gecko/20100101 Firefox/28.0";
            webRequest.ContentLength = 0;

            string autorization = "erencylk53" + ":" + "19971953";
            byte[] binaryAuthorization = System.Text.Encoding.UTF8.GetBytes(autorization);
            autorization = Convert.ToBase64String(binaryAuthorization);
            autorization = "Basic " + autorization;
            webRequest.Headers.Add("AUTHORIZATION", autorization);

            var webResponse = (HttpWebResponse)webRequest.GetResponse();

            if (webResponse.StatusCode != HttpStatusCode.OK)
                Console.WriteLine(webResponse.Headers.ToString());

            dynamic array;
            List<String> baseData = new List<string>();
            using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
            {
                string json = reader.ReadToEnd();
                List<Item> items = JsonConvert.DeserializeObject<List<Item>>(json);
                array = JsonConvert.DeserializeObject(json);
                foreach (var item in array)
                {
                    baseData.Add(Convert.ToString(item.value_1));
                }
            }
            List<Data> data = new List<Data>();
            for (int i=(baseData.Count)-1; i>=0; i--)
            {
                data.Add(new Data
                {
                    id = Convert.ToString(baseData[i]).Substring(0, 1),
                    day = Convert.ToString(baseData[i]).Substring(2, 2),
                    month = Convert.ToString(baseData[i]).Substring(5, 2),
                    year = Convert.ToString(baseData[i]).Substring(8, 4),
                    hour = Convert.ToString(baseData[i]).Substring(12, 2),
                    minute = Convert.ToString(baseData[i]).Substring(15, 2),
                    second = Convert.ToString(baseData[i]).Substring(18, 2)
                });
            }
            
            return data;
        }

        public void BarExample()
        {
            int monday = 0;
            int tuesday = 0;
            int wednesday = 0;
            int thursday = 0;
            int friday = 0;
            int saturday = 0;
            int sunday = 0;
            int sayac = 0;
            bool IsMonday = false;
            bool IsTuesday = false;
            bool IsWednesday = false;
            bool IsThursday = false;
            bool IsFriday = false;
            bool IsSaturday = false;
            bool IsSunday =false;

            
            this.chartControl.Titles.Clear();
            this.chartControl.Series.Clear();

            // Data arrays
            string[] seriesArray = { "Pazartesi", "Salı", "Çarşamba", "Perşembe", "Cuma", "Cumartesi", "Pazar" };
            List<Data> data = Parse();
            DateTime firsDateTime = new DateTime(Convert.ToInt32(data[data.Count-1].year), Convert.ToInt32(data[data.Count-1].month), Convert.ToInt32(data[data.Count-1].day));
            foreach (var item in data)
            {
                if (item.id==textBox1.Text)
                {
                    
                
                DateTime dt = new DateTime(Convert.ToInt32(item.year), Convert.ToInt32(item.month), Convert.ToInt32(item.day));
                if(dt<firsDateTime.AddDays(-7))
                    break;
                if (sayac > 7)
                {
                    monday = 0;
                    tuesday = 0;
                    wednesday = 0;
                    thursday = 0;
                    friday = 0;
                    saturday = 0;
                    sunday = 0;
                    sayac = 0;
                    IsMonday = false;
                    IsTuesday = false;
                    IsWednesday = false;
                    IsThursday = false;
                    IsFriday = false;
                    IsSaturday = false;
                    IsSunday = false;
                }
                switch (dt.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        if (!IsMonday)
                            sayac++;
                        IsMonday = true;
                        monday++;
                        break;
                    case DayOfWeek.Tuesday:
                        if (!IsTuesday)
                            sayac++;
                        IsTuesday = true;
                        tuesday++;
                        break;
                    case DayOfWeek.Wednesday:
                        if (!IsWednesday)
                            sayac++;
                        IsWednesday = true;
                        wednesday++;
                        break;
                    case DayOfWeek.Thursday:
                        if (!IsThursday)
                            sayac++;
                        IsThursday = true;
                        thursday++;
                        break;
                    case DayOfWeek.Friday:
                        if (!IsFriday)
                            sayac++;
                        IsFriday = true;
                        friday++;
                        break;
                    case DayOfWeek.Saturday:
                        if (!IsSaturday)
                            sayac++;
                        IsSaturday = true;
                        saturday++;
                        break;
                    case DayOfWeek.Sunday:
                        if (!IsSunday)
                            sayac++;
                        IsSunday = true;
                        sunday++;
                        break;
                }
                }
            }

            
            int[] pointsArray = { monday, tuesday, wednesday,thursday,friday,saturday,sunday };
            

            // Set palette
            this.chartControl.Palette = ChartColorPalette.EarthTones;

            // Set title
            this.chartControl.Titles.Add("Su Grafiği");

            
            

            // Add series.
            for (int i = 0; i < seriesArray.Length; i++)
            {
                Series series = this.chartControl.Series.Add(seriesArray[i]);
                series.Points.Add(pointsArray[i]);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Parse();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            chartControl.ResetAutoValues();
            BarExample();
        }
    }
}
