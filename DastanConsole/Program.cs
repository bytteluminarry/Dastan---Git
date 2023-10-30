//Skeleton Program code for the AQA A Level Paper 1 Summer 2023 examination
//this code should be used in conjunction with the Preliminary Material
//written by the AQA Programmer Team
//developed in the Visual Studio Community Edition programming environment

using System;
using System.Collections.Generic;

namespace Dastan
{
    class Program
    {
        static void Main(string[] args)
        {
            Dastan ThisGame = new Dastan(8, 7, 5);
            ThisGame.PlayGame();
            Console.WriteLine("Goodbye!");
            Console.ReadLine();
        }
    }

    class WeatherEvent
    {
        private int countdown = 10;

        public int GetCountDown()
        {
            return countdown;
        }

        public void DecrementCountDown()
        {
            countdown--;
        }

        private int SquareReference;
        public bool CountDownComplete()
        {
            return countdown == 0;
        }

        public void SetWeatherLocation(int SquareReference)
        {
            this.SquareReference = SquareReference;
        }

        public int GetWeatherLocation()
        {
            return SquareReference;
        }
    }

    class Dastan
    {
        protected List<Square> Board;
        protected int NoOfRows, NoOfColumns, MoveOptionOfferPosition;
        protected List<Player> Players = new List<Player>();
        protected List<string> MoveOptionOffer = new List<string>();
        protected Player CurrentPlayer;
        protected Random RGen = new Random();

        public void PlaceBarrier(Player currentPlayer, int SquareReference)
        {
            Barrier barrier = new Barrier(currentPlayer);
            int Row = SquareReference / 10;
            int Col = SquareReference % 10;
            for (int i = Col - 1; i <= Col + 1; i++)
            {
                int BarrierReference = Convert.ToInt32(Row + "" + i);
                Piece piece = new Piece("", currentPlayer, 0, " ");
                Board[GetIndexOfSquare(BarrierReference)] = barrier;
                Board[GetIndexOfSquare(BarrierReference)].SetPiece(piece);
            }
        }

        public bool CheckBarrierIsValid(Player currenPlayer, int SquareReference)
        {
            int Row = SquareReference / 10;
            int Col = SquareReference % 10;
            bool isValid = true;
            for(int i=Col-1; i <= Col + 1; i++)
            {
                if (i > NoOfColumns)
                {
                    isValid = false;
                    break;
                }
                int BarrierReference = Convert.ToInt32(Row + "" + i);
                if(Board[GetIndexOfSquare(BarrierReference)].GetPieceInSquare() != null)
                {
                    isValid = false;
                    break;
                }
            }
            return isValid;
        }

        private WeatherEvent weatherEvent;
        public bool WeatherEventOccurs()
        {
            bool occuring = new Random().Next(0, 100) <= 50; //true 50 percent of occuring
            if (occuring)
            {
                weatherEvent = new WeatherEvent();
                Console.WriteLine("The Weather Event is occuring in : "+weatherEvent.GetCountDown());

                int SquareReference = 0;
                int RandomRow, RandomColumn;
                bool stop = false;
                while(stop == false)
                {
                    RandomRow = new Random().Next(1, NoOfRows);
                    RandomColumn = new Random().Next(1, NoOfColumns);
                    SquareReference = Convert.ToInt32(RandomRow + "" + RandomColumn); //76
                    Piece piece = Board[GetIndexOfSquare(SquareReference)].GetPieceInSquare();
                    if (piece == null)
                    {
                        weatherEvent.SetWeatherLocation(SquareReference);
                        Console.WriteLine("The Weather Event Location is set on square (" + SquareReference + ")");
                        stop = true;
                        int countdown = weatherEvent.GetCountDown();
                        for (int i = countdown; i >= 0; i--)
                        {
                            Console.WriteLine("Time remaining: {0} seconds", i);
                            weatherEvent.DecrementCountDown();
                            System.Threading.Thread.Sleep(1000);
                        }
                        for(int i = 1; i <= NoOfRows; i++)
                        {
                            int FinishSquareReference = Convert.ToInt32(i + "" + RandomColumn);
                            piece = Board[GetIndexOfSquare(FinishSquareReference)].GetPieceInSquare();
                            if (piece != null && piece.GetBelongsTo()!=null)
                            {
                                Board[GetIndexOfSquare(FinishSquareReference)].SetPiece(null);
                                Square square = new Square();
                                Board[GetIndexOfSquare(FinishSquareReference)] = square;
                            }
                        }
                    }
                }

            }
            return occuring;
        }

        public bool AwardWafr()
        {
            return new Random().Next(0, 100) <= 25;
        }

        public void CreateCustomPlayers()
        {
            string player1Name = "";
            string player2Name = "";

            Console.WriteLine("Enter the name of Player One");
            player1Name = Console.ReadLine();
            Players.Add(new Player(player1Name, 1));

            do
            {
                Console.WriteLine("Enter the name of Player Two");
                player2Name = Console.ReadLine();
            }
            while (player1Name.Equals(player2Name));

            Players.Add(new Player(player2Name, -1));
        }

        protected int NoOfPieces;

        public Dastan(int R, int C, int NoOfPieces)
        {

            this.NoOfPieces = NoOfPieces;

            CreateCustomPlayers();

            CreateMoveOptions();
            NoOfRows = R;
            NoOfColumns = C;
            MoveOptionOfferPosition = 0;
            CreateMoveOptionOffer();
            CreateBoard();
            CreatePieces(NoOfPieces);
            CurrentPlayer = Players[0];
        }

