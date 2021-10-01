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
        //public static readonly string[] DIRECTION = { "NORTH", "EAST", "SOUTH", "WEST" };

        public Robot activeRobot;
        public char[] delimiterChars = { ' ', ',', '\t' }; // Used for split commands
        List<Robot> robots = new List<Robot>();

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
        /// if valid create a new Robot instance and set it's property
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
            /* Check facing argument */
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
                robots.Add(new Robot(userInputX, userInputY, placeArgs[3], robots.Count, true));
                activeRobot = robots[0];
            }
            else
            {
                robots.Add(new Robot(userInputX, userInputY, placeArgs[3], robots.Count, false));
            }
        }

        /// <summary>
        /// Print information of how many robots on the table,
        /// current active robot,
        /// and it's position and facing.
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
                string strFacing;   // Store the stringified facing value
                // Convert robot's _facing to string
                switch (activeRobot.GetRobotDirection())
                {
                    case 0:
                        strFacing = "NORTH";
                        break;
                    case 1:
                        strFacing = "EAST";
                        break;
                    case 2:
                        strFacing = "SOUTH";
                        break;
                    case 3:
                        strFacing = "WEST";
                        break;
                    default:
                        strFacing = "UNKNOWN";
                        break;
                }
                // Because robot's id starts from 0, needs to +1 when print output
                Console.WriteLine($"There are {robots.Count} robots on the table. Active robot is Robot {activeRobot.GetRobotId()+1}."); 
                Console.WriteLine($"It's position is {activeRobot.GetRobotXPos()}, {activeRobot.GetRobotYPos()}, facing {strFacing}");
            }
        }

        /// <summary>
        /// Check whether the active robot is valid to move forward. 
        /// </summary>
        /// <returns></returns>
        public bool IsValidMove()
        {
            int facing = activeRobot.GetRobotDirection();
            int xPosition = activeRobot.GetRobotXPos();
            int yPosition = activeRobot.GetRobotYPos();
            switch (facing)
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
        /// Move the robot.
        /// Prevent the robot from falling off the table by 
        /// checking whether the robot is valid to move at first.
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
        /// Check whether there is any robot on the table at first.
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
        public void UseCmdInput()
        {
            Console.WriteLine("Please enter x y position and facing of the robot, type ('q' or 'Q') to quit: ");
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
