using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot_Challenge
{
    class Robot
    {
        public enum Direction
        {
            North = 0,
            East = 1,
            South = 2,
            West = 3
        }
        private int _id { get; set; }
        private int _xPosition { get; set; }
        private int _yPosition { get; set; }
        private Direction _facing { get; set; }
        private bool _isActive { get; set; }

        public static int TotalRobot = 0;      // A counter to store how many robots have been placed

        
        #region Constructor
        /// <summary>
        /// Constructor that takes no arguments
        /// </summary>
        public Robot()
        {
            _xPosition = 0;
            _yPosition = 0;
            _facing = 0;
            _id = -1;
            _isActive = false;
        }
        /// <summary>
        /// Constructor that takes 5 arguments
        /// </summary>
        /// <param name="xPosition"></param>
        /// <param name="yPosition"></param>
        /// <param name="direction"></param>
        /// <param name="robotId"></param>
        /// <param name="isActive"></param>
        public Robot(int xPosition, int yPosition, string direction, int robotId, bool isActive)
        {
            _xPosition = xPosition;
            _yPosition = yPosition;
            SetDirection(direction);
            _id = robotId;
            _isActive = isActive;
        }
        #endregion
        // Convert input direction from string to enum

        #region Set methods

        /// <summary>
        /// Set the robot direction according to input
        /// </summary>
        /// <param name="direction"></param>
        private void SetDirection(string direction)
        {
            switch (direction)
            {
                case "NORTH":
                    _facing = Direction.North;
                    break;
                case "EAST":
                    _facing = Direction.East;
                    break;
                case "SOUTH":
                    _facing = Direction.South;
                    break;
                case "WEST":
                    _facing = Direction.West;
                    break;
            }
        }

        /// <summary>
        /// Check whether the movement is validate.
        /// </summary>
        /// <returns></returns>
       
        public void SetMovement()
        {
            switch (_facing)
            {
                case Direction.North:
                    _yPosition++;
                    break;
                case Direction.East:
                    _xPosition++;
                    break;
                case Direction.South:
                    _yPosition--;
                    break;
                case Direction.West:
                    _xPosition--;
                    break;
            }
        }

        /// <summary>
        /// Handle robot rotation
        /// Takes an input string parameter "direction"
        /// If "left" turn left 90 degrees, otherwise turn right 90 degrees
        /// </summary>
        /// <param name="direction"></param>
        public void SetRotation(string direction)
        {
            _facing = direction == "LEFT" ? (Direction)(((int)_facing + 3) % 4) : (Direction)(((int)_facing + 1) % 4);
        }

        /// <summary>
        /// Set current robot to active
        /// </summary>
        public void SetRobotActivate()
        {
            _isActive = true;
        }

        /// <summary>
        /// Set current robot to inactive
        /// </summary>
        public void SetRobotInactivate()
        {
            _isActive = false;
        }
        #endregion

        #region Get Methods
        public int GetRobotXPos()
        {
            return _xPosition;
        }
        public int GetRobotYPos()
        {
            return _yPosition;
        }

        public int GetRobotDirection()
        {
            return (int)_facing;
        }
        public int GetRobotId()
        {
            return this._id;
        }
        #endregion
    }
}
