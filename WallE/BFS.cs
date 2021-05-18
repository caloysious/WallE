using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WallE
{
    public class BFS : SearchMethod
    {
        private Queue<GridState> _queue;

        /// <summary>
        /// Constructor 
        /// </summary>
        public BFS() 
        {
            _queue = new Queue<GridState>(); //The frontier is a queue implementing FIFO
            _serachName = "BFS";
            _noOfNodes = 0;
        }
        
        /// <summary>
        /// Add nodes to the frontier
        /// </summary>
        /// <param name="aState"></param>
        protected override void AddToFrontier(GridState aState)
        {
            _queue.Enqueue(aState);
            _noOfNodes++;
        }

        /// <summary>
        /// Pop node from the frontier (FIFO)
        /// </summary>
        /// <returns></returns>
        protected override GridState PopFrontier()
        {
            GridState thisState = _queue.Dequeue();
            return thisState;
        }

        /// <summary>
        /// Find goal state starting from the inital state
        /// </summary>
        /// <param name="intial"></param>
        /// <param name="goal"></param>
        /// <returns></returns>
        public override Direction[] Solve(GridState intial, GridState goal)
        {

            AddToFrontier(intial);

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
                        AddToFrontier(newStates[i]);
                    }
                }
            }

            Direction[] result = { Direction.None }; // no resut found
            return result;
        }


    }
}