using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlTypes;
using System.Dynamic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace clsBus
{
    public class Game
    {
        private int[,] values =     { { 1, 2, 3, 4 },
                                            { 5, 6, 7, 8 },                              { 9, 10, 11, 12 },
                                            { 13, 14, 15, 0 } };
        Random rnd = new Random();

        private int _moves { get; set; } = 0;
        public int Moves => _moves;

        private enum enDirection { UP, DOWN, LEFT, RIGHT };

        private List <enDirection> _ValidMoves()
        {
            Tuple<int, int> pos = _WhereIsTheZero();
            List<enDirection> moves = new List<enDirection>();
            if (pos.Item1 > 0)
            {
                moves.Add(enDirection.UP);
            }
            if (pos.Item1 < values.GetLength(0) - 1)
            {
                moves.Add(enDirection.DOWN);
            }
            if (pos.Item2 > 0)
            {
                moves.Add(enDirection.LEFT);
            }
            if (pos.Item2 < values.GetLength(1) - 1)
            {
                moves.Add(enDirection.RIGHT);
            }
            return moves;
        }

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
                        continue;
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

        public bool PlayMove(int Num)
        {
           return _Swap(Num, 0);
        }
        private bool _Swap(int from,int with)
        {
            Tuple<int, int> pos1 = WhereIsTheNumber(from);
            Tuple<int, int> pos2 = WhereIsTheNumber(with);
            if (IsValidMove(pos1, pos2))
            {
                int temp = values[pos1.Item1, pos1.Item2];
                values[pos1.Item1, pos1.Item2] = values[pos2.Item1, pos2.Item2];
                values[pos2.Item1, pos2.Item2] = temp;
                _IncreaseMoves();
                return true;
            }
            else
            {
                return false;
            }
        }

        public int[,] GetValues()
        {     
                   return values;

        }


        private void _RandomMove()
        {
            Tuple<int, int> zeroPos = _WhereIsTheZero();
            int targetRow = zeroPos.Item1;
            int targetCol = zeroPos.Item2;
            List<enDirection> moves = _ValidMoves();
            int index = rnd.Next(0, moves.Count);
            if(moves[index] == enDirection.UP)
            {
                targetRow--;
            }
            if (moves[index] == enDirection.DOWN)
            {
                targetRow++;
            }
            if (moves[index] == enDirection.LEFT)
            {
                targetCol--;
            }
            if (moves[index] == enDirection.RIGHT)
            {
                targetCol++;
            }
            int targetNumber = values[targetRow, targetCol];
            _Swap(0,targetNumber);
        }

        public void Shuffle()
        {
            for (int i = 0; i < 100; i++)
            {
                _RandomMove();
            }
            _ResetMoves();
        }

        private void _IncreaseMoves()
        {
            _moves++;
        }

        private void _ResetMoves()
        {
            _moves = 0;
        }
        public void Reset()
        {
            values = new int[,] { { 1, 2, 3, 4 },
                                  { 5, 6, 7, 8 },
                                  { 9, 10, 11, 12 },
                                  { 13, 14, 15, 0 } };
            _ResetMoves();
        }
    }
}
