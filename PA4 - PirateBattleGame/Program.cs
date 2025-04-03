using PirateBattleGame.Attacks;
using PirateBattleGame.Characters;
using System;
using System.Collections.Generic;

namespace PirateBattleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.Clear();
            
            DisplayTitleScreen();
            
            var game = new Game();
            game.Start();
        }
        
        static void DisplayTitleScreen()
        {
            string titleLogo = @"
                  _                   _
                _( )                 ( )_
               (_, |      __ __      | ,_)
                  \'\    /  ^  \    /'/
                   '\'\,/\      \,/'/'
                     '\| []   [] |/'
                       (_  /^\  _)
                         \  ~  /
                         /HHHHH\
                       /'/{^^^}\'\
                   _,/'/'  ^^^  '\'\,_
                  (_, |           | ,_)
                    (_)           (_)
";
            Console.WriteLine(titleLogo);
            GUI.AnimateText("PIRATE BATTLE!", 10,20);
            GUI.AnimateText("A Game of Skill and Chance", 12, 20);
            GUI.AnimateText("Press any key to set sail...", 15, 20);
            Console.ReadKey(true);
            Console.Clear();
        }

    }

public class Game
    {
        private Character? player1;
        private Character? player2;
        private readonly Random random = new Random();
        private readonly Queue<TurnResult> turnHistory = new Queue<TurnResult>(3);

        public void Start()
        {
            try 
            {
                SetupPlayers();
                BattleLoop();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void SetupPlayers()
        {
            Console.Clear();
            GUI.DrawBox();
            GUI.CenterText("Welcome to Pirate Battle!", 2);
            GUI.CenterText("--------------------------------", 3);

            // Player 1 setup
            GUI.CenterText("Player 1, enter your name:", 6);
            
            int inputRow = 7;
            int centerX = GUI.BoxLeft + (GUI.BoxWidth - 20) / 2;
            Console.SetCursorPosition(centerX, GUI.BoxTop + inputRow);
            Console.CursorVisible = true;
            
            string? name1 = Console.ReadLine();
            Console.CursorVisible = false;
            player1 = SelectCharacter(name1 ?? "Player 1");

            // Player 2 setup
            GUI.ClearLine(6);
            GUI.ClearLine(7);
            GUI.ClearLine(8);
            GUI.CenterText("Player 2, enter your name:", 6);
            
            Console.SetCursorPosition(centerX, GUI.BoxTop + inputRow);
            Console.CursorVisible = true;
            
            string? name2 = Console.ReadLine();
            Console.CursorVisible = false;
            player2 = SelectCharacter(name2 ?? "Player 2");

            GUI.TypeText("Let the battle begin!", 10);
            GUI.WaitForKey();
        }

        private Character SelectCharacter(string playerName)
        {
            Console.Clear();
            GUI.DrawBox();
            GUI.CenterText($"{playerName}, choose your character:", 2);
            GUI.CenterText("--------------------------------", 3);
            
            GUI.CenterText("1. Jack Sparrow (Distract)", 6);
            GUI.CenterText("2. Will Turner (Sword)", 7);
            GUI.CenterText("3. Davy Jones (Cannon)", 8);
            
            GUI.CenterText("Enter choice (1-3):", 11);
            
            int choice = 0;
            while (choice < 1 || choice > 3)
            {
                Console.SetCursorPosition(38, 11);
                ConsoleKeyInfo key = Console.ReadKey(true);
                
                if (char.IsDigit(key.KeyChar))
                {
                    int.TryParse(key.KeyChar.ToString(), out choice);
                    if (choice >= 1 && choice <= 3)
                    {
                        GUI.CenterText(key.KeyChar.ToString(), 11, 38, 1);
                        break;
                    }
                }
            }
            
            return choice switch
            {
                1 => new JackSparrow(playerName),
                2 => new WillTurner(playerName),
                3 => new DavyJones(playerName),
                _ => throw new InvalidOperationException()
            };
        }

        private void BattleLoop()
        {
            if (player1 == null || player2 == null) return;

            Character attacker = random.Next(2) == 0 ? player1 : player2;
            Character defender = attacker == player1 ? player2 : player1;

            while (player1.Health > 0 && player2.Health > 0)
            {
                Console.Clear();
                GUI.DrawBattleScreen(player1, player2);
                
                GUI.CenterText($"{attacker.Name}'s turn!", 8);
                GUI.CenterText($"Stats: {attacker.GetStats()}", 9);
                
                GUI.CenterText($"{defender.Name} is defending...", 11);
                GUI.CenterText($"Stats: {defender.GetStats()}", 12);
                
                GUI.DisplayTurnHistory(turnHistory);
                
                GUI.CenterText("Press any key to attack...", 18);
                Console.ReadKey(true);

                int baseDamage = attacker.PerformAttack();
                double typeBonus = attacker.CalculateTypeBonus(defender);
                
                int finalDamage = (int)(baseDamage * typeBonus);
                bool isCrit = baseDamage > 15; 
                bool isMiss = baseDamage == 0; 
                
                string attackMessage;

                if (isMiss)
                {
                    attackMessage = $"{attacker.Name}'s {attacker.GetAttackName()} missed!";
                }
                else
                {
                    if (isCrit)
                    {
                        attackMessage = $"{attacker.Name} lands a CRITICAL HIT with {attacker.GetAttackName()}!";
                    }
                    else
                    {
                        attackMessage = $"{attacker.Name} attacks with {attacker.GetAttackName()}!";
                    }
                }

                var result = new TurnResult(
                    attacker.Name,
                    defender.Name,
                    finalDamage,
                    attacker.GetAttackName(),
                    isCrit,
                    isMiss
                );
                
                turnHistory.Enqueue(result);
                if (turnHistory.Count > 3)
                {
                    turnHistory.Dequeue();
                }

                GUI.DisplayTurnHistory(turnHistory);

                GUI.ClearLine(14);
                GUI.CenterText(attackMessage, 14);
                
                if (!isMiss)
                {
                    GUI.ClearLine(15);
                    GUI.CenterText($"Damage dealt: {finalDamage}", 15);
                    
                    if (isCrit)
                    {
                        GUI.ClearLine(16);
                        GUI.CenterText("*** CRITICAL STRIKE! ***", 16);
                    }
                }

                defender.TakeDamage(finalDamage);
                
                if (defender.Health <= 0) break;

                (attacker, defender) = (defender, attacker);
                
                // Wait for next turn
                GUI.WaitForKey("Press any key for next turn...");
            }

            AnnounceWinner();
        }
        

        private void AnnounceWinner()
        {
            if (player1 == null || player2 == null) return;
            
            Character winner = player1.Health > 0 ? player1 : player2;
            
            Console.Clear();
            GUI.DrawBox();
            GUI.CenterText("====================================", 8);
            GUI.CenterText($"{winner.Name} wins the battle!", 10);
            GUI.CenterText("Thank you for playing!", 12);
            GUI.CenterText("====================================", 14);
            
            GUI.WaitForKey("Press any key to exit...");
        }
    }
}