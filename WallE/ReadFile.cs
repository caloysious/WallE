using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WallE
{
    public class ReadFile
    {
        private string _filePath;
        private string _fileName;
        private Tuple<int, int> _gridDimensions;
        private Tuple<int, int> _robot;
        private Tuple<int, int> _goal;
        private List<int[]> _walls;

        public ReadFile(string fileName)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            _fileName = fileName;
            _filePath = currentDirectory + "\\" + _fileName ;
            _walls = new List<int[]>();
            ReadGrid();
        }

        private Tuple<int, int> ClosestGoal (List<Tuple<int, int>> goals)
        {
            Tuple<int, int> closest = goals[0];
            int distance = Math.Abs(this.Robot.Item1 - goals[0].Item1) + Math.Abs(this.Robot.Item2 - goals[0].Item2);

            for (int i = 0; i < goals.Count(); i++)
            {
                if (Math.Abs(this.Robot.Item1 - goals[i].Item1) + Math.Abs(this.Robot.Item2 - goals[i].Item2) <= distance)
                {
                    distance = Math.Abs(this.Robot.Item1 - goals[i].Item1) + Math.Abs(this.Robot.Item2 - goals[i].Item2);
                    closest = goals[i];
                }

            }

            return closest;

        }

        private void ReadGrid()
        {
            List<int[]> data = new List<int[]>();
            char[] trim = new char[] { '[', ']', '(', ')', ',', '|', ' ' };
            int noOfGoals = 1 ;
            StreamReader file = new StreamReader(_filePath);

            while (!file.EndOfStream)
            {
                string[] line = file.ReadLine().Split(trim, StringSplitOptions.RemoveEmptyEntries);
                data.Add(Array.ConvertAll(line, s => int.Parse(s)));
            }
            file.Close();

            //create the grid 
            _gridDimensions = Tuple.Create<int, int>((data[0])[0], (data[0])[1]);

            //initial position of robot 
            _robot = Tuple.Create<int, int>((data[1])[0], (data[1])[1]);

            //goal cells 
            string goalLine = File.ReadLines(_filePath).Skip(2).Take(1).First();

            noOfGoals = goalLine.Count(f => f == '|') + 1;


            List<Tuple<int, int>> goals = new List<Tuple<int, int>>();

            for (int i = 0; i < noOfGoals; i++)
            {
                goals.Add(Tuple.Create<int, int>((data[2])[i*2], (data[2])[(i*2)+1]));
            }

            //find the closest goal
            _goal = ClosestGoal(goals);

            //walls
            for (int i = 0; i < data.Count - 3; i++)
            {
                int[] wall = new int[4];
                wall[0] = (data[i + 3])[0];
                wall[1] = (data[i + 3])[1];
                wall[2] = (data[i + 3])[2];
                wall[3] = (data[i + 3])[3];
                _walls.Add(wall);
            }
        }

        /// <summary>
        /// Item 1 = rows 
        /// Item 2 = columns 
        /// </summary>
        public Tuple<int, int> GridDimensions
        {
            get
            {
                return _gridDimensions;
            }
        }

        /// <summary>
        /// Item 1 = x cordinate
        /// Item 2 = y cordinate 
        /// </summary>
        public Tuple<int, int> Robot
        {
            get
            {
                return _robot;
            }
        }

        /// <summary>
        /// Item 1 = x cordinate
        /// Item 2 = y cordinate
        /// </summary>
        public Tuple<int, int> Goal
        {
            get
            {
                return _goal;
            }
        }


        /// <summary>
        /// Walls
        /// [0] = x_cordinate
        /// [1] = y_cordinate
        /// [2] = width 
        /// [3] = height
        /// </summary>
        public List<int[]> Walls
        {
            get
            {
                return _walls;
            }
        }

        /// <summary>
        /// File name of the input text file
        /// </summary>
        public string FileName
        {
            get
            {
                return _fileName;
            }
        }
    }
}