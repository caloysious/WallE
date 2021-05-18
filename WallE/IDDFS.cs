using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WallE
{
    public class IDDFS : SearchMethod
    {
        private int _depthLimit; //depth Limit that changes in every iternation if solutoion not found 
        public Stack<GridState> _stack;

        /// <summary>
        /// Constructor
        /// </summary>
        public IDDFS()
        {
            _depthLimit = 0;
            _serachName = "CUS1";
            _noOfNodes = 0;
            _stack = new Stack<GridState>();
        }

        /// <summary>
        /// Pop nodes from the fronter
        /// </summary>
        /// <returns></returns>
        protected override GridState PopFrontier()
        {
            GridState thisState = _stack.Pop();
            return thisState;
        }

        /// <summary>
        /// Add nodes to frontier
        /// </summary>
        /// <param name="aState"></param>
        protected override void AddToFrontier(GridState aState)
        {
            if (aState.Cost <= _depthLimit)
            {
                _stack.Push(aState);
                _noOfNodes++;
            }
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
                            AddToFrontier(newStates[i]);
                        }
                    }
                }
                _depthLimit++;
            }
            Direction[] result = { Direction.None };
            return result;
        }
    }

}
