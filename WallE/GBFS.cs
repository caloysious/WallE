using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WallE
{
    public class GBFS : SearchMethod
    {
        private List<GridState> _queue;

        /// <summary>
        /// Constructor
        /// </summary>
        public GBFS()
        {
            _queue = new List<GridState>(); //The frontier
            _serachName = "GBFS";
            _noOfNodes = 0;

        }

        /// <summary>
        /// Add nodes to the frontier 
        /// </summary>
        /// <param name="aState"></param>
        protected override void AddToFrontier(GridState aState)
        {
            if (aState.HeursiticValue != -1) //Add to Fromtier only if h value has been calculated 
            {
                _queue.Add(aState);
                _noOfNodes++;
            }
        }

        /// <summary>
        /// Pop node from the frontier (Returns the node with the mimum h value)
        /// </summary>
        /// <returns></returns>
        protected override GridState PopFrontier()
        {
            int minHValue = _queue.Min(p => p.HeursiticValue);

            for (int i = _queue.Count() - 1; i >= 0; i--) // return the node that was added to the frontier last when h values are equal
            //for(int i = 0; i < _queue.Count(); i ++) //return the node that was added to the frontier first when f values are equal
            {
                if (_queue[i].HeursiticValue == minHValue)
                {
                    GridState poped = _queue[i];
                    _queue.Remove(_queue[i]);
                    return poped;

                }
            }
            return null;
        }

        /// <summary>
        /// Calculate the h value 
        /// </summary>
        /// <param name="aState"></param>
        /// <param name="goalState"></param>
        private void CalculateHeursiticValue(GridState aState, GridState goalState)
        {
            aState.HeursiticValue = Math.Abs(aState.RobotCell.Item1 - goalState.RobotCell.Item1) + Math.Abs(aState.RobotCell.Item2 - goalState.RobotCell.Item2); //Manhattan distance

        }

        /// <summary>
        /// Find goal state starting from the intial state 
        /// </summary>
        /// <param name="initial"></param>
        /// <param name="goal"></param>
        /// <returns></returns>
        public override Direction[] Solve(GridState initial, GridState goal)
        {
            CalculateHeursiticValue(initial, goal);

            AddToFrontier(initial);
            List<GridState> newStates = new List<GridState>();



            while (_queue.Count() > 0)
            {
                GridState thisState = PopFrontier();
                if (thisState.Equals(goal))
                {
                   return thisState.GetPathToState();
                }
                else
                {
                    newStates = thisState.Explore();

                    for (int i = 0; i < newStates.Count(); i++)
                    {
                        CalculateHeursiticValue(newStates[i], goal);
                        AddToFrontier(newStates[i]);
                    }
                }

            }
            Direction[] result = { Direction.None }; //no result  found 
            return result;

        }

    }
}
