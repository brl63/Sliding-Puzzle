using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clsBus
{
    public class Game
    {
       private int[,] values =     { { 1, 2, 3, 4 },
                                            { 5, 6, 7, 8 },
                                            { 9, 10, 11, 12 },
                                            { 13, 14, 15, 0 } };
        public Tuple<int, int> WhereIsTheNumber(int num)
        {
            for (int i = 0; i < values.GetLength(0); i++)
            {
                for (int j = 0; j < values.GetLength(1); j++)
                {
                    if (values[i, j] == num)
                    {
                        return new Tuple<int, int>(i, j);
                    }
                }
            }
            return null;
        }

        private Tuple <int, int> _WhereIsTheZero()
        {
            return WhereIsTheNumber(0);
        }
        public bool IsSolved()
        {
            for (int i = 0; i < values.GetLength(0); i++)
            {
                for (int j = 0; j < values.GetLength(1); j++)
                {

                    if (i ==values.GetLength(0) - 1 && j == values.GetLength(1) - 1 && values[i, j] == 0)
                    {
                       return true;
                    }
                    if (values[i, j] != i * 4 + j + 1)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public bool IsValidMove(Tuple<int, int> from, Tuple<int, int> to)
        {
            if ( (Math.Abs(from.Item2 - to.Item2)) +(Math.Abs(from.Item1 - to.Item1)) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
          
        }

        public bool Swap(int from,int with)
        {
            Tuple<int, int> pos1 = WhereIsTheNumber(from);
            Tuple<int, int> pos2 = WhereIsTheNumber(with);
            if (IsValidMove(pos1, pos2))
            {
                int temp = values[pos1.Item1, pos1.Item2];
                values[pos1.Item1, pos1.Item2] = values[pos2.Item1, pos2.Item2];
                values[pos2.Item1, pos2.Item2] = temp;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void GetValues()
        {     
                   

        }

    }
}
