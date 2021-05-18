
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WallE
{   
    public class DFS : SearchMethod
    {
        private Stack<GridState> _stack;

        /// <summary>
        /// Constructor 
        /// </summary>
        public DFS()
        {
            _stack = new Stack<GridState>(); //The frontier is a stack implementing LIFO
            _serachName = "DFS";
            _noOfNodes = 0;
        }

        /// <summary>
        /// Add nodes to the frontier 
        /// </summary>
        /// <param name="aState"></param>
        protected override void AddToFrontier(GridState aState)
        {
            _stack.Push(aState);
            _noOfNodes++;
        }

        /// <summary>
        /// Pop node from the fronter (LIFO)
        /// </summary>
        /// <returns></returns>
        protected override GridState PopFrontier()
        {
            GridState thisState = _stack.Pop();
            return thisState;
        }

        /// <summary>
        /// Find goal state starting from the inital state 
        /// </summary>
        /// <param name="initial"></param>
        /// <param name="goal"></param>
        /// <returns></returns>
        public override Direction[] Solve(GridState initial, GridState goal)
        {
            AddToFrontier(initial);

            List<GridState> newStates = new List<GridState>();
            
            while (_stack.Count() > 0)
            {
                GridState thisState = PopFrontier();
                if (thisState.Equals(goal))
                {
                    return thisState.GetPathToState();
                }
                else
                {
                    newStates = thisState.Explore();

                    for (int i = newStates.Count() -1 ; i >= 0; i--)
                    {
                        AddToFrontier(newStates[i]);
                    }
                }
            }
            Direction[] result = { Direction.None}; //no result found
            return result;
        }

    }
    
}
