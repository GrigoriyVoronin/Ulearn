using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Digger
{
    //Напишите здесь классы Player, Terrain и другие.
    class Player : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            var el = new CreatureCommand();
            switch (Game.KeyPressed)
            {
                case Keys.Up:
                    if (y - 1 > -1 && !(Game.Map[x, y - 1] is Sack))
                        el.DeltaY = -1;
                    break;
                case Keys.Right:
                    if (x + 1 < Game.MapWidth && !(Game.Map[x + 1, y] is Sack))
                        el.DeltaX = 1;
                    break;
                case Keys.Down:
                    if (y + 1 < Game.MapHeight && !(Game.Map[x, y + 1] is Sack))
                        el.DeltaY = 1;
                    break;
                case Keys.Left:
                    if (x - 1 > -1 && !(Game.Map[x - 1, y] is Sack))
                        el.DeltaX = -1;
                    break;
            }
            return el;
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return conflictedObject is Sack || conflictedObject is Monster;
        }

        public int GetDrawingPriority()
        {
            return 3;
        }

        public string GetImageFileName()
        {
            return "Digger.png";
        }
    }

    class Terrain : ICreature
    {
        public string GetImageFileName()
        {
            return "Terrain.png";
        }

        public int GetDrawingPriority()
        {
            return 0;
        }

        public CreatureCommand Act(int x, int y)
        {
            return new CreatureCommand();
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return true;
        }
    }

    class Sack : ICreature
    {
        int flyTime;
        public CreatureCommand Act(int x, int y)
        {
            var countOfEnableCells = CalculateEmptyCells(x, y);
            CreatureCommand el;
            if (countOfEnableCells == 0)
            {
                if(y + 1 < Game.MapHeight && 
                    (Game.Map[x, y + 1] is Player || Game.Map[x, y + 1] is Monster) 
                    && flyTime >= 1)
                {
                    el = NextStep();
                    Act(x, y + 1);
                    return el;
                }
                else
                    return CheckTransform();
            }
            if (countOfEnableCells == 1)
                return NextStep();
            el = NextStep();
            Act(x, y + 1);
            return el;
        }

        private CreatureCommand CheckTransform ()
        {
            var el = new CreatureCommand();
            if (flyTime > 1)
                el.TransformTo = new Gold();
            else
                flyTime = 0;
            return el;
        }

        private CreatureCommand NextStep ()
        {
            var el = new CreatureCommand();
            el.DeltaY = 1;
            flyTime++;
            return el;
        }

        private int CalculateEmptyCells(int x, int y)
        {
            var count = 0;
            while (y + count + 1 < Game.MapHeight && Game.Map[x, y + count + 1] == null)
                count++;
            return count;
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return false;
        }

        public int GetDrawingPriority()
        {
            return 4;
        }

        public string GetImageFileName()
        {
            return "Sack.png";
        }
    }

    class Gold : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            return new CreatureCommand();
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            if (conflictedObject is Player)
            {
                Game.Scores += 10;
                return true;
            }
            return conflictedObject is Monster;
        }

        public int GetDrawingPriority()
        {
            return 4;
        }

        public string GetImageFileName()
        {
            return "Gold.png";
        }
    }

    class Monster : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            var (pX, pY) = PlayersCoordinate();
            if ( (pX,pY) == (-1,-1) )
                return new CreatureCommand();
            return Move(pX,pY,x,y);
        }

        private CreatureCommand Move(int pX, int pY, int x, int y)
        {
            var el = new CreatureCommand();
            var dX = pX - x;
            var dY = pY - y;
            if (dX > 0 && x + 1 < Game.MapWidth && CheckBox(x+1,y))
                el.DeltaX = 1;
            else if (dX < 0 && x - 1 >-1 && CheckBox(x - 1, y))
                el.DeltaX = -1;
            else if (dY > 0 && y + 1 < Game.MapHeight && CheckBox(x, y+1))
                el.DeltaY = 1;
            else if (dY < 0 && y - 1 > -1 && CheckBox(x, y-1))
                el.DeltaY = -1;
            return el;
        }

        private bool CheckBox(int x,int y)
        {
            return !(Game.Map[x, y] is Terrain || Game.Map[x, y] is Sack || Game.Map[x, y] is Monster);
        }

        private (int,int) PlayersCoordinate()
        {
            for (int i = 0; i < Game.Map.GetLength(0); i++)
                for (int j = 0; j < Game.Map.GetLength(1); j++)
                    if (Game.Map[i, j] is Player)
                        return (i, j);
            return (-1, -1);
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return conflictedObject is Sack || conflictedObject is Monster;
        }

        public int GetDrawingPriority()
        {
            return 3;
        }

        public string GetImageFileName()
        {
            return "Monster.png";
        }
    }
}