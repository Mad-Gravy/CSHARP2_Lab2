using System;
using System.Collections.Generic;

namespace Yahtzee
{
    class Program
    {
        const int NONE = -1;
        const int ONES = 0;
        const int TWOS = 1;
        const int THREES = 2;
        const int FOURS = 3;
        const int FIVES = 4;
        const int SIXES = 5;
        const int THREE_OF_A_KIND = 6;
        const int FOUR_OF_A_KIND = 7;
        const int FULL_HOUSE = 8;
        const int SMALL_STRAIGHT = 9;
        const int LARGE_STRAIGHT = 10;
        const int CHANCE = 11;
        const int YAHTZEE = 12;
        const int SUBTOTAL = 13;
        const int BONUS = 14;
        const int TOTAL = 15;

        static Random rng = new Random();
        static void Main(string[] args)
        {

            int[] userScorecard = new int[16];
            int[] compScorecard = new int[16];

            int userTurnCount = 0;
            int compTurnCount = 0;

            bool isUserTurn = false;

            ResetScorecard(userScorecard);
            ResetScorecard(compScorecard);

            Console.SetWindowSize(100, 100);

            // Testers
            //List<int> testDice = new List<int>();
            //Roll(5, testDice);
            //DisplayDice(testDice);

            do
            {
                isUserTurn = !isUserTurn;

                UpdateScorecard(userScorecard);
                UpdateScorecard(compScorecard);
                DisplayScoreCards(userScorecard, compScorecard);

                if (isUserTurn)
                {
                    Console.WriteLine("It is your turn.");
                    Console.ReadLine();
                    UserPlay(ref userScorecard, ref userTurnCount);
                }
                else
                {
                    Console.WriteLine("It is the computer's turn.");
                    ComputerPlay(ref compScorecard, ref compTurnCount);
                }

            } while (userTurnCount <= 13 && compTurnCount <= 13);

            UpdateScorecard(userScorecard);
            UpdateScorecard(compScorecard);
            DisplayScoreCards(userScorecard, compScorecard);

            if (userScorecard[TOTAL] > compScorecard[TOTAL])
            {
                Console.WriteLine("You win!");
            }
            else if (userScorecard[TOTAL] < compScorecard[TOTAL])
            {
                Console.WriteLine("The computer wins!");
            }
            else
            {
                Console.WriteLine("It's a tie!");
            }

            Console.ReadLine();
        }

        #region Scorecard Methods

        // sets all of the items in a scorecard to -1 to start the game
        // takes a data structure for a scorecard and the corresponding score card count as parameters.  Both are altered by the method.
        static void ResetScorecard(int[] scorecard)
        {
            for (int i = 0; i < scorecard.Length; i++)
                scorecard[i] = NONE;
        }

        // calculates the subtotal, bonus and the total for the scorecard
        // takes a data structure for a scorecard as it's parameter
        static void UpdateScorecard(int[] scorecard)
        {
            /* you can uncomment this code once you declare the parameter */
            scorecard[SUBTOTAL] = 0;
            scorecard[BONUS] = 0;
            for (int i = ONES; i <= SIXES; i++)
                if (scorecard[i] != -1)
                    scorecard[SUBTOTAL] += scorecard[i];

            if (scorecard[SUBTOTAL] >= 63)
                scorecard[BONUS] = 35;

            scorecard[TOTAL] = scorecard[SUBTOTAL] + scorecard[BONUS];
            for (int i = THREE_OF_A_KIND; i <= YAHTZEE; i++)
                if (scorecard[i] != -1)
                    scorecard[TOTAL] += scorecard[i];
        }

        static string FormatCell(int value)
        {
            return (value < 0) ? "" : value.ToString();
        }

