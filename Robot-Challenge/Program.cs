using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Robot_Challenge
{
    class Program
    {
        public const int TABLE_LOWER_LIMIT = 0;
        public const int TABLE_UPPER_LIMIT = 4;

        public Robot activeRobot;
        List<Robot> robots = new List<Robot>();

        public char[] delimiterChars = { ' ', ',', '\t' }; // Used for split commands

        static void Main(string[] args)
        {
            Program p = new Program();
            Console.WriteLine("Please select input method(type 1 or 2 to choose, q or Q to quit): ");
            Console.WriteLine("1. File input \t\t 2. Command line input\n");
            string inputSelect = Console.ReadLine();
            while (inputSelect != "q" && inputSelect != "Q")
            {
                switch (inputSelect)
                {
                    case "1":
                        p.UseFileInput();
                        break;
                    case "2":
                        p.UseCmdInput();
                        break;
                    default:
                        Console.WriteLine("Invalid command...");
                        break;
                }
                Console.WriteLine("Please select input method(type 1 or 2 to choose, q or Q to quit): ");
                Console.WriteLine("1. File input \t\t 2. Command line input\n");
                inputSelect = Console.ReadLine();
            }
        }

        #region Robot Control
        /// <summary>
        /// Place a new robot on the table.
        /// Check inputs, if invalid print error message,
        /// if valid create a new Robot instance and set its property
        /// according to parameter.
        /// </summary>
        /// <param name="placeArgs"></param>
        public void PlaceRobot(string[] placeArgs)
        {
            /* Check the number of arguments.
             * Correct argument shoud be 4 arguments
             * e.g. PLACE 0,3,SOUTH
             */
            if (placeArgs.Count()!=4)
            {
                Console.WriteLine("Invalid number of arguments.");
                return;
            }

            int userInputX, userInputY;
            // Check x, y values from input only contains number
            bool isValidX = int.TryParse(placeArgs[1], out userInputX);
            bool isValidY = int.TryParse(placeArgs[2], out userInputY);

            /* If there is invalid input in x, y or the robot is placed out of table boundary,
             * print error message and cancel robot placement.
             */
            if (!isValidX || !isValidY || userInputX < TABLE_LOWER_LIMIT || userInputX > TABLE_UPPER_LIMIT || userInputY < TABLE_LOWER_LIMIT || userInputY > TABLE_UPPER_LIMIT)
            {
                Console.WriteLine("X Y values must be number between 0 and 4...");
                return;
            }
            /* Check direction argument */
            else if (placeArgs[3]!="NORTH" && placeArgs[3]!="EAST" && placeArgs[3]!="SOUTH" && placeArgs[3]!="WEST")
            {
                Console.WriteLine("Facing must be one of \"NORTH\", \"EAST\", \"SOUTH\", or \"WEST\".");
                return;
            }
            /** When placing first robot, set it to active.
             *  Robots placed later set to inactive
             */
            if (robots.Count == 0)
            {
                robots.Add(new Robot(userInputX, userInputY, placeArgs[3], robots.Count, true));    // use robots.Count as robot id
                activeRobot = robots[0];    // Set first added robot to active.
            }
            else
            {
                robots.Add(new Robot(userInputX, userInputY, placeArgs[3], robots.Count, false));   // use robots.Count as robot id
            }
        }

        /// <summary>
        /// Print information of how many robots on the table,
        /// current active robot,
        /// and its position and direction.
        /// </summary>
        public void ReportRobotStatus()
        {
            // Check whether there is any robot on the table
            if (robots.Count == 0)
            {
                Console.WriteLine("There is no robot on the table.");
                return;
            }
            else
            {
                string strDirection;   // Store the stringified facing value
                // Convert robot's direction to string
                switch (activeRobot.GetDirection())
                {
                    case 0:
                        strDirection = "NORTH";
                        break;
                    case 1:
                        strDirection = "EAST";
                        break;
                    case 2:
                        strDirection = "SOUTH";
                        break;
                    case 3:
                        strDirection = "WEST";
                        break;
                    default:
                        strDirection = "UNKNOWN";
                        break;
                }
                // Because robot's id starts from 0, needs to +1 when print output
                Console.WriteLine($"There are {robots.Count} robots on the table. Active robot is Robot {activeRobot.GetId()+1}."); 
                Console.WriteLine($"It's position is {activeRobot.GetXPos()}, {activeRobot.GetYPos()}, facing {strDirection}");
            }
        }

        /// <summary>
        /// Check whether the active robot is valid to move forward. 
        /// </summary>
        /// <returns></returns>
        public bool IsValidMove()
        {
            int direction = activeRobot.GetDirection();
            int xPosition = activeRobot.GetXPos();
            int yPosition = activeRobot.GetYPos();
            switch (direction)
            {
                case 0 when yPosition == TABLE_UPPER_LIMIT:
                    return false;
                case 1 when xPosition == TABLE_UPPER_LIMIT:
                    return false;
                case 2 when yPosition == TABLE_LOWER_LIMIT:
                    return false;
                case 3 when xPosition == TABLE_LOWER_LIMIT:
                    return false;
                default:
                    return true;
            }
        }

        /// <summary>
        /// Move the robot if movement is valid
        /// </summary>
        public void SetRobotMovement()
        {
            if (robots.Count == 0)
            {
                Console.WriteLine("There is no robot on the table.");
                return;
            }
            if (IsValidMove())
            {
                activeRobot.SetMovement();
            }
        }

        /// <summary>
        /// Rotate robot.
        /// </summary>
        /// <param name="direction"></param>
        public void SetRobotDirection(string direction)
        {
            if(robots.Count == 0)
            {
                Console.WriteLine("There is no robot on the table.");
                return;
            }
            activeRobot.SetRotation(direction);
        }

        /// <summary>
        /// Select a robot to activate it.
        /// </summary>
        /// <param name="selectArgs"></param>
        public void SelectRobot(string[] selectArgs)
        {
            if (selectArgs.Count() != 2)
            {
                Console.WriteLine("Invalid number of arguments.");
                return;
            }
            int selectedRobotId;
            bool isValidRobotId = int.TryParse(selectArgs[1], out selectedRobotId);

            // Wrong type of input
            if (!isValidRobotId)
            {
                Console.WriteLine("Robot id must be integer.");
                return;
            }
            // The selected robot does not exist
            else if (selectedRobotId <= 0 || selectedRobotId > robots.Count)
            {
                Console.WriteLine("Your selected robot doesn't exist, try another one.");
                return;
            }
            else
            {
                activeRobot.SetRobotInactivate();
                activeRobot = robots[selectedRobotId - 1];
                activeRobot.SetRobotActivate();
            }
     
        }
        #endregion

        public void ShowHelp()
        {
            Console.WriteLine("Place a robot:\tPLACE X,Y,FACING");
            Console.WriteLine("Get report:\tREPORT");
            Console.WriteLine("Move robot:\tMOVE");
            Console.WriteLine("Rotate robot:\tLEFT  RIGHT");
            Console.WriteLine("Select robot:\tROBOT <robot id>");
        }
        public void UseCmdInput()
        {
            Console.WriteLine("Please enter x y position and facing of the robot,\n type 'HELP' to list all commands,\n type ('q' or 'Q') to quit: ");
            string[] tokens = Console.ReadLine().Split(delimiterChars);
            
            while (tokens[0] != "q" && tokens[0] != "Q")
            {
                switch (tokens[0])
                {
                    case "PLACE":
                        PlaceRobot(tokens);
                        break;
                    case "REPORT":
                        ReportRobotStatus();
                        break;
                    case "MOVE":
                        SetRobotMovement();
                        break;
                    case "LEFT":
                    case "RIGHT":
                        SetRobotDirection(tokens[0]);
                        break;
                    case "ROBOT":
                        SelectRobot(tokens);
                        break;
                    case "HELP":
                        ShowHelp();
                        break;
                    default:
                        Console.WriteLine("Invalid command...");
                        break;
                }
                Console.WriteLine("Please enter another command: ");
                tokens = Console.ReadLine().Split(delimiterChars);
            }
        }

        /// <summary>
        /// The file path is hard coded, 
        /// need to change it if run 
        /// on another system.
        /// </summary>
        public void UseFileInput()
        {
            string testFilePath = @"D:\C#\Robot-Challenge\Robot-Challenge\test.txt";
            StreamReader testFile = new StreamReader(testFilePath);
            string line;
            string[] tokens;
            while ((line = testFile.ReadLine()) != null)
            {
                tokens = line.Split(delimiterChars);
                switch (tokens[0])
                {
                    case "PLACE":
                        PlaceRobot(tokens);
                        break;
                    case "REPORT":
                        ReportRobotStatus();
                        break;
                    case "MOVE":
                        SetRobotMovement();
                        break;
                    case "LEFT":
                    case "RIGHT":
                        SetRobotDirection(tokens[0]);
                        break;
                    case "ROBOT":
                        SelectRobot(tokens);
                        break;
                    default:
                        Console.WriteLine("Invalid command...");
                        break;
                }
            }
        }
    }
}
