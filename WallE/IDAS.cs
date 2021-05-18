using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WallE
{
    public class IDAS : SearchMethod
    {
        private int _threshold; //threshold evaluation value
        private Stack<GridState> _stack;
       
        /// <summary>
        /// Construcotr
        /// </summary>
        public IDAS()
        {
            _stack = new Stack<GridState>();
            _threshold = 0;
            _serachName = "CUS2";
            _noOfNodes++;
        }

        /// <summary>
        /// Add nodes to the frontier
        /// </summary>
        /// <param name="aState"></param>
        protected override void AddToFrontier(GridState aState)
        {
            if (aState.HeursiticValue <= _threshold)
            {
                _stack.Push(aState);
                _noOfNodes++;
            }
        }

        /// <summary>
        ///   Pops Nodes from the frontier
        /// </summary>
        /// <returns></returns>
        protected override GridState PopFrontier()
        {
            GridState thisState = _stack.Pop();
            return thisState;
        }

        /// <summary>
        /// Calculate the evaluation f value (g + h)
        /// </summary>
        /// <param name="aState"></param>
        /// <param name="goalState"></param>
        private void CalculateEvaluationFunction(GridState aState, GridState goalState)
        {
            aState.HeursiticValue = Math.Abs(aState.RobotCell.Item1 - goalState.RobotCell.Item1) + Math.Abs(aState.RobotCell.Item2 - goalState.RobotCell.Item2);
            aState.EvaluationFunction = aState.HeursiticValue + aState.Cost;
        }

        /// <summary>
        /// Find goal state starting from the initial state
        /// </summary>
        /// <param name="initial"></param>
        /// <param name="goal"></param>
        /// <returns></returns>
        public override Direction[] Solve(GridState initial, GridState goal)
        {
            bool _goalFound;
            _goalFound = false;
            while (_goalFound != true)
            {
                CalculateEvaluationFunction(initial, goal);
                AddToFrontier(initial);

                List<GridState> newStates = new List<GridState>();

                while (_stack.Count() > 0)
                {
                    GridState thisState = PopFrontier();
                    if (thisState.Equals(goal))
                    {
                        _goalFound = true;
                        return thisState.GetPathToState();
                    }
                    else
                    {
                        newStates = thisState.Explore();

                        for (int i = newStates.Count() - 1; i >= 0; i--)
                        {
                            CalculateEvaluationFunction(initial, goal);
                            AddToFrontier(newStates[i]);
                        }
                    }
                }

                _threshold++;

            }

            Direction[] result = { Direction.None };
            return result;

        }
    }
}
