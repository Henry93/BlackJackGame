using System;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;

namespace BlackJackGame
{
    public enum Face
    {
        Ace,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
    };

    public class Card
    {
        public Face Cards {get; set;}
        public int Value {get; set;}
    }

    // Deck represents a value of cards
    public class Deck
    {
        public List<Card> cards;

        public Deck()
        {
            this.Initialize();
        }

        public void Initialize()
        {
            cards = new List<Card>();

            for (int i = 0; i < 13; i++)
            {
                cards.Add(new Card() {Cards = (Face)i});

                if (i <= 8)
                {
                    cards[cards.Count - 1].Value = i + 1; // make values add up from 0 to 9
                }
                else
                {
                    cards[cards.Count - 1].Value = 10; // values > 9 will have value of 10
                }
            }
        }

        // Returns a shuffled deck of random cards
        public void ShuffleCards()
        {
            Random random = new Random();

            int n = cards.Count;
            while (n > 1)
            {
                n--;
                int c = random.Next(n + 1);
                Card card = cards[c];
                cards[c] = cards[n];
                cards[n] = card;
            }
        }

        public Card DrawACard()
        {
            if (cards.Count <= 0)
            {
                this.Initialize();
                this.ShuffleCards();
            }

            Card cardToReturn = cards[cards.Count - 1];
            cards.RemoveAt(cards.Count - 1);
            return cardToReturn; // reduces the size of the deck as the user plays
            
        }

        public int RemainingCards()
        {
            return cards.Count;
        }

        public void ShowDeck()
        {
            int i = 1;
            foreach (Card card in cards)
            {
                System.Console.WriteLine("Card {0}: {1}. Value: {2}", i, card.Cards, card.Value);
                i++;
            }
        }
    }
    public class Program
    {

        public static Deck deck;
        public static List<Card> userHand;
        public static List<Card> dealerHand;

        public static void Main(string[] args)
        {
            deck = new Deck();
            deck.ShuffleCards();
            dealHand();
        }

