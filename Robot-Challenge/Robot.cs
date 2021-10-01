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
        private Direction _direction { get; set; }
        private bool _isActive { get; set; }
        
        #region Constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public Robot()
        {
            _xPosition = 0;
            _yPosition = 0;
            _direction = 0;
            _id = -1;
            _isActive = false;
        }
        /// <summary>
        /// Constructor
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
            ConvertDirection(direction);
            _id = robotId;
            _isActive = isActive;
        }
        #endregion

        /// <summary>
        /// Convert direction from string to enum Direction.
        /// </summary>
        /// <param name="direction"></param>
        private void ConvertDirection(string direction)
        {
            switch (direction)
            {
                case "NORTH":
                    _direction = Direction.North;
                    break;
                case "EAST":
                    _direction = Direction.East;
                    break;
                case "SOUTH":
                    _direction = Direction.South;
                    break;
                case "WEST":
                    _direction = Direction.West;
                    break;
            }
        }

        #region Set methods
        /// <summary>
        /// Set robot movement
        /// </summary>
        public void SetMovement()
        {
            switch (_direction)
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
            _direction = direction == "LEFT" ? (Direction)(((int)_direction + 3) % 4) : (Direction)(((int)_direction + 1) % 4);
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
        public int GetXPos()
        {
            return _xPosition;
        }
        public int GetYPos()
        {
            return _yPosition;
        }

        public int GetDirection()
        {
            return (int)_direction;
        }
        public int GetId()
        {
            return this._id;
        }
        #endregion
    }
}