        // takes the data structure for the user's scorecard and the data structure for the computer's scorecard as parameters
        static void DisplayScoreCards(int[] uScorecard, int[] cScorecard)
        {
            /* you can uncomment this code when you have declared the parameters */
            string[] labels = {"Ones", "Twos", "Threes", "Fours", "Fives", "Sixes",
                                "3 of a Kind", "4 of a Kind", "Full House", "Small Straight", "Large Straight",
                                "Chance", "Yahtzee", "Sub Total", "Bonus", "Total Score"};
            string lineFormat = "| {3,2} {0,-15}|{1,8}|{2,8}|";
            string border = new string('-', 39);

            Console.Clear();
            Console.WriteLine(border);
            Console.WriteLine(String.Format(lineFormat, "", "  You   ", "Computer", ""));
            Console.WriteLine(border);
            for (int i = ONES; i <= SIXES; i++)
            {
                Console.WriteLine(String.Format(lineFormat, labels[i], FormatCell(uScorecard[i]), FormatCell(cScorecard[i]), i));
            }
            Console.WriteLine(border);
            Console.WriteLine(String.Format(lineFormat, labels[SUBTOTAL], FormatCell(uScorecard[SUBTOTAL]), FormatCell(cScorecard[SUBTOTAL]), ""));
            Console.WriteLine(border);
            Console.WriteLine(String.Format(lineFormat, labels[BONUS], FormatCell(uScorecard[BONUS]), FormatCell(cScorecard[BONUS]), ""));
            Console.WriteLine(border);
            for (int i = THREE_OF_A_KIND; i <= YAHTZEE; i++)
            {
                Console.WriteLine(String.Format(lineFormat, labels[i], FormatCell(uScorecard[i]), FormatCell(cScorecard[i]), i));
            }
            Console.WriteLine(border);
            Console.WriteLine(String.Format(lineFormat, labels[TOTAL], FormatCell(uScorecard[TOTAL]), FormatCell(cScorecard[TOTAL]), ""));
            Console.WriteLine(border);
        }
        #endregion

        #region Rolling Methods
        // rolls the specified number of dice and adds them to the data structure for the dice
        // takes an integer that represents the number of dice to roll and a data structure to hold the dice as it's parameters
        static void Roll(int numberOfDice, List<int> dice)
        {
            for (int i = 0; i < numberOfDice; i++)
                dice.Add(rng.Next(1, 7));
        }

        // takes a data structure that is a set of dice as it's parameter.  Call it dice.
        static void DisplayDice(List<int> dice)
        {
            /* you can uncomment this code when you have declared the parameter */
            string lineFormat = "|   {0}  |";
            string border = "*------*";
            string second = "|      |";

            foreach (int d in dice)
                Console.Write(border);
            Console.WriteLine();
            foreach (int d in dice)
                Console.Write(second);
            Console.WriteLine("");
            foreach (int d in dice)
                Console.Write(String.Format(lineFormat, d));
            Console.WriteLine();
            foreach (int d in dice)
                Console.Write(second);
            Console.WriteLine("");
            foreach (int d in dice)
                Console.Write(border);
            Console.WriteLine();
        }
        #endregion

        #region Computer Play Methods

        // figures out the highest possible score for the set of dice for the computer
        // takes the scorecard datastructure and the set of dice that the computer is keeping as it's parameters
        static int GetComputerScorecardItem(int[] scorecard, List<int> keeping)
        {
            int indexOfMax = 0;
            int max = 0;

            /* you can uncomment this code once you've identified the parameters for this method*/
            for (int i = ONES; i <= YAHTZEE; i++)
            {
                if (scorecard[i] == -1)
                {
                    int score = Score(i, keeping);
                    if (score >= max)
                    {
                        max = score;
                        indexOfMax = i;
                    }
                }
            }
            

            return indexOfMax;
        }

        // implements the computer's turn.  The computer only rolls once.
        // You can earn extra credit for making the computer play smarter
        // takes the computer's scorecard data structure and scorecard count as parameters.  Both are altered by the method.
        static void ComputerPlay(ref int[] cScorecard, ref int compTurnCount)
        {
            /* you can uncomment this code once you declare the parameters
            declare a data structure for the dice that the computer is keeping.  Call it keeping.*/
            List<int> keeping = new List<int>();

            int itemIndex = -1;

            Roll(5, keeping);
            Console.WriteLine("The dice the computer rolled: ");
            DisplayDice(keeping);
            Pause();
            Pause();

            itemIndex = GetComputerScorecardItem(cScorecard, keeping);
            cScorecard[itemIndex] = Score(itemIndex, keeping);
            compTurnCount++;
            
        }
        #endregion

        #region User Play Methods

        // moves the dice that the user want to keep from the rolling data structure to the keeping data structure
        // takes the rolling data structure and the keeping data structure as parameters
        static void GetKeeping(List<int> rolling, List<int> keeping)
        {
            Console.WriteLine("Choose which dice to keep by position, 1-5, and your chosen dice will not be rerolled. Enter a zero/0 to stop choosing.");

            while (true)
            {
                Console.Write("Which one will you keep? (Enter a '0' to keep the remaining dice). ");
                int choice = int.Parse(Console.ReadLine());

                if (choice == 0)
                    break;

                keeping.Add(rolling[choice - 1]);
                rolling.RemoveAt(choice - 1);
            }
        }

