using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1_alpha_beta_algorithm_X_0_v3
{

    internal class Node
    {
        /// <summary>
        /// {1,2,3,4,5,6,7,8,9}
        /// </summary>
        public List<int> gameBoard;
        /// <summary>
        /// дети
        /// </summary>
        public List<Node> nodes;
        /// <summary>
        /// вес
        /// </summary>
        public int weight;
        /// <summary>
        /// глубина
        /// </summary>
        public int depth;
        /// <summary>
        /// 10 or -10
        /// </summary>
        int player;
        /// <summary>
        /// куда походил
        /// </summary>
        public int currentTurn;

        /// <summary>
        /// создаем копию gameBoard
        /// </summary>
        /// <param name="gameBoard"></param>
        public Node(List<int> gameBoard)
        {
            this.gameBoard = new List<int>();
            foreach (var item in gameBoard)
            {
                this.gameBoard.Add(item);
            }
            nodes = new List<Node>();
        }
        public Node(List<int> gameBoard,int currentTurn,int depth=0)
        {
            this.gameBoard = new List<int>();
            foreach (var item in gameBoard)
            {
                this.gameBoard.Add(item);
            }
            this.currentTurn = currentTurn;
            this.depth = depth;
            nodes = new List<Node>();
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
            res += "Ход=" + currentTurn + " Глубина=" + depth + " Вес=" + weight;

            return res;
        }
    }
    internal class Tree
    {
        public Node root;
        /// <summary>
        /// X=10 0=-10
        /// </summary>
        int AI;
        int human;

        public int countNodes = 0;
        public Tree(List<int> gameBoard,int AI, int human)
        {
            root = new Node(gameBoard);
            countNodes++;
            this.AI = AI;
            this.human = human;
            minimax(root, 1, int.MinValue, int.MaxValue, AI);
        }

        public int minimax(Node currentNode,int depth,int alpha,int beta, int AIorHuman,bool A_Fpruning=true)
        {
            //если это лист, то возвращаем оценку конечного состояния
            if ( IsTerminated(currentNode, AIorHuman))
            {
                //то есть вес нашего листа
                currentNode.weight = HeuristicEval(currentNode, AIorHuman);
                return currentNode.weight;
            }
            
            int eval;
            List<int> board = new List<int>(currentNode.gameBoard);
            //Делаем все ходы по очереди, создаем новые листья для дерева
            Node temp;
            foreach (var turn in GetEmptyIndices(currentNode))
            {
                board[turn] = AIorHuman;
                temp = new Node(board, turn + 1,depth);
                countNodes++;
                currentNode.nodes.Add(temp);
                if (IsTerminated(temp, AIorHuman))
                    break;
                board[turn] = turn + 1;
            }
            //ход ИИ
            if (AIorHuman == AI)
            {
                int max = int.MinValue;
                foreach (var child in currentNode.nodes)
                {
                    //это есть вес данного поля
                    eval = minimax(child, depth+1, alpha, beta, -AIorHuman,A_Fpruning);
                    child.depth = depth;
                    child.weight = eval;

                    max = Max(max, eval);
                    if (A_Fpruning)
                    {
                        alpha = Max(alpha, eval);
                        //отсекаем 
                        if (beta <= alpha)
                            break;
                    }
                    
                   
                }
                //вернем лучший вес
                return max;
            }
            //Ход человека
            else
            {
                int min=int.MaxValue;
                foreach (var child in currentNode.nodes)
                {
                    eval = minimax(child, depth +1, alpha, beta, -AIorHuman,A_Fpruning);

                    child.weight = eval;

                    min = Min(min, eval);
                    if (A_Fpruning)
                    {
                        beta = Min(beta, eval);
                        //отсекаем
                        if (beta <= alpha)
                            break;
                    }
                }
                return min;
            }
        }
        int Max(int n1,int n2)
        {
            if (n1 > n2)
                return n1;
            else
                return n2;
        }
        int Min(int n1, int n2)
        {
            if (n1 < n2)
                return n1;
            else
                return n2;
        }
        int HeuristicEval(Node node, int AIorHuman)
        {
            if (Win(node, AI)||Win(node,-AI))
            {
                if (AIorHuman == AI)
                    return -100 / (node.depth+1);
                else
                    return 100 / (node.depth+1);
            }
            else if (GetEmptyIndices(node).Count == 0)
                return 0;
            //может изменим оценку тут как-то
            else
                return 0;
        }
        public static bool IsTerminated(Node node,int AIorHuman )
        {
            if (Win(node, AIorHuman) || Win(node, -AIorHuman) || GetEmptyIndices(node).Count == 0)
                return true;
            else
                return false;
        }
        public static bool Win(List<int> gameBoard, int AIorHuman)
        {
            int p = AIorHuman;
            if ((gameBoard[0] == p && gameBoard[1] == p && gameBoard[2] == p) ||
               (gameBoard[3] == p && gameBoard[4] == p && gameBoard[5] == p) ||
               (gameBoard[6] == p && gameBoard[7] == p && gameBoard[8] == p) ||

               (gameBoard[0] == p && gameBoard[3] == p && gameBoard[6] == p) ||
               (gameBoard[1] == p && gameBoard[4] == p && gameBoard[7] == p) ||
               (gameBoard[2] == p && gameBoard[5] == p && gameBoard[8] == p) ||

               (gameBoard[0] == p && gameBoard[4] == p && gameBoard[8] == p) ||
               (gameBoard[2] == p && gameBoard[4] == p && gameBoard[6] == p))
                return true;
            else
                return false;

        }
        public static bool Win(Node node, int AIorHuman)
        {
            List<int> gameBoard = node.gameBoard;
            int p = AIorHuman;
            if ((gameBoard[0] == p && gameBoard[1] == p && gameBoard[2] == p) ||
               (gameBoard[3] == p && gameBoard[4] == p && gameBoard[5] == p) ||
               (gameBoard[6] == p && gameBoard[7] == p && gameBoard[8] == p) ||

               (gameBoard[0] == p && gameBoard[3] == p && gameBoard[6] == p) ||
               (gameBoard[1] == p && gameBoard[4] == p && gameBoard[7] == p) ||
               (gameBoard[2] == p && gameBoard[5] == p && gameBoard[8] == p) ||

               (gameBoard[0] == p && gameBoard[4] == p && gameBoard[8] == p) ||
               (gameBoard[2] == p && gameBoard[4] == p && gameBoard[6] == p))
                return true;
            else
                return false;

        }

        public static List<int> GetEmptyIndices(Node node)
        {
            List<int> res = new List<int>();
            foreach (var item in node.gameBoard)
            {
                if (item != -10 && item != 10)
                    res.Add(item - 1);
            }
            return res;
        }
        public static List<int> GetEmptyIndices(List<int> gameBoard)
        {
            List<int> res = new List<int>();
            foreach (var item in gameBoard)
            {
                if (item != -10 && item != 10)
                    res.Add(item - 1);
            }
            return res;
        }
        

        public int BestMove()
        {
            int max = root.nodes.Max(t => t.weight);
            foreach (var item in root.nodes)
            {
                if (item.weight == max)
                    return item.currentTurn-1;
            }
            return 0;
        }
        public override string ToString()
        {
            string res = "";
            FullBypassTree(root, ref res);
            res += "\nВсего ходов=" + countNodes;
            return res;
        }
        void FullBypassTree(Node start, ref string res)
        {
            //if (start.depth == 9)
            {
                res += start.ToString();
            }

            foreach (var item in start.nodes)
            {
                FullBypassTree(item, ref res);
            }
        }
    }
}


/*
 int minimax(Node currentNode,int depth,int alpha,int beta, int AIorHuman)
        {
            if (depth == 0 || IsTerminated(currentNode, AIorHuman))
                return HeuristicEval(currentNode,AIorHuman);
            int eval;
            if (AIorHuman == AI)
            {
                int max = int.MinValue;
                foreach (var child in currentNode.nodes)
                {
                    eval = minimax(child, depth - 1, alpha, beta, -AIorHuman);
                    max = Max(max, eval);
                    alpha = Max(alpha, eval);
                    //отсекаем
                    if (beta <= alpha)
                        break;
                   
                }
                return max;
            }
            else
            {
                int min=int.MaxValue;
                foreach (var child in currentNode.nodes)
                {
                    eval = minimax(child, depth - 1, alpha, beta, -AIorHuman);
                    min = Min(min, eval);
                    beta = Min(beta, eval);
                    //отсекаем
                    if (beta <= alpha)
                        break;
                }
                return min;
            }
        }*/