using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Level31_FountainOfObjects
{
    public interface IGameBoard
    {

        Position CalculateNewPosition(int row, int column);

    }

}
