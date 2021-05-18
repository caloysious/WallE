using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WallE
{
    public abstract class SearchMethod
    {
        protected int _noOfNodes;
        protected string _serachName;

        protected abstract void AddToFrontier(GridState aState);
        protected abstract GridState PopFrontier();
        public abstract Direction[] Solve(GridState intial, GridState goal);

        public int NoOfNodes
        {
            get
            {
                return _noOfNodes;
            }
        }

        public string SerachName
        {
            get
            {
                return _serachName;
            }
        }
    }
}
