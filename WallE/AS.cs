using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WallE
{
    public class AS : SearchMethod
    {
        private List<GridState> _queue;


        /// <summary>
        /// Constructor 
        /// </summary>
        public AS()
        {
            _queue = new List<GridState>(); //The frontier
            _serachName = "A*";
            _noOfNodes = 0;
        }

        /// <summary>
        /// Add nodes to the frontier 
        /// </summary>
        /// <param name="aState"></param>
        protected override void AddToFrontier(GridState aState)
        {
            if ((aState.HeursiticValue != -1) && (aState.EvaluationFunction != -1)) //Add to Frontier only if h value and f value have been calculated 
            {
                _queue.Add(aState);
                _noOfNodes++;

            }
        }

        /// <summary>
        /// Pop node from the frontier (Returns the node with the minimum f value) 
        /// </summary>
        /// <returns></returns>
        protected override GridState PopFrontier()
        {
            int minEValue = _queue.Min(p => p.EvaluationFunction); 


            //for (int i = _queue.Count() - 1; i >= 0; i--) // return the node that was added to the frotier last when f values are equal
            for(int i = 0; i < _queue.Count(); i ++) //retun the node that was added to the frontier first when f values are equal
            {
                if (_queue[i].EvaluationFunction == minEValue)
                {
                    GridState poped = _queue[i];
                    _queue.Remove(_queue[i]);
                    return poped;

                }
            }
            return null;

        }

        /// <summary>
        /// Calculate the evaluation f (= g + h) value
        /// </summary>
        /// <param name="aState"></param>
        /// <param name="goalState"></param>
        private void CalculateEvaluationFunction(GridState aState, GridState goalState)
        {
            aState.HeursiticValue = Math.Abs(aState.RobotCell.Item1 - goalState.RobotCell.Item1) + Math.Abs(aState.RobotCell.Item2 - goalState.RobotCell.Item2); //Manhattan distance
            aState.EvaluationFunction = aState.HeursiticValue + aState.Cost; // f = h + g

        }

        /// <summary>
        /// Find goal state starting from the initial state
        /// </summary>
        /// <param name="initial"></param>
        /// <param name="goal"></param>
        /// <returns></returns>
        public override Direction[] Solve(GridState initial, GridState goal)
        {
            CalculateEvaluationFunction(initial, goal);
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
                        CalculateEvaluationFunction(newStates[i], goal);
                        AddToFrontier(newStates[i]);
                    }
                }

            }

            Direction[] result = { Direction.None }; //no result found 
            return result;

        }


    }
}
