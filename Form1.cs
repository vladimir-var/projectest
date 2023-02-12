using LiveCharts;
using LiveCharts.Definitions.Series;
using LiveCharts.Wpf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static projectest.Form1;

namespace projectest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            double lat = (double)numericUpDown1.Value;
            double lon = (double)numericUpDown2.Value;

            string uri = "https://api.openweathermap.org/data/2.5/onecall?lat=" + lat + "&lon=" + lon + "&units=metric&lang=ua&appid=767afa044a6cfd0d5886594427b539f3";

            using (HttpClient client = new HttpClient())
            {
                using (HttpRequestMessage request = new HttpRequestMessage())
                {
                    request.RequestUri = new Uri(uri);
                    request.Method = HttpMethod.Get;

                    HttpResponseMessage message = await client.SendAsync(request).ConfigureAwait(false);
                    string content = await message.Content.ReadAsStringAsync();
                    Root root = JsonConvert.DeserializeObject<Root>(content);
                    Action action = () => Chartch(root);
                    Invoke(action);
                }
            }
        }
        void Chartch(Root root)
        {
            List<string> time = new List<string>();
            List<double> temp = new List<double>();
            List<int> hum = new List<int>();

            time = root.hourly.Select(x => Timeconv(x.dt)).ToList();
            temp = root.hourly.Select(x => x.temp).ToList();
            hum = root.hourly.Select(x => x.humidity).ToList();

            Chartset1(temp, time);
            Chartset2(hum, time);
        }

        String Timeconv(int dt)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(dt).ToLocalTime();
            return dateTime.ToString();
        }
        private void Chartset1(List<double> temperature, List<String> time)
        {
            cartesianChart1.AxisX[0].Labels = new List<string>(time);

            SeriesCollection series = new SeriesCollection();
            var signUpsValues = new ChartValues<double>(temperature);
            LineSeries lineSeries = new LineSeries();
            lineSeries.Title = "Температура=";
            lineSeries.Values = signUpsValues;
            series.Add(lineSeries);

            cartesianChart1.Series = series;    

            

        }

        private void Chartset2(List<int> humidity, List<String> time)
        {
            cartesianChart2.AxisX[0].Labels = new List<string>(time);

            SeriesCollection series = new SeriesCollection();
            var signUpsValues = new ChartValues<int>(humidity);
            LineSeries lineSeries = new LineSeries();
            lineSeries.Title = "Влажность=";
            lineSeries.Values = signUpsValues;
            series.Add(lineSeries) ;
           
            
            cartesianChart2.Series = series;

           
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            cartesianChart1.AxisX.Add(new Axis()
            {
                Separator = new Separator { Step = 12 },
                Title = "Время",

            });
            cartesianChart1.AxisY.Add(new Axis()
            {
                Title = "Температура",
                LabelFormatter = value => $"{value} C"
            });
            cartesianChart2.AxisX.Add(new Axis()
            {
                Separator = new Separator { Step = 12 },
                Title = "Время"
            });
            cartesianChart2.AxisY.Add(new Axis()
            {
                Title = "Влажность",
                LabelFormatter = value => $"{value} %"
            });

            numericUpDown1.Value = 47;
            numericUpDown2.Value = 33;

            button1_Click(sender, e);
        }
       

    }



}







