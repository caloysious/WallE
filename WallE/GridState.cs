using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WallE
{
    public class GridState
    {
        private int[,] _map;
        private Tuple<int, int> _robotCell;
        private GridState _parent;
        private List<GridState> _children;
        private int _cost;
        private int _heursiticValue;
        private int _evaluationFunction;
        private Direction _pathFromParent;

        /// <summary>
        /// Constructor for Child node
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="pathFromParent"></param>
        /// <param name="map"></param>
        public GridState(GridState parent, Direction pathFromParent, int[,] map)
        {
            _parent = parent;
            _pathFromParent = pathFromParent;
            _map = map;
            _cost = parent.Cost + 1;
            _evaluationFunction = -1; //f value yet to be calculated 
            _heursiticValue =-1; //h value yet to be calculated
        }

        /// <summary>
        /// Constructor for Initial node (No parent)
        /// </summary>
        /// <param name="GridDimensions"></param>
        /// <param name="RobotCell"></param>
        /// <param name="Walls"></param>
        public GridState(Tuple<int, int> GridDimensions, Tuple<int, int> RobotCell, List<int[]> Walls)
        {

            _parent = null;
            _pathFromParent = Direction.None;
            _cost = 0;
            _map = CreateMap( GridDimensions, RobotCell, Walls);
            _evaluationFunction = -1; //f value yet to be calculated 
            _heursiticValue = -1; //h value yet to be calculated
        }

        //Creating the map the robot will navigate
        private int[,] CreateMap(Tuple<int, int> GridDimensions, Tuple<int, int> RobotCell, List<int[]> Walls)
        {
            int[,] map = new int[GridDimensions.Item2, GridDimensions.Item1];
            for (int j = 0; j < map.GetLength(1); j++)
            {
                for (int i = 0; i < map.GetLength(0); i++)
                {
                    map[i, j] = 0;
                }
            }

            _robotCell = RobotCell;


            for (int w = 0; w < Walls.Count; w++)
            {
                int wall_x = (Walls[w])[0];
                int wall_y = (Walls[w])[1];
                int wall_w = (Walls[w])[2];
                int wall_h = (Walls[w])[3];
                for (int i = 0; i < wall_w; i++)
                {
                    for (int j = 0; j < wall_h; j++)
                    {
                        map[wall_x + i, wall_y + j] = 3;
                    }
                }
            }

            return map;
        }

        /// <summary>
        /// Gets all possible actions the robot can perform from the current cell
        /// </summary>
        /// <returns></returns>
        private List<Direction> GetPossibleActions()
        {
            
            List<Direction> result = new List<Direction>();
            int x = this.RobotCell.Item1;
            int y = this.RobotCell.Item2;

            if (y < 1)
            {
                //upper bordwer
            }
            else
            {
                if (this.Map[x, y - 1] != 3) 
                {
                    result.Add(Direction.Up); //Can Move Up if there's no wall
                }
            }
            if (x < 1)
            {
                // left border
            }
            else
            {
                if (this.Map[x - 1, y] != 3)
                {
                    result.Add(Direction.Left); //Can Move down if there's no wall
                }
            }
            if (y > this.Map.GetLength(1) - 2)
            {
                //down border
            }
            else
            {
                if (this.Map[x, y + 1] != 3)
                {
                    result.Add(Direction.Down); //Can Move down if there's no wall
                }
            }


            if (x > this.Map.GetLength(0) - 2)
            {
                //right border
            }
            else
            {
                if (this.Map[x + 1, y] != 3)
                {
                    result.Add(Direction.Right); //Can Move right if there's no wall
                }
            }
            return result;
        }

        /// <summary>
        /// Create all possible gridstates from the current state by moving the robot
        /// Moves in the following order if all else is equal : Up, Left, Down, Right
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        private GridState Move(Direction direction)
        {

            GridState result = new GridState(this, direction, (this.Map));

            if (direction == Direction.Up)
            {
                result.RobotCell = new Tuple<int, int>(this.RobotCell.Item1, this.RobotCell.Item2 - 1);

                if (this.IsInfinity(result))
                {
                    return result;
                }
                return null;

            }
            else if (direction == Direction.Left)
            {
                result.RobotCell = new Tuple<int, int>(this.RobotCell.Item1 - 1, this.RobotCell.Item2);

                if (this.IsInfinity(result))
                {
                    return result;
                }
                return null;


            }
            else if (direction == Direction.Down)
            {
                result.RobotCell = new Tuple<int, int>(this.RobotCell.Item1, this.RobotCell.Item2 + 1);

                if (this.IsInfinity(result))
                {
                    return result;
                }
                return null;

            }
            else if (direction == Direction.Right)
            {

                result.RobotCell = new Tuple<int, int>(this.RobotCell.Item1 + 1, this.RobotCell.Item2);

                if (this.IsInfinity(result))
                {
                    return result;
                }
                return null;


            }
            else
            {
                return null;
                //do nothing 
            }
        }

        /// <summary>
        /// Compares two grid states 
        /// </summary>
        /// <param name="gridState"></param>
        /// <returns></returns>  
        public bool Equals(GridState gridState)
        {
            if ((this.RobotCell.Item1 == gridState.RobotCell.Item1) && (this.RobotCell.Item2 == gridState.RobotCell.Item2))
            {
                return true;
            }
            else
            {
                return false;

            }
        }
        
        /// <summary>
        /// Tracks all the way back to the parent state to see to provide infinity loops 
        /// </summary>
        /// <param name="check"></param>
        /// <returns></returns>
        private bool IsInfinity(GridState check)
        {
            bool result;
            if ((this.RobotCell.Item1 == check.RobotCell.Item1) && (this.RobotCell.Item2 == check.RobotCell.Item2))
            {
                result = false;
            }
            else
            {
                if (this.Parent != null)
                {
                    result = this.Parent.IsInfinity(check);
                }
                else
                {
                    if ((this.RobotCell.Item1 == check.RobotCell.Item1) && (this.RobotCell.Item2== check.RobotCell.Item2))
                    {
                        result = false;
                    }
                    else
                    {
                        result = true;
                    }
                }
            }
            return result;
        }
       
        /// <summary>
        /// Adds children states depending on the possible actions that can be made from the current cell
        /// </summary>
        /// <returns></returns>
        public List<GridState> Explore()
        {
            List<Direction> possibleActions = GetPossibleActions();
            _children = new List<GridState>();
            for (int i = 0; i < possibleActions.Count; i++)
            {
                if (Move(possibleActions[i]) != null)
                {
                    _children.Add(Move(possibleActions[i]));

                }
            }
            return _children;
            
        }

        /// <summary>
        /// Tracks all the way back to the parent node and finds the actions taken to end up at the node
        /// </summary>
        /// <returns></returns>
        public Direction[] GetPathToState()
        {
            Direction[] result;

            if (_parent == null)
            {
                result = new Direction[0];
                return result;
            }
            else
            {
                Direction[] pathToParent = _parent.GetPathToState();
                result = new Direction[pathToParent.Length + 1];

                for (int i = 0; i < pathToParent.Length; i++)
                {
                    result[i] = pathToParent[i];
                }
                result[result.Length - 1] = this.PathFromParent;
                return result;
            }
        }

        /// <summary>
        /// Map
        /// </summary>
        public int[,] Map
        {
            get
            {
                return _map;
            }
            set
            {
                _map = value;
            }
        }

        /// <summary>
        /// Parent node of this node
        /// </summary>
        public GridState Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
            }
        }
        
        /// <summary>
        /// Robot location of this node
        /// </summary>
        public Tuple<int, int> RobotCell
        {
            get
            {
                return _robotCell;
            }
            set
            {
                _robotCell = value;
            }
        }

        /// <summary>
        /// Path from parent to this node
        /// </summary>
        public Direction PathFromParent
        {
            get
            {
                return _pathFromParent;
            }
        }

        /// <summary>
        /// Cost 
        /// </summary>
        public int Cost
        {
            get
            {
                return _cost;
            }
        }

        /// <summary>
        /// h value
        /// </summary>
        public int HeursiticValue
        {
            get
            {
                return _heursiticValue;
            }
            set
            {
                _heursiticValue = value;
            }
        }

        /// <summary>
        /// f value
        /// </summary>
        public int EvaluationFunction
        {
            get
            {
                return _evaluationFunction;
            }
            set
            {
                _evaluationFunction = value;
            }
        }
    }
}


