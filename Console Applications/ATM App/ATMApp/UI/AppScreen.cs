using ATMApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMApp.UI
{
    public class AppScreen
    {
        internal const string cur = "USD ";
        internal static void Welcome()
        {
            // clears the console screen
            Console.Clear();
            // sets the title of the console window
            Console.Title = "My ATM App";
            // sets the text color or foreground color to white
            Console.ForegroundColor = ConsoleColor.White;

            // set the welcome message
            Console.WriteLine("\n\n---------------------------Welcome to My ATM App---------------------------\n\n");
            // prompt the user to insert atm card
            Console.WriteLine("Please insert your ATM card");
            Console.WriteLine("Note: Actual ATM machine will accept and validate a physical ATM card, " +
                "read the card number and validate it.");
            Utility.PressEnterToContinue();
        }

        internal static UserAccount UserLoginForm()
        {
            UserAccount tempUserAccount = new UserAccount();

            tempUserAccount.CardNumber = Validator.Convert<long>("your card number.");
            tempUserAccount.CardPin = Convert.ToInt32(Utility.GetSecretInput("Enter your card PIN"));
            return tempUserAccount;
        }

        internal static void LoginProgress()
        {
            Console.WriteLine("\nChecking card number and PIN...");
            Utility.PrintDotAnimation(10);
        }

        internal static void PrintLockSreen()
        {
            Console.Clear();
            Utility.PrintMessage("Your account is locked. Please go to the nearest " +
                "branch to unlock your account. Thank you.", true);
            Utility.PressEnterToContinue();
            Environment.Exit(1);
        }

        internal static void WelcomeCustomer(string fullName)
        {
            Console.WriteLine($"Welcome back, {fullName}");
            Utility.PressEnterToContinue();
        }

        internal static void DisplayAppMenu()
        {
            Console.Clear();
            Console.WriteLine("------My ATM App Menu------");
            Console.WriteLine(":                         :");
            Console.WriteLine("1. Account Balance        :");
            Console.WriteLine("2. Cash Deposit           :");
            Console.WriteLine("3. Withdrawal             :");
            Console.WriteLine("4. Transfer               :");
            Console.WriteLine("5. Transactions           :");
            Console.WriteLine("6. Logout                 :");
        }

        internal static void LogOutProgress()
        {
            Console.WriteLine("Thank you for using My ATM App.");
            Utility.PrintDotAnimation();
            Console.Clear();
        }

        internal static int SelectAmount()
        {
            Console.WriteLine("");
            Console.WriteLine(":1.{0}20       6.{0}200", cur);
            Console.WriteLine(":2.{0}40       7.{0}300", cur);
            Console.WriteLine(":3.{0}60       8.{0}400", cur);
            Console.WriteLine(":4.{0}80       9.{0}500", cur);
            Console.WriteLine(":5.{0}100     10.{0}1,000", cur);
            Console.WriteLine(":0.Other");
            Console.WriteLine("");

            int selectedAmount = Validator.Convert<int>("option:");
            switch(selectedAmount)
            {
                case 1:
                    return 20;
                    break;
                case 2:
                    return 40;
                    break;
                case 3:
                    return 60;
                    break;
                case 4:
                    return 80;
                    break;
                case 5:
                    return 100;
                    break;
                case 6:
                    return 200;
                    break;
                case 7:
                    return 300;
                    break;
                case 8:
                    return 400;
                    break;
                case 9:
                    return 500;
                    break;
                case 10:
                    return 1000;
                    break;
                case 0:
                    return 0;
                    break;
                default:
                    Utility.PrintMessage("Invalid input. Try again.", false);
                    return -1;
                    break;
            }
        }

        internal InternalTransfer InternalTransferForm()
        {
            var internalTransfer = new InternalTransfer();
            internalTransfer.RecipientBankAccountNumber = Validator.Convert<long>("recipient's account number:");
            internalTransfer.TransferAmount = Validator.Convert<decimal>($"amount {cur}");
            internalTransfer.RecipientBankAccountName = Utility.GetUserInput("recipient's name:");
            return internalTransfer;
        }
    }
}
