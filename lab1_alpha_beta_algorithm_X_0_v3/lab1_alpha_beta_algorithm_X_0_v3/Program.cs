// See https://aka.ms/new-console-template for more information
using lab1_alpha_beta_algorithm_X_0_v3;


    string read;
    int AI = 10;
    int human = -10;
    while (true)
    {
        Console.WriteLine("Введите 0 - если хотите играть за 0, введите X - если хотите играть за X");
        read = Console.ReadLine();
        if (read == "X" || read == "x")
        {
            AI = -10;
            human = 10;
            break;
        }
        else if (read == "0" || read == "o" || read == "O")
        {
            AI = 10;
            human = -10;
            break;
        }
    }

Game game = new Game(AI, human);
Console.WriteLine(game);
while (true)
{
    if (AI == 10)
    {
        game.MoveAI();
        if (game.IsEnd())
        {
            Console.Clear();
            Console.WriteLine(game);
            break;
        }
        Console.WriteLine(game);
        while (true) 
        {
            Console.WriteLine("Введите клетку на которую хотите походить: ");
            read = Console.ReadLine();
            int tmp=0;
            if (int.TryParse(read, out tmp)) 
            {
                if (game.MoveHuman(int.Parse(read)))
                    break;
                else
                    Console.WriteLine("Данная клетка занята или не существует");
            }
            else
                Console.WriteLine("Некоректный ввод");
        }
        if (game.IsEnd())
        {
            Console.Clear();
            Console.WriteLine(game);
            break;
        }
        Console.Clear();
        Console.WriteLine(game);
    }
    else
    {
        
        while (true)
        {
            Console.WriteLine("Введите клетку на которую хотите походить: ");
            read = Console.ReadLine();
            int tmp = 0;
            if (int.TryParse(read, out tmp))
            {
                if (game.MoveHuman(int.Parse(read)))
                    break;
                else
                    Console.WriteLine("Данная клетка занята или не существует");
            }
            else
                Console.WriteLine("Некоректный ввод");
        }
        if (game.IsEnd())
        {
            Console.Clear();
            Console.WriteLine(game);
            break;
        }
        Console.Clear();
        Console.WriteLine(game);

        game.MoveAI();
        if (game.IsEnd())
        {
            Console.Clear();
            Console.WriteLine(game);
            break;
        }
        Console.WriteLine(game);
    }

}








    /*
    //List<int> gameBoard = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    List<int> gameBoard = new List<int>() { 10, 10, -10, 4, -10, 6, 7, -10, 10 };
    Tree tree = new Tree(gameBoard, AI, human);
    int minimax = tree.minimax(tree.root, 1, int.MinValue, int.MaxValue, AI, true);
    Console.WriteLine(minimax);
    Console.WriteLine(tree.countNodes);
    Console.WriteLine(tree);
    Console.WriteLine(tree.BestMove());


    */