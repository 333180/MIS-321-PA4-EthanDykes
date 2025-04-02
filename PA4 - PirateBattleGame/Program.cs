﻿using PirateBattleGame.Attacks;
using PirateBattleGame.Characters;
using System;
using System.Collections.Generic;

namespace PirateBattleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("🏴‍☠️ Welcome to Pirate Battle! 🏴‍☠️");
            Console.WriteLine("--------------------------------");
            
            var game = new Game();
            game.Start();
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
            Console.WriteLine("\nPlayer 1, enter your name:");
            string? name1 = Console.ReadLine();
            player1 = SelectCharacter(name1 ?? "Player 1");

            Console.WriteLine("\nPlayer 2, enter your name:");
            string? name2 = Console.ReadLine();
            player2 = SelectCharacter(name2 ?? "Player 2");

            Console.WriteLine("\nLet the battle begin!");
        }

        private Character SelectCharacter(string playerName)
        {
            Console.WriteLine($"\n{playerName}, choose your character:");
            Console.WriteLine("1. Jack Sparrow (Distract)");
            Console.WriteLine("2. Will Turner (Sword)");
            Console.WriteLine("3. Davy Jones (Cannon)");
            
            while (true)
            {
                Console.Write("Enter choice (1-3): ");
                if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= 3)
                {
                    return choice switch
                    {
                        1 => new JackSparrow(playerName),
                        2 => new WillTurner(playerName),
                        3 => new DavyJones(playerName),
                        _ => throw new InvalidOperationException()
                    };
                }
                Console.WriteLine("Invalid choice. Please enter 1, 2, or 3.");
            }
        }

        private void BattleLoop()
{
    if (player1 == null || player2 == null) return;

    Character attacker = random.Next(2) == 0 ? player1 : player2;
    Character defender = attacker == player1 ? player2 : player1;

    while (player1.Health > 0 && player2.Health > 0)
    {
        Console.Clear();
        Console.WriteLine("\n====================================");
        DisplayHealthBars();
        
        Console.WriteLine($"\n{attacker.Name}'s turn!");
        Console.WriteLine($"Stats: {attacker.GetStats()}");
        
        Console.WriteLine($"\n{defender.Name} is defending...");
        Console.WriteLine($"Stats: {defender.GetStats()}");
        
        DisplayTurnHistory();
        
        Console.WriteLine("\nPress any key to attack...");
        Console.ReadKey();

        var (finalDamage, isCrit, isMiss) = attacker.PerformAttack(defender);
        string attackMessage;

        if (isMiss)
        {
            attackMessage = $"⚔️ {attacker.Name}'s {attacker.GetAttackName()} missed!";
        }
        else
        {
            if (isCrit)
            {
                attackMessage = $"⚔️ {attacker.Name} lands a CRITICAL HIT with {attacker.GetAttackName()}!";
            }
            else
            {
                attackMessage = $"⚔️ {attacker.Name} attacks with {attacker.GetAttackName()}!";
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

        Console.WriteLine(attackMessage);
        if (!isMiss)
        {
            Console.WriteLine($"💥 Damage dealt: {finalDamage}");
            if (isCrit)
            {
                Console.WriteLine("💥💥💥 CRITICAL STRIKE! 💥💥💥");
            }
        }

        if (defender.Health <= 0) break;

        (attacker, defender) = (defender, attacker);
    }

    AnnounceWinner();
}        private void DisplayTurnHistory()
        {
            if (turnHistory.Count == 0) return;
            
            Console.WriteLine("\nLast turns:");
            foreach (var turn in turnHistory)
            {
                Console.WriteLine($"- {turn}");
            }
        }

        private void DisplayHealthBars()
        {
            if (player1 == null || player2 == null) return;
            
            Console.WriteLine($"{player1.Name} {player1.GetHealthBar()}");
            Console.WriteLine($"{player2.Name} {player2.GetHealthBar()}");
        }

        private void AnnounceWinner()
        {
            if (player1 == null || player2 == null) return;
            
            Character winner = player1.Health > 0 ? player1 : player2;
            Console.WriteLine("\n====================================");
            Console.WriteLine($"🏆 {winner.Name} wins the battle! 🏆");
            Console.WriteLine("====================================");
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}