        // on the last roll moves the dice that the user just rolled into the data structure for the dice that the user is keeping
        static void MoveRollToKeep(List<int> rolling, List<int> keeping)
        {
            // iterate through the rolling data structure and copy each item into the keeping data structure
            foreach (int dice in rolling)
            {
                keeping.Add(dice);
            }

            // clear the rollling data structure
            rolling.Clear();
        }

        // asks the user which item on the scorecard they want to score 
        // must make sure that the scorecard doesn't already have a value for that item
        // remember that the scorecard is initialized with -1 in each item
        // takes a scorecard data structure as it's parameter 
        static int GetScorecardItem(int[] scorecard)
        {
            int choice;

            do
            {
                Console.WriteLine("Scoring Options:");
                Console.WriteLine("|| 1. Ones || 2. Twos || 3. Threes || 4. Fours || 5. Fives || 6. Sixes || 7. Three-of-a-Kind ||");
                Console.WriteLine("|| 8. Four-of-a-Kind || 9. Full House || 10. Small Straight || 11. Large Straight || 12. Chance || 13. Yahtzee ||");
                Console.WriteLine("How would you like to score this hand? (Type the number next to the scoring option.)");

                choice = int.Parse(Console.ReadLine());

                if (choice < 1 || choice > 13)
                {
                    Console.WriteLine("Invalid Entry, please choose a number between 1 and 13.");
                }
                else if (scorecard[choice - 1] != -1)
                {
                    Console.WriteLine("You already used this scoring option. You must choose a scoring option you haven't used yet.");
                }

            } while (choice < 1 || choice > 13 || scorecard[choice - 1] != -1);

            return choice - 1;
        }

        // implments the user's turn
        // takes the user's scorecard data structure and the user's move count as parameters.  Both will be altered by the method.
        static void UserPlay(ref int[] uScorecard, ref int userTurnCount)
        {
            List<int> rolling = new List<int>();
            List<int> keeping = new List<int>();

            int rollCount = 0;
            int scorecardItem = -1;

            do
            {
                Roll(5 - keeping.Count, rolling);
                rollCount++;

                Console.WriteLine($"Here's roll number {rollCount}!");

                DisplayDice(rolling);

                if (rollCount < 3)
                {
                    GetKeeping(rolling, keeping);
                }

                else
                {
                    MoveRollToKeep(rolling, keeping);
                }

                DisplayDice(keeping);
            } while (rollCount < 3 && keeping.Count < 5);

            scorecardItem = GetScorecardItem(uScorecard);

            uScorecard[scorecardItem] = Score(scorecardItem, keeping);
            userTurnCount++;
        }

        #endregion

        #region Scoring Methods

        // counts how many of a specified value are in the set of dice
        // takes the value that you're counting and the data structure containing the set of dice as it's parameter
        // returns the count
        static int Count(int value, List<int> dice)
        {
            int count = 0;

            foreach (int die in dice)
                if (die == value)
                    count++;

            return count;
        }

        // counts the number of ones, twos, ... sixes in the set of dice
        // takes a data structure for a set of dice as it's parameter
        // returns a data structure that contains the count for each dice value
        static int[] GetCounts(List<int> dice)
        {
            int[] counts = new int[6];

            for (int value = 1; value <= 6; value++)
            {
                counts[value - 1] = Count(value, dice);
            }

            return counts;                
        }

        // adds the value of all of the dice based on the counts
        // takes a data structure that represents all of the counts as a parameter
        static int Sum(int[] counts)
        {
            int sum = 0;
            /* you can uncomment this code once you have declared the parameter*/
            for (int i = ONES; i <= SIXES; i++)
            {
                int value = i + 1;
                sum += (value * counts[i]);
            }
            
            return sum;
        }

        // determines if you have a specified count based on the counts
        // takes a data structure that represents all of the counts as a parameter
        static bool HasCount(int howMany, int[] counts)
        {
            /* you can uncomment this when you declare the parameter*/
            foreach (int count in counts)
                if (howMany == count)
                    return true;
            
            return false;
        }

        // chance is the sum of the dice
        // takes a data structure that represents all of the counts as a parameter
        static int ScoreChance(int[] counts)
        {
            return Sum(counts);
        }

