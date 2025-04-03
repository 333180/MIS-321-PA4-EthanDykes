using System;
using System.Threading;
using PirateBattleGame.Characters;

namespace PirateBattleGame
{
    public static class GUI
    {
        public static readonly int BoxLeft = 20;
        public static readonly int BoxTop = 5;
        public static readonly int BoxWidth = 40;
        private static readonly int BoxHeight = 20;
        public static readonly int InnerWidth = BoxWidth - 2;

        public static void DrawBox(int left = 10, int top = 5, int width = 60, int height = 20, 
                             char topLeft = '╔', char topRight = '╗', 
                             char bottomLeft = '╚', char bottomRight = '╝',
                             char horizontal = '═', char vertical = '║')
        {
            // Draw top border
            Console.SetCursorPosition(left, top);
            Console.Write(topLeft + new string(horizontal, width - 2) + topRight);
            
            // Draw sides
            for (int i = 1; i < height; i++)
            {
                Console.SetCursorPosition(left, top + i);
                Console.Write(vertical + new string(' ', width - 2) + vertical);
            }
            
            // Draw bottom border
            Console.SetCursorPosition(left, top + height);
            Console.Write(bottomLeft + new string(horizontal, width - 2) + bottomRight);
        }

        public static void CenterText(string text, int row, int boxLeft = 20, int innerWidth = 38)
        {
            int textLength = text.Length;
            
            int startPos = boxLeft + (innerWidth - textLength) / 2;
            
            if (startPos < boxLeft + 1)
            {
                startPos = boxLeft + 1;
            }
            
            Console.SetCursorPosition(startPos, BoxTop + row); 
            Console.Write(text);
        }

        public static void ClearLine(int row, int boxLeft = 20, int innerWidth = 38)
        {
            Console.SetCursorPosition(boxLeft + 1, BoxTop + row);
            Console.Write(new string(' ', innerWidth));
        }
        public static void DrawHealthBar(Character player, int row)
        {
            CenterText($"{player.Name} {player.GetHealthBar()}", row);
        }
        public static void AnimateText(string text, int row, int delay = 50)
        {
            // Center position calculation
            int centerPos = (Console.WindowWidth - text.Length) / 2;
            Console.SetCursorPosition(centerPos, row);
            
            // Type out the text with animation
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }
        }
        public static void DrawBattleScreen(Character player1, Character player2)
        {
            Console.Clear();
            Console.CursorVisible = false;
            
            DrawBox();
            
            CenterText("Pirate Battle!", 2);
            
            CenterText(new string('─', InnerWidth), 3);
            
            DrawHealthBar(player1, 5);
            DrawHealthBar(player2, 6);
            
            CenterText("Press any key to attack...", 18);
            
            DrawBox(10, 26, 60, 6, '┌', '┐', '└', '┘', '─', '│');
            CenterText("Turn History", 22, 20, 38);
            CenterText("────────────", 23, 20, 38);
        }
        
        public static void TypeText(string text, int row, int delay = 30)
        {
            int startPos = BoxLeft + (InnerWidth - text.Length) / 2;
            if (startPos < BoxLeft + 1)
            {
                startPos = BoxLeft + 1;
            }
            
            Console.SetCursorPosition(startPos, BoxTop + row);
            
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }
        }
        
        public static void WaitForKey(string message = "Press any key to continue...")
        {
            CenterText(message, BoxHeight - 2);
            Console.ReadKey(true);
        }
        
        public static void DisplayTurnHistory(Queue<TurnResult> turnHistory)
        {
            int row = 24;
            
            for (int i = 0; i < 3; i++)
            {
                ClearLine(row + i, 20, 38);
            }
            
            foreach (var turn in turnHistory)
            {
                CenterText($"{turn}", row++, 20, 38);
            }
        }
    }
}