        private void DisplayBoard()
        {
            Console.Write(Environment.NewLine + "   ");
            for (int Column = 1; Column <= NoOfColumns; Column++)
            {
                Console.Write(Column.ToString() + "  ");
            }
            Console.Write(Environment.NewLine + "  ");
            for (int Count = 1; Count <= NoOfColumns; Count++)
            {
                Console.Write("---");
            }
            Console.WriteLine("-");
            for (int Row = 1; Row <= NoOfRows; Row++)
            {
                Console.Write(Row.ToString() + " ");
                for (int Column = 1; Column <= NoOfColumns; Column++)
                {
                    int Index = GetIndexOfSquare(Row * 10 + Column);
                    Console.Write("|" + Board[Index].GetSymbol());
                    Piece PieceInSquare = Board[Index].GetPieceInSquare();
                    if (PieceInSquare == null)
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write(PieceInSquare.GetSymbol());
                    }
                }
                Console.WriteLine("|");
            }
            Console.Write("  -");
            for (int Column = 1; Column <= NoOfColumns; Column++)
            {
                Console.Write("---");
            }
            Console.WriteLine();
            Console.WriteLine();
        }

        private void DisplayState()
        {
            DisplayBoard();
            Console.WriteLine("Move option offer: " + MoveOptionOffer[MoveOptionOfferPosition]);
            Console.WriteLine();
            Console.WriteLine(CurrentPlayer.GetPlayerStateAsString());
            Console.WriteLine("Turn: " + CurrentPlayer.GetName());
            Console.WriteLine();
        }

        private int GetIndexOfSquare(int SquareReference)  //square reference = Row = 2, Column = 3  = 23
        {
            int Row = SquareReference / 10;
            int Col = SquareReference % 10;
            return (Row - 1) * NoOfColumns + (Col - 1);
        }