        public static void dealHand()
        {
            while (true)
            {
                System.Console.WriteLine("Would you like to play BlackJack? y/n");
                string response = System.Console.ReadLine();
                if (response == "y")
                {

                    if (deck.RemainingCards() < 20)
                    {
                        deck.Initialize();
                        deck.ShuffleCards();
                    }

                    userHand = new List<Card>();
                    userHand.Add(deck.DrawACard());
                    userHand.Add(deck.DrawACard());

                    var aceCount = 0;

                    foreach (Card card in userHand)
                    {
                        // Let the player choose the value of Ace
                        if (card.Cards == Face.Ace)
                        {
                            while (true)
                            {
                                System.Console.WriteLine("You got an Ace! Would you like to set its value to 1 or 11?");
                                string reply = System.Console.ReadLine();

                                if (reply == "1")
                                {
                                    card.Value = 1;
                                    aceCount++;
                                    break;
                                }
                                else if (reply == "11")
                                {
                                    card.Value = 11;
                                    aceCount++;
                                    break;
                                }
                                else
                                {
                                    System.Console.WriteLine("Wrong value, please try again!");
                                    continue;
                                }
                            }
                        }
                    }

                    System.Console.WriteLine("\n<PLAYER>\n");
                    System.Console.WriteLine("Card 1: {0}", userHand[0].Cards);
                    System.Console.WriteLine("Card 2: {0}", userHand[1].Cards);
                    System.Console.WriteLine("Total: {0}\n", userHand[0].Value + userHand[1].Value);

                    dealerHand = new List<Card>();
                    dealerHand.Add(deck.DrawACard());
                    dealerHand.Add(deck.DrawACard());

                    System.Console.WriteLine("<DEALER>\n");
                    System.Console.WriteLine("Card 1: {0}", dealerHand[0].Cards);
                    System.Console.WriteLine("Card 2: [hole card]");
                    System.Console.WriteLine("Total: {0}\n", dealerHand[0].Value);

                    while (true)
                    {

                        System.Console.WriteLine("Hit or stand? h/s");

                        string answer = System.Console.ReadLine();

                        if (answer == "h")
                        {
                            userHand.Add(deck.DrawACard());
                            System.Console.WriteLine("Hitted: {0}", userHand[userHand.Count - 1].Cards);
                            int totalCardsValue = 0;

                            foreach (Card card in userHand)
                            {
                                // var result = card.Cards;
                                // result += 1;
                                // System.Console.WriteLine(result);
                                
                                if (aceCount == 0)
                                {
                                    while (card.Cards == Face.Ace && aceCount == 0)
                                    {
                                        System.Console.WriteLine("You got an Ace! Would you like to set its value to 1 or 11?");
                                        string reply = System.Console.ReadLine();

                                        if (reply == "1")
                                        {
                                            card.Value = 1;
                                            totalCardsValue += card.Value;
                                            totalCardsValue -= 1;
                                            aceCount++;
                                            break;
                                        }
                                        else if (reply == "11")
                                        {
                                            card.Value = 11;
                                            totalCardsValue += card.Value;
                                            totalCardsValue -= 10;
                                            aceCount++;
                                            break;
                                        }
                                        else
                                        {
                                            System.Console.WriteLine("Wrong value, please try again!");
                                            continue;
                                        }
                                    }
                                }

                                var faceValue = (int)card.Value;
                                if (faceValue < 10)
                                {
                                    totalCardsValue += faceValue;
                                }
                                else
                                {
                                    totalCardsValue += 10;
                                }
                            }

                            System.Console.WriteLine("Total card value now is: {0}", totalCardsValue);

                            if (totalCardsValue > 21)
                            {
                                System.Console.WriteLine("You busted! Dealer won the game!");
                                break;
                            }
                            else if (totalCardsValue == 21)
                            {
                                System.Console.WriteLine("You got a BlackJack! Good job!");
                                continue;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else if (answer == "s")
                        {
                            System.Console.WriteLine("\n<DEALER>\n");
                            System.Console.WriteLine("Card 1: {0}", dealerHand[0].Cards);
                            System.Console.WriteLine("Card 2: {0}", dealerHand[1].Cards);

                            // Dealer card value
                            int dealerCardValue = 0;
                            
                            foreach (Card card in dealerHand)
                            {
                                dealerCardValue += card.Value;
                            }
                            // While card value is below 17, it will add up the values
                            while (dealerCardValue < 17)
                            {
                                dealerHand.Add(deck.DrawACard());
                                dealerCardValue = 0;
                                foreach (Card card in dealerHand)
                                {
                                    dealerCardValue += card.Value;
                                }
                                System.Console.WriteLine("Card {0}: {1}", dealerHand.Count, dealerHand[dealerHand.Count - 1].Cards);
                            }
                            dealerCardValue = 0;
                            foreach (Card card in dealerHand)
                            {
                                dealerCardValue += card.Value;
                            }
                            System.Console.WriteLine("Total: {0}\n", dealerCardValue);

                            if (dealerCardValue > 21)
                            {
                                System.Console.WriteLine("Dealer busted, you won!!!");
                                break;
                            }
                            else
                            {
                                int playersCardValue = 0;
                                foreach (Card card in userHand)
                                {
                                    playersCardValue += card.Value;
                                }

                                if (dealerCardValue == playersCardValue)
                                {
                                    System.Console.WriteLine("Player and dealer have tied, no winner!!!");
                                    break;
                                }
                                
                                if (dealerCardValue > playersCardValue)
                                {
                                    System.Console.WriteLine("Dealer has {0} and player has {1}, dealer won!!!", dealerCardValue, playersCardValue);
                                    break;
                                }
                                else
                                {
                                    System.Console.WriteLine("Player has {0} and dealer has {1}, you won!!!", playersCardValue, dealerCardValue);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            System.Console.WriteLine("Wrong answer, please try again!");
                            continue;
                        }
                    }
                }
                else if (response == "n")
                {
                    System.Console.WriteLine("See you later!");
                }
                else
                {
                    System.Console.WriteLine("Incorrect response, please try again!");
                    continue;
                }

                System.Console.ReadLine();
            
            }
        }
    }
}
