using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApplication3.Models;
using System.ComponentModel;
using System.Timers;
using System.Windows.Threading;
using System.Net.Http;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Threading;

namespace WpfApplication3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DateTime timeStamps;
        TextBox txtUrl = new TextBox();
        TextBlock txtblosk = new TextBlock();
        public readonly string urls = "";
        string url = "http://www.starsports.com/syndicationdata/livematches/livematches.json?callback=JsonData&_=";
        public MainWindow()
        {
            //1426757025314            //1426741999773            //1426822299379            //1426838784318
            InitializeComponent();
            txtUrl.Text = "Enter url here";
            txtUrl.HorizontalAlignment = HorizontalAlignment.Stretch;
            txtUrl.VerticalAlignment = VerticalAlignment.Bottom;
            txtUrl.MouseLeave += txtUrl_MouseLeave;
            txtUrl.MouseDown += txtUrl_MouseDown;
            txtUrl.Background = new SolidColorBrush(Color.FromRgb(255, 192, 203));
            mainGrid.Children.Add(txtUrl);
            //InitTimer();

        }

        void txtUrl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtUrl.Text = "";
        }

        void txtUrl_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUrl.Text))
            {
                
                txtblosk.Text = "Please wait...";
                txtblosk.HorizontalAlignment = HorizontalAlignment.Stretch;
                txtblosk.VerticalAlignment = VerticalAlignment.Bottom;
                txtblosk.Background = new SolidColorBrush(Color.FromRgb(255, 192, 203));
                mainGrid.Children.Add(txtblosk);
                double n;
                bool IsIntOrNot = false;
                IsIntOrNot = Double.TryParse(txtUrl.Text, out n);
                if (IsIntOrNot)
                {
                    url = url + txtUrl.Text;
                    txtUrl.IsReadOnly = true;
                    txtUrl.IsEnabled = false;
                    DispatcherTimer dispatcherTimer = new DispatcherTimer();
                    dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                    dispatcherTimer.Interval = new TimeSpan(0, 0, 10);
                    dispatcherTimer.Start();
                }
                else
                {
                    MessageBox.Show("Please enter a valid 13 digit numeric key!");
                }
            }
            
            else
            {
                MessageBox.Show("Please enter a valid numeric key!");
                txtUrl.Focus();
            }
        }

        public void myWorker_DoWork()
        {
            try
            {
                RootObject allIssues = null;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                String ver = response.ProtocolVersion.ToString();
                string str = "";
                StreamReader reader;
                using (reader = new StreamReader(response.GetResponseStream()))
                {
                    str = reader.ReadToEnd();
                }
                if(str != null)
                {
                    txtUrl.IsReadOnly = true;
                    if (str.Contains("({") && str.Contains("})"))
                    {
                        str = str.Replace("({", "[{");
                        str = str.Replace("})", "}]");
                    }
                    if (str.Contains("JsonData"))
                    {
                        str = str.Replace("JsonData", "\"JsonData\":");
                        str = str.Insert(0, "{");
                        int s = str.Length;
                        str = str.Insert(s, "}");
                    }
                    allIssues = JsonConvert.DeserializeObject<RootObject>(str);
                    
                    if (allIssues.JsonData[0].cricketMatches != null && allIssues.JsonData[0].cricketMatches.Count != 0)
                    {
                        if (allIssues.JsonData[0].cricketMatches[0].Batsmen.Count == 2)
                        {
                            lblBatsman1Name.Text = "1st Batsman: " + allIssues.JsonData[0].cricketMatches[0].Batsmen[0].BatsmanName + ">> ";
                            lblSaparate1Runs.Text = "Runs: " + allIssues.JsonData[0].cricketMatches[0].Batsmen[0].Runs + ">> ";
                            lblSaparate1Balls.Text = "Balls faced: " + allIssues.JsonData[0].cricketMatches[0].Batsmen[0].BallsFaced + ">> ";

                            lblBatsman2Name.Text = "2nd Batsman: " + allIssues.JsonData[0].cricketMatches[0].Batsmen[1].BatsmanName + ">> ";
                            lblSaparate2Runs.Text = "Runs: " + allIssues.JsonData[0].cricketMatches[0].Batsmen[1].Runs + ">> ";
                            lblSaparate2Balls.Text = "Balls faced: " + allIssues.JsonData[0].cricketMatches[0].Batsmen[1].BallsFaced + ">> ";
                        }
                        else
                        {
                            lblBatsman1Name.Text = "1st Batsman: " + allIssues.JsonData[0].cricketMatches[0].Batsmen[0].BatsmanName + ">> ";
                            lblSaparate1Runs.Text = "Runs: " + allIssues.JsonData[0].cricketMatches[0].Batsmen[0].Runs + ">> ";
                            lblSaparate1Balls.Text = "Balls faced: " + allIssues.JsonData[0].cricketMatches[0].Batsmen[0].BallsFaced + ">> ";
                        }
                        lblTeamA.Text = allIssues.JsonData[0].cricketMatches[0].TeamA;
                        lblTeamB.Text = allIssues.JsonData[0].cricketMatches[0].TeamB;

                        lblBowler1Name.Text = "Bowler Name>> " + allIssues.JsonData[0].cricketMatches[0].Bowlers[0].BowlerName + ">> ";
                        lblBowler1Overs.Text = "Overs>> " + allIssues.JsonData[0].cricketMatches[0].Bowlers[0].Overs + ">> ";
                        lblBowler1RunsGiven.Text = "Runs given>> " + allIssues.JsonData[0].cricketMatches[0].Bowlers[0].RunsGiven + ">> ";
                        lblBowler1WicketTaken.Text = "Wickets taken>> " + allIssues.JsonData[0].cricketMatches[0].Bowlers[0].Wickets + ">> ";

                        lblBowler2Name.Text = "Bowler Name>> " + allIssues.JsonData[0].cricketMatches[0].Bowlers[1].BowlerName + ">> ";
                        lblBowler2Overs.Text = "Overs>> " + allIssues.JsonData[0].cricketMatches[0].Bowlers[1].Overs + ">> ";
                        lblBowler3RunsGiven.Text = "Runs given>> " + allIssues.JsonData[0].cricketMatches[0].Bowlers[1].RunsGiven + ">> ";
                        lblBowler4WicketTaken.Text = "Wickets taken>> " + allIssues.JsonData[0].cricketMatches[0].Bowlers[1].Wickets + ">> ";


                        lblEditorSatus.Text = allIssues.JsonData[0].cricketMatches[0].EditorialText;
                        lblMatchStatusCate.Text = allIssues.JsonData[0].cricketMatches[0].MatchStatus;
                        lblTimeStampStatus.Text = allIssues.JsonData[0].metaData.Timestamp;
                        timeStamps = Convert.ToDateTime(allIssues.JsonData[0].metaData.Timestamp);
                        
                        Title ="Welcome ICC World Cup 2015, Today "+ allIssues.JsonData[0].cricketMatches[0].TeamA + " vs " + allIssues.JsonData[0].cricketMatches[0].TeamB;
                        if (allIssues.JsonData[0].cricketMatches[0].TeamAId == allIssues.JsonData[0].cricketMatches[0].WinningTeamId)
                        {
                            lblWinningTeamName.Text = allIssues.JsonData[0].cricketMatches[0].TeamA;
                        }
                        if (allIssues.JsonData[0].cricketMatches[0].TeamBId == allIssues.JsonData[0].cricketMatches[0].WinningTeamId)
                        {
                            lblWinningTeamName.Text = allIssues.JsonData[0].cricketMatches[0].TeamB;
                        }
                        if (allIssues.JsonData[0].cricketMatches[0].BattingTeamId == allIssues.JsonData[0].cricketMatches[0].TeamAId)
                        {
                            lblStatusA.Text = "Batting";
                            lblStatusB.Text = "Bowling";
                            lblTotalRunsStatus.Text = "Total Runs: " + allIssues.JsonData[0].cricketMatches[0].innings[1].TotalRuns + "/";
                            lblTotalWicketStatus.Text = allIssues.JsonData[0].cricketMatches[0].innings[1].TotalWickets + ", Overs: ";
                            lblTotalOversStatus.Text = allIssues.JsonData[0].cricketMatches[0].innings[1].Overs;
                        }
                        else if (allIssues.JsonData[0].cricketMatches[0].BattingTeamId == allIssues.JsonData[0].cricketMatches[0].TeamBId)
                        {
                            lblStatusA.Text = "Bowling";
                            lblStatusB.Text = "Batting";
                            lblTotalRunsBStatus.Text = "Total Runs: " + allIssues.JsonData[0].cricketMatches[0].innings[0].TotalRuns + "/";
                            lblTotalWicketBStatus.Text = allIssues.JsonData[0].cricketMatches[0].innings[0].TotalWickets + ", Overs: ";
                            lblTotalOversBStatus.Text = allIssues.JsonData[0].cricketMatches[0].innings[0].Overs;
                        }
                        if (allIssues.JsonData[0].cricketMatches[0].BattingFirstTeamId == allIssues.JsonData[0].cricketMatches[0].TeamBId)
                        {
                            lblTotalRunsBStatus.Text = "Total Runs: " + allIssues.JsonData[0].cricketMatches[0].innings[0].TotalRuns + "/";
                            lblTotalWicketBStatus.Text = allIssues.JsonData[0].cricketMatches[0].innings[0].TotalWickets + ", Overs: ";
                            lblTotalOversBStatus.Text = allIssues.JsonData[0].cricketMatches[0].innings[0].Overs;
                        }
                        else if (allIssues.JsonData[0].cricketMatches[0].BattingFirstTeamId == allIssues.JsonData[0].cricketMatches[0].TeamAId)
                        {
                            lblTotalRunsBStatus.Text = "Total Runs: " + allIssues.JsonData[0].cricketMatches[0].innings[0].TotalRuns + "/";
                            lblTotalWicketBStatus.Text = allIssues.JsonData[0].cricketMatches[0].innings[0].TotalWickets + ", Overs: ";
                            lblTotalOversBStatus.Text = allIssues.JsonData[0].cricketMatches[0].innings[0].Overs;
                        }
                        txtblosk.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        MessageBox.Show("There is no any cricket match score is visible on Star Sports today.!");
                        lblWinningTeamName.Text = "There is no any match today!.";
                    }
                   
                }//1426757025314
            }
            catch (HttpRequestException hre)
            {
                MessageBox.Show(hre.Message.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            myWorker_DoWork();
        }

    }
}