        private bool CheckSquareInBounds(int SquareReference)
        {
            int Row = SquareReference / 10;
            int Col = SquareReference % 10;
            if (Row < 1 || Row > NoOfRows)
            {
                return false;
            }
            else if (Col < 1 || Col > NoOfColumns)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool CheckSquareIsValid(int SquareReference, bool StartSquare)
        {
            if (!CheckSquareInBounds(SquareReference))
            {
                return false;
            }
            Piece PieceInSquare = Board[GetIndexOfSquare(SquareReference)].GetPieceInSquare();
            if (PieceInSquare == null)
            {
                if (StartSquare)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else if (CurrentPlayer.SameAs(PieceInSquare.GetBelongsTo()))
            {
                if (StartSquare)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (Board[GetIndexOfSquare(SquareReference)].ContainsBarrier())
                {
                    return false;
                }
                else
                {
                    if (StartSquare)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }

        private bool CheckIfGameOver()
        {
            bool Player1HasMirza = false;
            bool Player2HasMirza = false;
            foreach (var S in Board)
            {
                Piece PieceInSquare = S.GetPieceInSquare();
                if (PieceInSquare != null)
                {
                    if (S.ContainsKotla() && PieceInSquare.GetTypeOfPiece() == "mirza" && !PieceInSquare.GetBelongsTo().SameAs(S.GetBelongsTo()))
                    {
                        return true;
                    }
                    else if (PieceInSquare.GetTypeOfPiece() == "mirza" && PieceInSquare.GetBelongsTo().SameAs(Players[0]))
                    {
                        Player1HasMirza = true;
                    }
                    else if (PieceInSquare.GetTypeOfPiece() == "mirza" && PieceInSquare.GetBelongsTo().SameAs(Players[1]))
                    {
                        Player2HasMirza = true;
                    }
                }
            }
            return !(Player1HasMirza && Player2HasMirza);
        }

        private int GetSquareReference(string Description)
        {
            int SelectedSquare;
            Console.Write("Enter the square " + Description + " (row number followed by column number): ");

            //SelectedSquare = Convert.ToInt32(Console.ReadLine());

            while (!GetValidInt(Console.ReadLine(), out SelectedSquare))
            {
                Console.WriteLine("Please enter a valid integer");

            }

            return SelectedSquare;
        }

        private void UseMoveOptionOffer()
        {
            int ReplaceChoice;
            Console.Write("Choose the move option from your queue to replace (1 to 5): ");

            while (!GetValidInt(Console.ReadLine(), out ReplaceChoice) || ReplaceChoice == 8)
            {
                Console.WriteLine("Please enter a valid integer");

            }

            CurrentPlayer.UpdateMoveOptionQueueWithOffer(ReplaceChoice - 1, CreateMoveOption(MoveOptionOffer[MoveOptionOfferPosition], CurrentPlayer.GetDirection()));
            CurrentPlayer.ChangeScore(-(10 - (ReplaceChoice * 2)));
            MoveOptionOfferPosition = RGen.Next(0, 5);

            CurrentPlayer.DecreaseChoiceOptionsLeft();

        }

        private int GetPointsForOccupancyByPlayer(Player CurrentPlayer)
        {
            int ScoreAdjustment = 0;
            foreach (var S in Board)
            {
                ScoreAdjustment += (S.GetPointsForOccupancy(CurrentPlayer));
            }
            return ScoreAdjustment;
        }

        private void UpdatePlayerScore(int PointsForPieceCapture)
        {
            CurrentPlayer.ChangeScore(GetPointsForOccupancyByPlayer(CurrentPlayer) + PointsForPieceCapture);
        }

        private int CalculatePieceCapturePoints(int FinishSquareReference)
        {
            try
            {

                if (Board[GetIndexOfSquare(FinishSquareReference)].GetPieceInSquare() != null)
                {
                    return Board[GetIndexOfSquare(FinishSquareReference)].GetPieceInSquare().GetPointsIfCaptured();
                }
            }
            catch { }
            return 0;
        }

        public bool GetValidInt(string input, out int result) //  "1"   "a"
        {
            return int.TryParse(input, out result);
        }

        private int ModifyQueueOptions()
        {
            int Choice;
            do
            {
                Console.WriteLine("Options: ");
                Console.WriteLine("1.	Reverse the current player queue");
                Console.WriteLine("2.	Swap the current player queue with the opponent queue");
                Console.WriteLine("3.	Swap the first and last elements in the current player queue");
                Console.WriteLine("4.	Move one of the move options to the front of the current player queue");
                Console.WriteLine("5.	Nothing (make normal move)");
                Choice = Convert.ToInt32(Console.ReadLine());
            } while (Choice < 1 || Choice > 5);
            return Choice;
        }

        private string GetPlayerPieceSymbol(Player player)
        {
            int StartSquareReference = 0;
            for (int Row = 1; Row <= NoOfRows; Row++)
            {
                for (int Column = 1; Column <= NoOfColumns; Column++)
                {
                    StartSquareReference = Convert.ToInt32(Row + "" + Column);
                    Piece piece = Board[GetIndexOfSquare(StartSquareReference)].GetPieceInSquare();
                    if (piece != null)
                    {
                        if (CurrentPlayer.SameAs(piece.GetBelongsTo()) && piece.GetTypeOfPiece() != "mirza")
                        {
                            return piece.GetSymbol();
                        }
                    }
                }

            }

            return "";
        }

        private int CountNormalPieces(Player player)
        {
            int countOfPieces = 0;
            int StartSquareReference = 0;
            for (int Row = 1; Row <= NoOfRows; Row++)
            {
                for (int Column = 1; Column <= NoOfColumns; Column++)
                {
                    StartSquareReference = Convert.ToInt32(Row + "" + Column);
                    Piece piece = Board[GetIndexOfSquare(StartSquareReference)].GetPieceInSquare();
                    if(piece != null)
                    {
                        if (CurrentPlayer.SameAs(piece.GetBelongsTo()) && piece.GetTypeOfPiece() != "mirza")
                        {
                            countOfPieces++;
                        }
                    }
                }

            }

            return countOfPieces;
        }

        private bool CheckReincarnation(Player currenPlayer, int direction, int FinishSquareReference)
        {
            bool canReincarnate = false;
            int Row = FinishSquareReference / 10;
            int Col = FinishSquareReference % 10;

            int backRow = NoOfRows - 2;
            int playerFirstRow = 2;

            if(direction == -1)
            {
                backRow = 3;
                playerFirstRow = NoOfRows - 1;
            }

            if(Row == backRow)
            {
                Console.WriteLine("Would you like to reincarnate? (y/n)");
                string answer = Console.ReadLine();
                if(answer == "y")
                {
                    bool isValid = false;
                    do
                    {
                        Console.WriteLine("Select a column to reincarnate a piece");
                        int column = Convert.ToInt32(Console.ReadLine());

                        isValid = CheckSquareIsValid(Convert.ToInt32(playerFirstRow + "" + column), false);

                        if (isValid)
                            Board[GetIndexOfSquare(Convert.ToInt32(playerFirstRow + "" + column))].SetPiece(new Piece("piece", currenPlayer, 1, GetPlayerPieceSymbol(currenPlayer)));

                    } while (isValid == false);
                }
            }

            return canReincarnate;
        }

        public void PlayGame()
        {



            bool GameOver = false;
            while (!GameOver)
            {
                DisplayState();
                bool SquareIsValid = false;
                int Choice;

                int maxNumber;
                bool hasBeenAwarded = false;

                bool settingSecondKotla = false;
                bool hasChosenOption6 = false;

                WeatherEventOccurs();

                if (!CurrentPlayer.HasPlacedBarrier())
                {
                    bool isValid = false;
                    int reference = 0;
                    while(isValid == false)
                    {
                        Console.WriteLine("Enter a square reference to place a barrier");
                        reference = Convert.ToInt32(Console.ReadLine());
                        isValid = CheckBarrierIsValid(CurrentPlayer, reference);
                    }
                    PlaceBarrier(CurrentPlayer, reference);
                    CurrentPlayer.SetPlacedBarrier();
                    continue;
                }

                do
                {
                    maxNumber = 3;
                    if (CurrentPlayer.GetWafrAwarded() == false && AwardWafr())
                    {
                        CurrentPlayer.SetWafrAwarded(true);
                        Console.WriteLine("You have been awarded a Wafr, you can select any move from your queue for free this turn.");
                        maxNumber = 7;
                        hasBeenAwarded = true;
                    }
                    else Console.Write("-Choose move option to use from queue (1 to " + maxNumber + ") or 9 to take the offer, or 7 to place a new kotla, or 6 to get sub options: ");

                    if (GetValidInt(Console.ReadLine(), out Choice) == false) //true
                    {
                        Console.WriteLine("Please enter an Integer");
                        continue;
                    }

                    if (Choice == 6)
                    {
                        int optionChoice = ModifyQueueOptions();
                        if (optionChoice >= 1 || Choice <= 4)
                            CurrentPlayer.ChangeScore(-3);

                        if (optionChoice == 1)
                            CurrentPlayer.ReversePlayerQueue();
                        else if (optionChoice == 2)
                        {
                            MoveOptionQueue temporaryQueue = CurrentPlayer.GetMoveOptionQueue();
                            if (CurrentPlayer.SameAs(Players[0]))
                            {
                                CurrentPlayer.ReplaceQueue(Players[1].GetMoveOptionQueue());
                                Players[1].ReplaceQueue(temporaryQueue);
                            }
                            else
                            {
                                CurrentPlayer.ReplaceQueue(Players[0].GetMoveOptionQueue());
                                Players[0].ReplaceQueue(temporaryQueue);
                            }
                        }
                        else if(optionChoice == 3)
                        {
                            CurrentPlayer.SwapFirstAndLast();
                        }
                        else if (optionChoice == 4)
                        {
                            CurrentPlayer.MoveItemToFront(3);
                        }
                        hasChosenOption6 = true;
                        break;
                    }

                    else if (Choice == 7)
                    {
                        if (CurrentPlayer.GetSecondKotlaState() == false)
                        {
                            settingSecondKotla = true;
                        }
                        break;
                    }

                    else if (Choice == 8)
                    {
                        Player OtherPlayer;
                        if (CurrentPlayer.SameAs(Players[0]))
                        {
                            OtherPlayer = Players[1];
                        }
                        else
                        {
                            OtherPlayer = Players[0];
                        }
                        CurrentPlayer.ChangeScore(-5);
                        Console.WriteLine("Other Player's Queue: " + OtherPlayer.GetJustQueueAsString());
                        Console.WriteLine(CurrentPlayer.GetPlayerStateAsString());
                    }

                    else if (Choice == 9)
                    {
                        if (CurrentPlayer.GetChoiceOptionsLeft() > 0)
                        {
                            UseMoveOptionOffer();
                            Console.WriteLine("Offers left: " + CurrentPlayer.GetChoiceOptionsLeft());
                            DisplayState();
                        }
                        else Console.WriteLine("You have no available offer to choose");
                    }

                }
                while (Choice < 1 || Choice > maxNumber);

                if (hasChosenOption6) continue;

                int StartSquareReference = 0;
                while (!SquareIsValid)
                {
                    StartSquareReference = 0; 
                    if(settingSecondKotla == false )
                        StartSquareReference = GetSquareReference("containing the piece to move");
                    else StartSquareReference = GetSquareReference("containing the piece to sacrifice");
                    SquareIsValid = CheckSquareIsValid(StartSquareReference, true);
                    if (!SquareIsValid) Console.WriteLine("Wrong choice ");
                }
                if (settingSecondKotla)
                {

                    Piece CurrentPiece = new Piece("mirza", Players[0], 5, "2");

                    Kotla S = new Kotla(CurrentPlayer, "K");

                    Board[GetIndexOfSquare(StartSquareReference)] = S;

                    Board[GetIndexOfSquare(StartSquareReference)].SetPiece(CurrentPiece);


                    CurrentPlayer.SetSecondKotla();

                    if (CurrentPlayer.SameAs(Players[0]))
                    {
                        CurrentPlayer = Players[1];
                    }
                    else
                    {
                        CurrentPlayer = Players[0];
                    }
                    continue;
                }
                int FinishSquareReference = 0;
                SquareIsValid = false;
                bool isSahmMove = false;
                if (!CurrentPlayer.ChoiceIsSahm(Choice))
                {
                    while (!SquareIsValid)
                    {
                        FinishSquareReference = GetSquareReference("to move to");

                        bool isTaziz = CheckIfTaziz(CurrentPlayer,StartSquareReference ,FinishSquareReference);

                        if (!isTaziz)
                        {
                            SquareIsValid = CheckSquareIsValid(FinishSquareReference, false);

                            bool canReincarnate = CheckReincarnation(CurrentPlayer, CurrentPlayer.GetDirection(), FinishSquareReference);

                            if (!SquareIsValid) Console.WriteLine("Wrong choice");
                        }
                        else break ;
                    }
                }
                else
                {
                    //player has chosen sahm
                    isSahmMove = true;
                    if (CurrentPlayer.GetSahmStatus())
                    {
                        Console.WriteLine("Sahm can be used once");
                        continue;
                    }
                    CurrentPlayer.SetSahmUsed();
                    int StartRow = StartSquareReference / 10;
                    int StartColumn = StartSquareReference % 10;
                    StartRow += CurrentPlayer.GetDirection();
                    int CapturedPoints = 0;
                    if (CurrentPlayer.GetDirection() < 0)
                    {
                        for (int i = StartRow; i > 0; i--)
                        {
                            CapturedPoints = CalculateSahmMove(CurrentPlayer, StartRow, StartColumn);
                            StartRow += CurrentPlayer.GetDirection(); //5 (-1)  4, 3, 2, 1
                        }
                    }
                    else
                    {
                        for (int i = StartRow; i <= NoOfRows; i++)
                        {
                            CapturedPoints = CalculateSahmMove(CurrentPlayer, StartRow, StartColumn);
                            StartRow += CurrentPlayer.GetDirection(); //2 (1) 3, 4, 5, 6
                        }
                    }
                    CurrentPlayer.ChangeScore(CapturedPoints);
                }
                bool MoveLegal = CurrentPlayer.CheckPlayerMove(Choice, StartSquareReference, FinishSquareReference);
                string answer = "";
                if (MoveLegal && !isSahmMove)
                {
                    int PointsForPieceCapture = CalculatePieceCapturePoints(FinishSquareReference);

                    CurrentPlayer.UpdateQueueAfterMove(Choice);
                    UpdateBoard(StartSquareReference, FinishSquareReference);

                    int score = (Choice + (2 * (Choice - 1)));

                    Console.WriteLine("New score: " + (CurrentPlayer.GetScore()-score) + Environment.NewLine);

                    Console.WriteLine("Would you like to undo your move? y/n");
                    answer = Console.ReadLine();
                    if (answer.ToLower().Equals("y"))
                    {
                        Console.WriteLine("Choose a position");
                        int pos = Convert.ToInt32(Console.ReadLine());
                        if(!zeroCost)
                        CurrentPlayer.ChangeScore(-5);
                        CurrentPlayer.ResetQueueBackAfterUndo(1); 
                    }
                    else if (answer.ToLower().Equals("n"))
                    {
                        if (!hasBeenAwarded && !zeroCost)
                            CurrentPlayer.ChangeScore(-score);
                    }
                    if (!zeroCost)
                    {
                        UpdatePlayerScore(PointsForPieceCapture);
                    }
                }
                if (!answer.ToLower().Equals("y"))
                {
                    if (CurrentPlayer.SameAs(Players[0]))
                    {
                        CurrentPlayer = Players[1];
                    }
                    else
                    {
                        CurrentPlayer = Players[0];
                    }
                }
                GameOver = CheckIfGameOver();
               
            }
            DisplayState();
            DisplayFinalResult();
        }

        private bool CheckIfTaziz(Player currentPlayer, int StartSquareReference, int FinishSquareReference)
        {
            try
            {
                if (Board[GetIndexOfSquare(FinishSquareReference)].GetPieceInSquare() != null)
                {
                    if(Board[GetIndexOfSquare(FinishSquareReference)].GetPieceInSquare().GetTypeOfPiece() == "taziz")
                    {
                        Piece p = new Piece("taziz", currentPlayer, 0, GetPlayerPieceSymbol(currentPlayer));
                        Board[GetIndexOfSquare(FinishSquareReference)].SetPiece(p);

                        if (taziz.GetCampedTwoTurns())
                        {
                            zeroCost = true;
                        }

                        return true;
                    }
                }
            }
            catch { }
            return false;
        }

        bool zeroCost = false;

        private int CalculateSahmMove(Player CurrentPlayer, int StartRow, int StartColumn)
        {
            int FinishSquareReference = int.Parse(StartRow + "" + StartColumn);
            try
            {
                if (Board[GetIndexOfSquare(FinishSquareReference)].GetPieceInSquare() != null)
                {
                    if (!Board[GetIndexOfSquare(FinishSquareReference)].GetPieceInSquare().GetTypeOfPiece().Equals("mirza"))
                    {
                        Board[GetIndexOfSquare(FinishSquareReference)] = new Square();
                        if (null != Board[GetIndexOfSquare(FinishSquareReference)].GetPieceInSquare())
                            return Board[GetIndexOfSquare(FinishSquareReference)].GetPieceInSquare().GetPointsIfCaptured();
                    }
                }
            }
            catch { }
            return 0;
        }

        private void UpdateBoard(int StartSquareReference, int FinishSquareReference)
        {
            Board[GetIndexOfSquare(FinishSquareReference)].SetPiece(Board[GetIndexOfSquare(StartSquareReference)].RemovePiece());
        }

        private void DisplayFinalResult()
        {
            if (Players[0].GetScore() == Players[1].GetScore())
            {
                Console.WriteLine("Draw!");
            }
            else if (Players[0].GetScore() > Players[1].GetScore())
            {
                Console.WriteLine(Players[0].GetName() + " is the winner!");
            }
            else
            {
                Console.WriteLine(Players[1].GetName() + " is the winner!");
            }
        }

        Taziz taziz;

        private void CreateBoard()
        {
            Square S;
            Board = new List<Square>();

            Piece p = new Piece("taziz");

            taziz = new Taziz(p);

            for (int Row = 1; Row <= NoOfRows; Row++)
            {
                for (int Column = 1; Column <= NoOfColumns; Column++)
                {

                    if(NoOfColumns % 2 == 0) //even number
                    {

                        if (Row == NoOfRows / 2 && Column == NoOfColumns / 2)
                        {

                            Board.Add(taziz);
                            continue;
                        }

                        if (Row == 1 && Column == NoOfColumns / 2)
                        {
                            S = new Kotla(Players[0], "K");
                        }
                        else if (Row == NoOfRows && Column == NoOfColumns / 2 + 1)
                        {
                            S = new Kotla(Players[1], "k");
                        }
                        else
                        {
                            S = new Square();
                        }
                    }
                    else //odd
                    {

                        if (Row == NoOfRows / 2 && Column == NoOfColumns / 2 + 1)
                        {
                            Board.Add(taziz);
                            continue;
                        }

                        if (Row == 1 && Column == NoOfColumns / 2 +1)
                        {
                            S = new Kotla(Players[0], "K");
                        }
                        else if (Row == NoOfRows && Column == NoOfColumns / 2 +1)
                        {
                            S = new Kotla(Players[1], "k");
                        }
                        else
                        {
                            S = new Square();
                        }
                    }

                    Board.Add(S);
                }
            }
        }

        private void CreatePieces(int NoOfPieces)
        {
            Piece CurrentPiece;
            for (int Count = 1; Count <= NoOfPieces; Count++)
            {
                CurrentPiece = new Piece("piece", Players[0], 1, "!");
                Board[GetIndexOfSquare(2 * 10 + Count + 1)].SetPiece(CurrentPiece);
            }
            CurrentPiece = new Piece("mirza", Players[0], 5, "1");


            if (NoOfColumns % 2 == 0) //even
            {
                Board[GetIndexOfSquare(10 + NoOfColumns / 2)].SetPiece(CurrentPiece);
            }
            else //odd
            {
                Board[GetIndexOfSquare(10 + NoOfColumns / 2 + 1)].SetPiece(CurrentPiece);
            }
            


            for (int Count = 1; Count <= NoOfPieces; Count++)
            {
                CurrentPiece = new Piece("piece", Players[1], 1, "\"");
                Board[GetIndexOfSquare((NoOfRows - 1) * 10 + Count + 1)].SetPiece(CurrentPiece);
            }
            CurrentPiece = new Piece("mirza", Players[1], 5, "2");
            Board[GetIndexOfSquare(NoOfRows * 10 + (NoOfColumns / 2 + 1))].SetPiece(CurrentPiece);
        }

        private void CreateMoveOptionOffer()
        {
            MoveOptionOffer.Add("sahm");
            MoveOptionOffer.Add("jazair");
            MoveOptionOffer.Add("chowkidar");
            MoveOptionOffer.Add("cuirassier");
            MoveOptionOffer.Add("ryott");


            MoveOptionOffer.Add("faujdar");
            MoveOptionOffer.Add("faris");
            MoveOptionOffer.Add("sarukh");
        }

        

        private MoveOption CreateRyottMoveOption(int Direction)
        {
            MoveOption NewMoveOption = new MoveOption("ryott");
            Move NewMove = new Move(0, 1 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(0, -1 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(1 * Direction, 0);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(-1 * Direction, 0);
            NewMoveOption.AddToPossibleMoves(NewMove);
            return NewMoveOption;
        }

        private MoveOption CreateFaujdarMoveOption(int Direction)
        {
            MoveOption NewMoveOption = new MoveOption("faujdar");
            Move NewMove = new Move(0, -1 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(0, 1 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(0, 2 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(0, -2 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            return NewMoveOption;
        }

        private MoveOption CreateJazairMoveOption(int Direction)
        {
            MoveOption NewMoveOption = new MoveOption("jazair");
            Move NewMove = new Move(2 * Direction, 0);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(2 * Direction, -2 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(2 * Direction, 2 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(0, 2 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(0, -2 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(-1 * Direction, -1 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(-1 * Direction, 1 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            return NewMoveOption;
        }


        private MoveOption CreateCuirassierMoveOption(int Direction)
        {
            MoveOption NewMoveOption = new MoveOption("cuirassier");
            Move NewMove = new Move(1 * Direction, 0);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(2 * Direction, 0);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(1 * Direction, -2 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(1 * Direction, 2 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            return NewMoveOption;
        }

        private MoveOption CreateChowkidarMoveOption(int Direction)
        {
            MoveOption NewMoveOption = new MoveOption("chowkidar");
            Move NewMove = new Move(1 * Direction, 1 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(1 * Direction, -1 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(-1 * Direction, 1 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(-1 * Direction, -1 * Direction);
            NewMove = new Move(-1 * Direction, -1 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(0, 2 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(0, -2 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            return NewMoveOption;
        }

        private MoveOption CreateFarisMoveOption(int Direction)
        {
            MoveOption NewMoveOption = new MoveOption("faris");

            Move NewMove = new Move(2 * Direction,1 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);

            NewMove = new Move(2 * Direction, -1 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);

            NewMove = new Move(-2 * Direction, 1 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);

            NewMove = new Move(-2 * Direction, -1 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);

            NewMove = new Move(1 * Direction, 2 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);

            NewMove = new Move(-1 * Direction, 2 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);

            NewMove = new Move(1 * Direction, -2 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);

            NewMove = new Move(-1 * Direction, -2 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);

            return NewMoveOption;
        }

        private MoveOption CreateSarukhMoveOption(int Direction)
        {
            MoveOption NewMoveOption = new MoveOption("sarukh");

            Move NewMove = new Move(0, 1 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);

            NewMove = new Move(0, -1 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);

            NewMove = new Move(1 * Direction, 1 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);

            NewMove = new Move(1 * Direction, -1 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);

            NewMove = new Move(-1 * Direction, 1 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(-1 * Direction, -1 * Direction);
            NewMoveOption.AddToPossibleMoves(NewMove);

            NewMove = new Move(2 * Direction, 0);
            NewMoveOption.AddToPossibleMoves(NewMove);
            NewMove = new Move(-2 * Direction, 0);
            NewMoveOption.AddToPossibleMoves(NewMove);

            return NewMoveOption;
        }

        private MoveOption CreateSahmMoveOption(int Direction)
        {
            MoveOption NewMoveOption = new MoveOption("sahm");

            Move NewMove = new Move(0, 0);
            NewMoveOption.AddToPossibleMoves(NewMove);

            return NewMoveOption;
        }



        private MoveOption CreateMoveOption(string Name, int Direction)
        {
            if (Name == "chowkidar")
            {
                return CreateChowkidarMoveOption(Direction);
            }
            else if (Name == "ryott")
            {
                return CreateRyottMoveOption(Direction);
            }
            else if (Name == "faujdar")
            {
                return CreateFaujdarMoveOption(Direction);
            }
            else if (Name == "jazair")
            {
                return CreateJazairMoveOption(Direction);
            }
            else if (Name == "faris")
            {
                return CreateFarisMoveOption(Direction);
            }
            else if (Name == "sarukh")
            {
                return CreateSarukhMoveOption(Direction);
            }
            else if (Name == "sahm")
            {
                return CreateSahmMoveOption(Direction);
            }
            else
            {
                return CreateCuirassierMoveOption(Direction);
            }
        }

        private void CreateMoveOptions()
        {

            Players[0].AddToMoveOptionQueue(CreateMoveOption("ryott", 1)); // positive 1 means going down
            Players[0].AddToMoveOptionQueue(CreateMoveOption("sarukh", 1));
            Players[0].AddToMoveOptionQueue(CreateMoveOption("chowkidar", 1));
            Players[0].AddToMoveOptionQueue(CreateMoveOption("sahm", 1));
            Players[0].AddToMoveOptionQueue(CreateMoveOption("faris", 1));
            Players[0].AddToMoveOptionQueue(CreateMoveOption("cuirassier", 1));
            Players[0].AddToMoveOptionQueue(CreateMoveOption("faujdar", 1));
            Players[0].AddToMoveOptionQueue(CreateMoveOption("jazair", 1));
            Players[0].AddToMoveOptionQueue(CreateMoveOption("spy", 1));

            Players[1].AddToMoveOptionQueue(CreateMoveOption("ryott", -1));
            Players[1].AddToMoveOptionQueue(CreateMoveOption("sarukh", -1));
            Players[1].AddToMoveOptionQueue(CreateMoveOption("chowkidar", -1));
            Players[1].AddToMoveOptionQueue(CreateMoveOption("sahm", -1));
            Players[1].AddToMoveOptionQueue(CreateMoveOption("faris", -1));
            Players[1].AddToMoveOptionQueue(CreateMoveOption("cuirassier", -1));
            Players[1].AddToMoveOptionQueue(CreateMoveOption("faujdar", -1));
            Players[1].AddToMoveOptionQueue(CreateMoveOption("jazair", -1));
            Players[1].AddToMoveOptionQueue(CreateMoveOption("spy", -1));
        }
    }

    class Piece
    {
        protected string TypeOfPiece, Symbol;
        protected int PointsIfCaptured;
        protected Player BelongsTo;

        public Piece(string T)
        {
            TypeOfPiece = T;
        }

        public Piece(string T, Player B, int P, string S)
        {
            TypeOfPiece = T;
            BelongsTo = B;
            PointsIfCaptured = P;
            Symbol = S;
        }

        public void SetSymbol(string symbol)
        {
            Symbol = symbol;
        }

        public string GetSymbol()
        {
            return Symbol;
        }

        public string GetTypeOfPiece()
        {
            return TypeOfPiece;
        }

        public Player GetBelongsTo()
        {
            return BelongsTo;
        }

        public int GetPointsIfCaptured()
        {
            return PointsIfCaptured;
        }
    }

    class Square
    {
        protected string Symbol;
        protected Piece PieceInSquare;
        protected Player BelongsTo;

        public Square()
        {
            PieceInSquare = null;
            BelongsTo = null;
            Symbol = " ";
        }

        public bool ContainsBarrier()
        {
            return (Symbol == "B" || Symbol == "b");
        }

        public virtual bool GetCampedTwoTurns()
        {
            return false;
        }

        public virtual void SetPiece(Piece P)
        {
            PieceInSquare = P;
        }

        public virtual Piece RemovePiece()
        {
            Piece PieceToReturn = PieceInSquare;
            PieceInSquare = null;
            return PieceToReturn;
        }

        public virtual Piece GetPieceInSquare()
        {
            return PieceInSquare;
        }

        public virtual string GetSymbol()
        {
            return Symbol;
        }

        public virtual int GetPointsForOccupancy(Player CurrentPlayer)
        {
            return 0;
        }

        public virtual Player GetBelongsTo()
        {
            return BelongsTo;
        }

        public virtual bool ContainsKotla()
        {
            if (Symbol == "K" || Symbol == "k")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    class Taziz: Square
    {
        private int CampedTurns = 0;

        public void IncrementCampedTurns()
        {
            CampedTurns++;
        }

        public void ResetCampedTurns()
        {
            CampedTurns = 0;
        }

        public Taziz(Piece p) : base()
        {
            BelongsTo = p.GetBelongsTo();
            SetPiece(p);
        }

        public override void SetPiece(Piece P)
        {
            PieceInSquare = P;
            if (P.GetBelongsTo() == null) P.SetSymbol("x");
            else
            {
                IncrementCampedTurns();
                if (P.GetBelongsTo().GetDirection() == 1)
                    P.SetSymbol("A");
                else P.SetSymbol("a");
            }
        }

        public override Piece RemovePiece()
        {
            Piece PieceToReturn = PieceInSquare;
            PieceInSquare = null;
            PieceToReturn.SetSymbol("x");
            return PieceToReturn;
        }

        public override bool GetCampedTwoTurns()
        {
            return CampedTurns == 4 ? true : false;
        }

    }

    class Barrier: Square
    {
        public Barrier(Player P) : base()
        {
            BelongsTo = P;
            if (P.GetDirection() == 1)
                Symbol = "B";
            else Symbol = "b";
        }
    }

    class Kotla : Square
    {
        public Kotla(Player P, string S) : base()
        {
            BelongsTo = P;
            Symbol = S;
        }

        public override int GetPointsForOccupancy(Player CurrentPlayer)
        {
            if (PieceInSquare == null)
            {
                return 0;
            }
            else if (BelongsTo.SameAs(CurrentPlayer))
            {
                if (CurrentPlayer.SameAs(PieceInSquare.GetBelongsTo()) && (PieceInSquare.GetTypeOfPiece() == "piece" || PieceInSquare.GetTypeOfPiece() == "mirza"))
                {
                    return 5;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                if (CurrentPlayer.SameAs(PieceInSquare.GetBelongsTo()) && (PieceInSquare.GetTypeOfPiece() == "piece" || PieceInSquare.GetTypeOfPiece() == "mirza"))
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
    }

    class MoveOption 
    {
        protected string Name;
        protected List<Move> PossibleMoves;

        public MoveOption(string N)
        {
            Name = N;
            PossibleMoves = new List<Move>();
        }

        public void AddToPossibleMoves(Move M)
        {
            PossibleMoves.Add(M);
        }

        public string GetName()
        {
            return Name;
        }

        public bool CheckIfThereIsAMoveToSquare(int StartSquareReference, int FinishSquareReference)
        {
            int StartRow = StartSquareReference / 10;
            int StartColumn = StartSquareReference % 10;
            int FinishRow = FinishSquareReference / 10;
            int FinishColumn = FinishSquareReference % 10;
            foreach (var M in PossibleMoves)
            {
                if (StartRow + M.GetRowChange() == FinishRow && StartColumn + M.GetColumnChange() == FinishColumn)
                {
                    return true;
                }
            }
            return false;
        }
    }

    class Move
    {
        protected int RowChange, ColumnChange;

        public Move(int R, int C)
        {
            RowChange = R;
            ColumnChange = C;
        }

        public int GetRowChange()
        {
            return RowChange;
        }

        public int GetColumnChange()
        {
            return ColumnChange;
        }
    }

    class MoveOptionQueue
    {
        private List<MoveOption> Queue = new List<MoveOption>();

        public void ReverseQueue()
        {
            Queue.Reverse();
        }

        public void SwapFirstAndLast()
        {
            MoveOption lastMoveOption = Queue[Queue.Count-1];
            Queue[Queue.Count - 1] = Queue[0];
            Queue[0] = lastMoveOption;
        }

        public void MoveItemToFront(int position)
        {
            MoveOption lastMoveOption = Queue[Queue.Count - 1];
            Queue[Queue.Count - 1] = Queue[position];
            Queue[position] = lastMoveOption;
        }

        public void ResetQueueBack(int Position) 
        {
            MoveOption lastElement = Queue[Queue.Count - 1]; 
            Queue.RemoveAt(Queue.Count - 1);
            Queue.Insert(Position, lastElement);
        }

        public string GetQueueAsString()
        {
            string QueueAsString = "";
            int Count = 1;
            foreach (var M in Queue)
            {
                QueueAsString += Count.ToString() + ". " + M.GetName() + "   ";
                Count += 1;
            }
            return QueueAsString;
        }

        public void Add(MoveOption NewMoveOption)
        {
            Queue.Add(NewMoveOption);
        }

        public void Replace(int Position, MoveOption NewMoveOption)
        {
            Queue[Position] = NewMoveOption;
        }

        public void MoveItemToBack(int Position)
        {
            MoveOption Temp = Queue[Position];
            Queue.RemoveAt(Position);
            Queue.Add(Temp);
        }

        public MoveOption GetMoveOptionInPosition(int Pos)
        {
            return Queue[Pos];
        }
    }

    class Player
    {
        private string Name;
        private int Direction, Score;
        private MoveOptionQueue Queue = new MoveOptionQueue();

        private bool hasPlacedBarrier = false;

        public void SetPlacedBarrier()
        {
            hasPlacedBarrier = true;
        }

        public bool HasPlacedBarrier()
        {
            return hasPlacedBarrier;
        }

        public void SwapFirstAndLast()
        {
            Queue.SwapFirstAndLast();
        }

        public void MoveItemToFront(int position)
        {
            Queue.MoveItemToFront(position);
        }

        public void ReversePlayerQueue()
        {
            Queue.ReverseQueue();
        }

        public void ReplaceQueue(MoveOptionQueue Queue)
        {
            this.Queue = Queue;
        }

        public MoveOptionQueue GetMoveOptionQueue()
        {
            return Queue;
        }

        private bool hasSetSecondKotla = false;

        public bool GetSecondKotlaState()
        {
            return hasSetSecondKotla;
        }

        public void SetSecondKotla()
        {
            hasSetSecondKotla = true;
        }

        private bool SahmUsed = false;

        public bool GetSahmStatus()
        {
            return SahmUsed;
        }

        public void SetSahmUsed()
        {
            SahmUsed = true;
        }

        public bool ChoiceIsSahm(int Pos)
        {
            MoveOption Temp = Queue.GetMoveOptionInPosition(Pos - 1);
            return Temp.GetName().Equals("sahm");
        }

        int ChoiceOptionsLeft = 3;

        public void ResetQueueBackAfterUndo(int Position)
        {
            Queue.ResetQueueBack(Position);
        }

        public int GetChoiceOptionsLeft()
        {
            return ChoiceOptionsLeft;
        }

        public void DecreaseChoiceOptionsLeft()
        {
            if (ChoiceOptionsLeft > 0) 
                ChoiceOptionsLeft--;
        }

        private bool WafrAwarded = false;

        public bool GetWafrAwarded()
        {
            return WafrAwarded;
        }

        public void SetWafrAwarded(bool WafrAwarded)
        {
            this.WafrAwarded = WafrAwarded;
        }

        public string GetJustQueueAsString()
        {
            return Queue.GetQueueAsString();
        }

        public Player(string N, int D)
        {
            Score = 100;
            Name = N;
            Direction = D;
        }

        public bool SameAs(Player APlayer)
        {
            if (APlayer == null)
            {
                return false;
            }
            else if (APlayer.GetName() == Name)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetPlayerStateAsString()
        {
            return Name + Environment.NewLine + "Score: " + Score.ToString() + Environment.NewLine + "Move option queue: " + Queue.GetQueueAsString() + Environment.NewLine;
        }

        public void AddToMoveOptionQueue(MoveOption NewMoveOption)
        {
            Queue.Add(NewMoveOption);
        }

        public void UpdateQueueAfterMove(int Position)
        {
            Queue.MoveItemToBack(Position - 1);
        }

        public void UpdateMoveOptionQueueWithOffer(int Position, MoveOption NewMoveOption)
        {
            Queue.Replace(Position, NewMoveOption);
        }

        public int GetScore()
        {
            return Score;
        }

        public string GetName()
        {
            return Name;
        }

        public int GetDirection()
        {
            return Direction;
        }

        public void ChangeScore(int Amount)
        {
            Score += Amount;
        }

        public bool CheckPlayerMove(int Pos, int StartSquareReference, int FinishSquareReference)
        {
            MoveOption Temp = Queue.GetMoveOptionInPosition(Pos - 1);
            return Temp.CheckIfThereIsAMoveToSquare(StartSquareReference, FinishSquareReference);
        }
    }



    class vehicule //base class
    {
        public int speed = 0;
        
    }

    class car : vehicule
    {
        //diffrent behavior
        public car() : base()
        {
            this.speed = 100;
        }
    }


}
