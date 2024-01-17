using Newtonsoft.Json;
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
using System.Windows.Shapes;

namespace PROG7312
{
    /// <summary>
    /// Interaction logic for FindingCallNumbersWindow.xaml
    /// </summary>
    public partial class FindingCallNumbersWindow : Window
    {

        Tree tree;
        TreeNode answer;
        int score = 0;

        int highScore = 0;
        public FindingCallNumbersWindow()
        {
            InitializeComponent();

            // Read the data from the file
            string jsonString = File.ReadAllText("../../../callnumbers.json");

            // Load the call numbers into a tree from JSON
            tree = JsonConvert.DeserializeObject<Tree>(jsonString);

            // Start a new game
            startNewGame();

        }


        // Close the entire program if the close button is clicked
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // Go back to the main menu
        private void backButton_click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Hide();
        }

        // Start a new instance of the game
        public void startNewGame()
        {
            error.Text = "";
            answerBlock1.IsEnabled = true;
            answerBlock2.IsEnabled = true;
            answerBlock3.IsEnabled = true;
            answerBlock4.IsEnabled = true;

            // Get a question from a random call number in the tree
            TreeNode question = tree.GetRandomLeafNode();
            questionTextBlock.Text = question.Name;


            answer = tree.getParent(question);
            List<TreeNode> wrongAnswers = GetRandomUniqueValuesNotInList<TreeNode>(tree.Branches, answer, 3);
            wrongAnswers.Add(answer);

            List<Button> textBlocks = new List<Button> { answerBlock1, answerBlock2, answerBlock3, answerBlock4 };
            Random random = new Random();

            foreach (var answerButtons in textBlocks)
            {
                int randomIndex = random.Next(0, wrongAnswers.Count);
                answerButtons.Content = wrongAnswers[randomIndex].CallNumber + " " + wrongAnswers[randomIndex].Name;
                wrongAnswers.RemoveAt(randomIndex);
            }
        }



        // Reset the game state, keeps the high score
        private void restart_Click(object sender, RoutedEventArgs e)
        {
            score = 0;
            scoreTextBlock.Text = "My Score: " + score;

            startNewGame();
        }

        // When the user selects an answer
        private void answer_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if (button.Content.Equals($"{answer.CallNumber} {answer.Name}"))
            {
                score++;
                scoreTextBlock.Text = "My Score: " + score;

                if (score > highScore)
                {
                    highscoreTextBlock.Text = "High Score: " + score;
                }

                startNewGame();
                error.Text = "";
            }
            else
            {
                // Disable buttons and show error text.
                error.Text = $"Oops! The correct solution is {answer.CallNumber} {answer.Name}. Press restart to play again!";
                answerBlock1.IsEnabled = false;
                answerBlock2.IsEnabled = false;
                answerBlock3.IsEnabled = false;
                answerBlock4.IsEnabled = false;
            }


        }


        // Return a count value of element in a list that are not notvalue.
        public List<T> GetRandomUniqueValuesNotInList<T>(List<T> list, T notValue, int count)
        {
            List<T> temporaryList = new List<T>(list);
            temporaryList.Remove(notValue); // Remove the not value T
            Random random = new Random();

            List<T> listOfRandomValues = new List<T>();
            while (listOfRandomValues.Count < count && temporaryList.Count > 0)
            {
                int randomIndex = random.Next(0, temporaryList.Count);
                T randomValue = temporaryList[randomIndex];
                if (!listOfRandomValues.Contains(randomValue))
                {
                    listOfRandomValues.Add(randomValue);
                }
                temporaryList.RemoveAt(randomIndex);
            }

            return listOfRandomValues;
        }
    }


}
