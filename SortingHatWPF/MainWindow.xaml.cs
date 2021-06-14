using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SortingHatWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string questionsAnswersText = File.ReadAllText("sortingquestions.txt");
        static string descriptionsText = File.ReadAllText("descriptions.txt");

        static List<QuestionAnswer> questionsAnswers = new List<QuestionAnswer>();
        static List<Description> descriptionsHouses = new List<Description>();

        static int indexQA;

        static bool isOptionSelected;

        static int gryffindorCount;
        static int hufflepuffCount;
        static int ravenclawCount;
        static int slytherinCount;

        public MainWindow()
        {
            InitializeComponent();
            
            maingrid.Background = new ImageBrush(new BitmapImage(new Uri(System.IO.Path.GetFullPath("Images/sortinghatbg.jpg"))));
            GetQuestionsAnswers();
            GetDescriptions();

            startbutton.Tag = "Visible";
        }

        private void GetQuestionsAnswers()
        {
            List<string> questions = questionsAnswersText.Split('*').ToList();
            for (int i = 0; i < questions.Count(); i++)
            {
                string question = questions[i].Split('#')[0].Trim();
                List<string> answers = questions[i].Split('#')[1].Split('/').ToList().Select(x => x.Trim()).ToList();
                QuestionAnswer qA = new QuestionAnswer(question, answers);
                questionsAnswers.Add(qA);
            }
        }

        private void GetDescriptions()
        {
            List<string> questions = descriptionsText.Split('*').ToList();
            for (int i = 0; i < questions.Count; i++)
            {
                string descHousesString = questions[i].Split(':')[0];
                List<string> descHouses = new List<string>();
                if (descHousesString.Contains("-"))
                {
                    descHouses = descHousesString.Split('-').OrderBy(x => x).Select(x => x.Trim()).ToList();
                }
                else
                {
                    descHouses.Add(descHousesString.Trim());
                }
                string descText = questions[i].Split(':')[1];
                Description newDescription = new Description(descHouses, descText);
                descriptionsHouses.Add(newDescription);
            }
        }

        private void StartGame(object sender, RoutedEventArgs e)
        {
            gryffindorCount = 0;
            hufflepuffCount = 0;
            ravenclawCount = 0;
            slytherinCount = 0;
            indexQA = 0;

            mainTb.Tag = "Hidden";
            secondaryTb.Tag = "Hidden";
            resultsTb.Tag = "Hidden";
            resultHousesTb.Tag = "Hidden";

            questionTb.Tag = "Starter";
            questionsGrid.Tag = "Visible";

            startbutton.Tag = null;
            nextbutton.Tag = "Visible";

            maingrid.DataContext = questionsAnswers[indexQA];
        }

        private void NextQuestion(object sender, RoutedEventArgs e)
        {
            indexQA++;

            CountPoints();

            if (indexQA < questionsAnswers.Count)
            {
                if (isOptionSelected)
                {
                    maingrid.DataContext = questionsAnswers[indexQA];

                    gryffindorChoice.IsChecked = false;
                    hufflepuffChoice.IsChecked = false;
                    ravenclawChoice.IsChecked = false;
                    slytherinChoice.IsChecked = false;

                    isOptionSelected = false;

                    if (indexQA == questionsAnswers.Count - 1)
                    {
                        nextbutton.Tag = null;
                        finishbutton.Tag = "Visible";
                    }
                }
                else
                {
                    MessageBox.Show("Please, choose an option!");
                }
            }
            else
            {
                EndGame();
            }
        }

        private void CountPoints()
        {
            if(gryffindorChoice.IsChecked == true)
            {
                gryffindorCount++;
            }
            else if(hufflepuffChoice.IsChecked == true)
            {
                hufflepuffCount++;
            }
            else if (ravenclawChoice.IsChecked == true)
            {
                ravenclawCount++;
            }
            else
            {
                slytherinCount++;
            }
        }

        private void IsChecked(object sender, RoutedEventArgs e)
        {
            isOptionSelected = true;
        }

        private void EndGame()
        {
            maingrid.Background = new SolidColorBrush(Colors.Black);

            ChosenHouseWithDescription();

            HousesResultsImagesUpdate();

            finishbutton.Tag = null;
            newgamebutton.Tag = "Visible";
            questionsGrid.Tag = null;
            resultsGrid.Tag = "Endgame";
            questionTb.Tag = "Hidden";
        }

        private void HousesResultsImagesUpdate()
        {
            gryffindorChoice.Tag = "Hidden";
            gryffindorPic.Tag = "Visible";
            gryffindorPic.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("Images/gryffindor.jpg")));
            gryffindorText.Tag = "EndgameGryffindor";
            gryffindorText.Text = "GRYFFINDOR" + CalculeHousePercentage(gryffindorCount);

            hufflepuffChoice.Tag = "Hidden";
            hufflepuffPic.Tag = "Visible";
            hufflepuffPic.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("Images/hufflepuff.jpg")));
            hufflepuffText.Tag = "EndgameHufflepuff";
            hufflepuffText.Text = "HUFFLEPUFF" + CalculeHousePercentage(hufflepuffCount);

            ravenclawChoice.Tag = "Hidden";
            ravenclawPic.Tag = "Visible";
            ravenclawPic.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("Images/ravenclaw.jpg")));
            ravenclawText.Tag = "EndgameRavenclaw";
            ravenclawText.Text = "RAVENCLAW" + CalculeHousePercentage(ravenclawCount);

            slytherinChoice.Tag = "Hidden";
            slytherinPic.Tag = "Visible";
            slytherinPic.Source = new BitmapImage(new Uri(System.IO.Path.GetFullPath("Images/slytherin.jpg")));
            slytherinText.Tag = "EndgameSlytherin";
            slytherinText.Text = "SLYTHERIN" + CalculeHousePercentage(slytherinCount);
        }

        private void ChosenHouseWithDescription()
        {
            List<Tuple<string, int>> housesList = new List<Tuple<string, int>>(){
                new Tuple<string, int>("gryffindor", gryffindorCount),
                new Tuple<string, int>("hufflepuff", hufflepuffCount),
                new Tuple<string, int>("ravenclaw", ravenclawCount),
                new Tuple<string, int>("slytherin", slytherinCount)
            };

            resultHousesTb.Text = HouseResultText(MaxHouses(housesList));
            resultHousesTb.Tag = "Endgame";

            resultsTb.Text = HouseResultDescription(MaxHouses(housesList));
            resultsTb.Tag = "Endgame";
        }

        private string CalculeHousePercentage(int houseCount)
        {
            return " : " + String.Format("{0:P2}", ((double)houseCount / (double)questionsAnswers.Count)) 
                   + " (" + houseCount + " / " + questionsAnswers.Count + ")";
        }

        private string HouseResultText(List<string> maxes)
        {
            if(maxes.Count == 1)
            {
                return "You belong to House " + maxes[0] + " ! ";
            }
            else
            {
                string housesString = "You belong to Houses ";
                for (int i = 0; i < maxes.Count; i++)
                {
                    if(i < maxes.Count-1)
                    {
                        housesString += maxes[i] + " & ";
                    }
                    else
                    {
                        housesString += maxes[i] + " ! ";

                    }
                }

                return housesString;
            }
        }
        
        private List<string> MaxHouses(List<Tuple<string, int>> housesList)
        {
            List<string> maxes = housesList.Where(x => x.Item2 == (housesList.Max(y => y.Item2)))
                                 .Select(x => x.Item1.Trim())
                                 .OrderBy(x=>x)
                                 .ToList();
            return maxes;
        }

        private void ClickHouse(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Window description = new Window();
            description.Height = 500;
            description.Width = 800;
            description.ResizeMode = ResizeMode.NoResize;
            description.Background = new ImageBrush(new BitmapImage(new Uri(System.IO.Path.GetFullPath("Images/" + button.Name + "_Description.png"))));
            description.Show();
        }
    
        private string HouseResultDescription(List<string> maxhouses)
        {
            for (int i = 0; i < descriptionsHouses.Count; i++)
            {
                if(descriptionsHouses[i].Houses.SequenceEqual(maxhouses))
                {
                    return descriptionsHouses[i].DescriptionText;
                }   
            }

            return null;
        }

        private void NewSorting(object sender, RoutedEventArgs e)
        {
            maingrid.Background = new ImageBrush(new BitmapImage(new Uri(System.IO.Path.GetFullPath("Images/sortinghatbg.jpg"))));

            questionsGrid.Tag = null;
            resultsGrid.Tag = null;
            secondaryTb.Tag = null;
            resultsTb.Tag = null;
            mainTb.Tag = null;
            newgamebutton.Tag = null;

            slytherinPic.Tag = null;
            gryffindorPic.Tag = null;
            hufflepuffPic.Tag = null;
            ravenclawPic.Tag = null;

            slytherinChoice.Tag = null;
            gryffindorChoice.Tag = null;
            hufflepuffChoice.Tag = null;
            ravenclawChoice.Tag = null;

            gryffindorText.Tag = null;
            slytherinText.Tag = null;
            hufflepuffText.Tag = null;
            ravenclawText.Tag = null;

            questionTb.Tag = "Hidden";
            startbutton.Tag = "Visible";
            resultsTb.Tag = "Hidden";
            resultHousesTb.Tag = "Hidden";
            startbutton.Click += StartGame;

            gryffindorChoice.IsChecked = false;
            hufflepuffChoice.IsChecked = false;
            ravenclawChoice.IsChecked = false;
            slytherinChoice.IsChecked = false;
        }
    }

    public class QuestionAnswer
    {
        public string Question { get; set; }

        public List<string> Answers { get; set; }

        public QuestionAnswer(string question, List<string> answers)
        {
            Question = question;
            Answers = answers;
        }
    }

    public class Description
    {
        public List<string> Houses { get; set; }

        public string DescriptionText { get; set; }

        public Description(List<string> houses, string descriptiontext)
        {
            Houses = houses;
            DescriptionText = descriptiontext;
        }
    }
}