        // calculates the score for ONES given the set of counts (from GetCounts)
        // takes a data structure that represents all of the counts as a parameter
        static int ScoreOnes(int[] counts)
        {
            // you can comment out this line when you have declared the parameters
            return counts[ONES] * 1;
        }

        // WRITE ALL OF THESE: ScoreTwos, ScoreThrees, ScoreFours, ScoreFives, ScoreSies
        static int ScoreTwos(int[] counts)
        {
            // you can comment out this line when you have declared the parameters
            return counts[TWOS] * 2;
        }

        static int ScoreThrees(int[] counts)
        {
            return counts[THREES] * 3;
        }

        static int ScoreFours(int[] counts)
        {
            return counts[FOURS] * 4;
        }

        static int ScoreFives(int[] counts)
        {
            return counts[FIVES] * 5;
        }

        static int ScoreSixes(int[] counts)
        {
            return counts[SIXES] * 6;
        }

        // scores 3 of a kind.  4 of a kind or 5 of a kind also can be used for 3 of a kind
        // the sum of the dice are used for the score
        // takes a data structure that represents all of the counts as a parameter
        static int ScoreThreeOfAKind(int[] counts)
        {
            foreach(int count in counts)
            {
                if (count >= 3)
                    return Sum(counts);
            }

            return 0;
        }

        // WRITE ALL OF THESE: ScoreFourOfAKind, ScoreYahtzee - a yahtzee is worth 50 points
        static int ScoreFourOfAKind(int[] counts)
        {
            foreach (int count in counts)
            {
                if (count >= 4)
                    return Sum(counts);
            }

            return 0;
        }

        static int ScoreYahtzee(int[] counts)
        {
            foreach (int count in counts)
            {
                if (count == 5)
                    return 50;
            }

            return 0;
        }

        // takes a data structure that represents all of the counts as a parameter
        static int ScoreFullHouse(int[] counts)
        {
            /* you can uncomment this code once you declare the parameter*/
            if (HasCount(2, counts) && HasCount(3, counts))
                return 25;
            else
            
            return 0;

        }

        // takes a data structure that represents all of the counts as a parameter
        static int ScoreSmallStraight(int[] counts)
        {
            /* you can uncomment this code once you declare the parameter*/
            for (int i = THREES; i <= FOURS; i++)
            {
                if (counts[i] == 0)
                    return 0;
            }
            if ((counts[ONES] >= 1 && counts[TWOS] >= 1) ||
                (counts[TWOS] >= 1 && counts[FIVES] >= 1) ||
                (counts[FIVES] >= 1 && counts[SIXES] >= 1))
                return 30;
            else
            
            return 0;
        }

        // takes a data structure that represents all of the counts as a parameter
        static int ScoreLargeStraight(int[] counts)
        {
            /* you can uncomment this code once you declare the parameter*/
            for (int i = TWOS; i <= FIVES; i++)
            {
                if (counts[i] == 0)
                    return 0;
            }
            if (counts[ONES] == 1 || counts[SIXES] == 1)
                return 40;
            else
            
            return 0;
        }

        // scores a score card item based on the set of dice
        // takes an integer which represent the scorecard item as well as a data structure representing a set of dice as parameters
        static int Score(int whichElement, List<int> dice)
        {
            /* you can uncomment this code once you declare the parameter*/
            int[] counts = GetCounts(dice);
            switch (whichElement)
            {
                case ONES:
                    return ScoreOnes(counts);
                case TWOS:
                    return ScoreTwos(counts);
                case THREES:
                    return ScoreThrees(counts);
                case FOURS:
                    return ScoreFours(counts);
                case FIVES:
                    return ScoreFives(counts);
                case SIXES:
                    return ScoreSixes(counts);
                case THREE_OF_A_KIND:
                    return ScoreThreeOfAKind(counts);
                case FOUR_OF_A_KIND:
                    return ScoreFourOfAKind(counts);
                case FULL_HOUSE:
                    return ScoreFullHouse(counts);
                case SMALL_STRAIGHT:
                    return ScoreSmallStraight(counts);
                case LARGE_STRAIGHT:
                    return ScoreLargeStraight(counts);
                case CHANCE:
                    return ScoreChance(counts);
                case YAHTZEE:
                    return ScoreYahtzee(counts);
                default:
                    return 0;
            }
            
            return 0;
        }

        #endregion

        static void Pause()
        {
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));
        }
    }
}
