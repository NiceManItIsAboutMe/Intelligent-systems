using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1_alpha_beta_algorithm_X_0_v3
{
    internal class Game
    {
        public List<int> gameBoard;

        int whoWin = -1;
        int AI;
        int Human;
        public int whoMove = 10;
        public int currentTurn = -1;
        /// <summary>
        /// X=10 0=-10
        /// </summary>
        /// <param name="AI">10 or -10</param>
        /// <param name="Human">10 or -10</param>
        public Game(int AI, int Human)
        {
            gameBoard = new List<int>() { 1,2,3,4,5,6,7,8,9};
            this.AI = AI;
            this.Human = Human;
        }

        public bool IsEnd()
        {
            if (Tree.Win(gameBoard, AI))
                whoWin = AI;
            else if (Tree.Win(gameBoard, Human))
                whoWin = Human;
            else if (Tree.GetEmptyIndices(gameBoard).Count == 0)
                whoWin = 0;
            else
                return false;
            return true;
                   
        }

        public void MoveAI()
        {
            Tree tree = new Tree(gameBoard, AI, Human);
            currentTurn = tree.BestMove()+1;
            gameBoard[currentTurn-1] = AI;
            whoMove = Human;
        }

        public bool MoveHuman(int move)
        {
            bool res=false;
            foreach (var item in Tree.GetEmptyIndices(gameBoard))
            {
                if (move == item+1) 
                { 
                    res = true; break; 
                }    
            }
            if (!res)
                return false;
            gameBoard[move - 1] = Human;
            currentTurn = move;
            whoMove = AI;
            return res;
        }

        public override string ToString()
        {
            string res = "";

            int i = 0;
            foreach (var item in gameBoard)
            {
                i++;
                if (i == 4 || i == 1 || i == 7)
                    res += "\n" + "-----------------\n";
                res += "| ";
                if (item == 10)
                    res += "X";
                else if (item == -10)
                    res += "O";
                else
                    res += item.ToString();
                res += " | ";
            }
            res += "\n" + "-----------------\n";
            if(currentTurn!=-1)
            if (whoMove == AI)
                res += "Вы походили на " + currentTurn + " клетку";
            else
                res += "ИИ походил на " + currentTurn + " клетку, какой же он гений, не правда ли ?";

            if (whoWin == 0)
                res += "\nЛегкая игра, ничья, но даже не думай о ней в следующий раз!";
            else if (whoWin == AI)
                res += "\nГений победил, и это не ты\n PS а я говорил куда ходить...";
            else if (whoWin == Human)
                res += "\nВ этот раз тебе повезло, сдаюсь";

            return res;
        }
    }
}
