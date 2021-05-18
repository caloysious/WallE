using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace WallE
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = args[0];
            string searchMethodName = args[1];

            ReadFile file = new ReadFile(fileName); //args[0] would be the filename 

            GridState i = new GridState(file.GridDimensions, file.Robot, file.Walls);
            GridState g = new GridState(file.GridDimensions, file.Goal, file.Walls);


            List<SearchMethod> searchMethods = new List<SearchMethod>();
            searchMethods.Add(new DFS());
            searchMethods.Add(new BFS());
            searchMethods.Add(new GBFS());
            searchMethods.Add(new AS());
            searchMethods.Add(new IDDFS());
            searchMethods.Add(new IDAS());


            foreach (SearchMethod s in searchMethods)
            {
                if (searchMethodName.ToLower() == s.SerachName.ToLower()) //args[1] would be the search method
                {
                    Direction[] solution = s.Solve(i, g);
                    Console.Write("\n" + file.FileName + " " + s.SerachName + " " + s.NoOfNodes + "\n");
                    for (int j = 0; j < solution.Length; j++)
                    {
                        if (j == solution.Length - 1)
                        {
                            Console.WriteLine(solution[j]);

                        }
                        else
                        {
                            Console.Write(solution[j] + "; ");
                        }
                    }

                }

            }
            //Console.ReadKey();
        }
    }
}