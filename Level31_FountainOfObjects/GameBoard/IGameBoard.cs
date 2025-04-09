using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Level31_FountainOfObjects.GameBoard
{
    public interface IGameBoard
    {

        Position CalculateNewPosition(int row, int column);

    }

}
