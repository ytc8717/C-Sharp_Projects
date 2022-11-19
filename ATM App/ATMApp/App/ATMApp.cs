
using ATMApp.Domain.Entities;
using ATMApp.Domain.Enums;
using ATMApp.Domain.Interfaces;
using ATMApp.UI;
using ConsoleTables;
using System;
using System.Collections.Generic;

namespace ATMApp
{
    public class ATMApp : IUserLogin, IUserAccountActions, ITransaction
    {
        
        private List<UserAccount> userAccountList;
        private UserAccount selectedAccount;
        private List<Transaction> _listOfTransactions;
        private const decimal minimumKeptAmount = 20;
        private readonly AppScreen screen;

        public ATMApp()
        {
            screen = new AppScreen();
        }

        public void CheckUserCardNumberAndPassword()
        {
            bool isCorrectLogin = false;
            while(isCorrectLogin == false)
            {
                UserAccount inputAccount = AppScreen.UserLoginForm();
                AppScreen.LoginProgress();
                foreach(UserAccount account in userAccountList)
                {
                    selectedAccount = account;
                    if(inputAccount.CardNumber.Equals(selectedAccount.CardNumber))
                    {
                        selectedAccount.TotalLogin++;

                        if(inputAccount.CardPin.Equals(selectedAccount.CardPin))
                        {
                            selectedAccount = account;

                            if(selectedAccount.IsLocked || selectedAccount.TotalLogin > 3)
                            {
                                AppScreen.PrintLockSreen();
                            }
                            else
                            {
                                selectedAccount.TotalLogin = 0;
                                isCorrectLogin = true;
                                break;
                            }
                        }
                    }
                    if (selectedAccount.TotalLogin >= 3) break;
                }
                if (isCorrectLogin == false)
                {
                    Utility.PrintMessage("\nInvalid card number or PIN.", false);
                    selectedAccount.IsLocked = selectedAccount.TotalLogin == 3;
                    if (selectedAccount.IsLocked)
                    {
                        AppScreen.PrintLockSreen();
                    }
                }
                Console.Clear();
            }
            
        }
        
        public void Run()
        {
            AppScreen.Welcome();
            CheckUserCardNumberAndPassword();
            AppScreen.WelcomeCustomer(selectedAccount.FullName);
            while (true)
            {
                AppScreen.DisplayAppMenu();
                ProcessMenuOption();
            }
        }

        private void ProcessMenuOption()
        {
            switch(Validator.Convert<int>("an option"))
            {
                case (int)AppMenu.CheckBalance:
                    CheckBalance();
                    break;
                case (int)AppMenu.PlaceDeposit:
                    PlaceDeposit();
                    break;
                case (int)AppMenu.MakeWithdrawal:
                    MakeWithdrawal();
                    break;
                case (int)AppMenu.InternalTransfer:
                    var internalTransfer = screen.InternalTransferForm();
                    ProcessInternalTransfer(internalTransfer);
                    break;
                case (int)AppMenu.ViewTransaction:
                    ViewTransactinon();
                    break;
                case (int)AppMenu.Logout:
                    AppScreen.LogOutProgress();
                    Utility.PrintMessage("You have successfully logged out. Please collect your ATM card.", true);
                    Run();
                    break;
                default:
                    Utility.PrintMessage("Invalid Option.", false);
                    break;
            }
        }

        public void InitializeData()
        {
            userAccountList = new List<UserAccount>
            {
                new UserAccount{Id = 1, FullName = "Leon Kennedy", AccountNumber = 123456, CardNumber = 321321, CardPin = 123123, AccountBalance = 50000.00m, IsLocked=false},
                new UserAccount{Id = 2, FullName = "Ada Wong", AccountNumber = 456789, CardNumber = 654654, CardPin = 456456, AccountBalance = 4000.00m, IsLocked=false},
                new UserAccount{Id = 3, FullName = "Chris Redfield", AccountNumber = 123555, CardNumber = 987987, CardPin = 789789, AccountBalance = 2000.00m, IsLocked=true},
                
            };
            _listOfTransactions = new List<Transaction>();
        }

        public void CheckBalance()
        {
            Utility.PrintMessage($"Your account balance is: {Utility.FormatAmount(selectedAccount.AccountBalance)}");
        }

        public void PlaceDeposit()
        {
            Console.WriteLine("\nMaximum of $200,000 allowed.\n");
            var transaction_amt = Validator.Convert<int>($"amount {AppScreen.cur}");

            // sumulate counting
            Console.WriteLine("\nChecking and Counting bank notes.");
            Utility.PrintDotAnimation();
            Console.WriteLine("");

            // some gaurd clause
            if(transaction_amt <= 0)
            {
                Utility.PrintMessage("Amount needs to be greater than zero. Try again.", false);
                return;
            }
            if(transaction_amt > 200000)
            {
                Utility.PrintMessage("Enter deposit amount within 200,000. Please try again.", false);
                return;
            }

            if(PreviewBankNotesCount(transaction_amt) == false)
            {
                Utility.PrintMessage($"You have cancelled your action.", false);
                return;
            }

            // bind transaction details to transaction object
            InsertTransaction(selectedAccount.Id, TransactionType.Despoit, transaction_amt, "");

            // update account balance
            selectedAccount.AccountBalance += transaction_amt;

            // print success message
            Utility.PrintMessage($"Your deposit of {Utility.FormatAmount(transaction_amt)} was successful.", true);


        }

        public void MakeWithdrawal()
        {
            var transaction_amt = 0;
            int selectedAmount = AppScreen.SelectAmount();
            if(selectedAmount == -1)
            {
                MakeWithdrawal();
                return;
            }
            else if(selectedAmount != 0)
            {
                transaction_amt = selectedAmount;
            }
            else
            {
                transaction_amt = Validator.Convert<int>($"amount {AppScreen.cur}");
            }

            // input validation
            if(transaction_amt <= 0)
            {
                Utility.PrintMessage("Amount needs to be greater than zero. Try again.", false);
                return;
            }
            if(transaction_amt > 200000)
            {
                Utility.PrintMessage("You can only withdraw amount within $200,000 USD. Please try again.", false);
                return;
            }
            // business logic validations
            if(transaction_amt > selectedAccount.AccountBalance)
            {
                Utility.PrintMessage($"Withdrawal failed. Your balance is too low to withdrawal "
                    + $"{Utility.FormatAmount(transaction_amt)}", false);
                return;
            }
            if((selectedAccount.AccountBalance - transaction_amt) < minimumKeptAmount)
            {
                Utility.PrintMessage($"Withdrawal failed. Your account needs to have " +
                    $"minimum {Utility.FormatAmount(minimumKeptAmount)}", false);
                return;
            }
            // Bind withdrawal details to transaction object
            InsertTransaction(selectedAccount.Id, TransactionType.Withdrawal, -transaction_amt, "");
            // update account balance
            selectedAccount.AccountBalance -= transaction_amt;
            // success message
            Utility.PrintMessage($"You have successfully withdrawal {Utility.FormatAmount(transaction_amt)}", true);
            
        }

        private bool PreviewBankNotesCount(int amount)
        {
            int hundredNotesCount = amount / 100;
            int fifityNotesCount = amount % 100 / 50;
            int twentyNotesCount = amount % 100 % 50 / 20;
            int tenNotesCount = amount % 100 % 50 % 20 / 10;
            int fiveNotesCount = amount % 100 % 50 % 20 % 10 / 5;
            int twoNotesCount = amount % 100 % 50 % 20 % 10 % 5 / 2;
            int oneNotesCount = amount % 100 % 50 % 20 % 10 % 5 % 2 / 1;

            Console.WriteLine("\nSummary");
            Console.WriteLine("-------");
            Console.WriteLine($"{AppScreen.cur}100 X {hundredNotesCount} = {100 * hundredNotesCount}");
            Console.WriteLine($"{AppScreen.cur}50 X {fifityNotesCount} = {50 * fifityNotesCount}");
            Console.WriteLine($"{AppScreen.cur}20 X {twentyNotesCount} = {20 * twentyNotesCount}");
            Console.WriteLine($"{AppScreen.cur}10 X {tenNotesCount} = {10 * tenNotesCount}");
            Console.WriteLine($"{AppScreen.cur}5 X {fiveNotesCount} = {5 * fiveNotesCount}");
            Console.WriteLine($"{AppScreen.cur}2 X {twoNotesCount} = {2 * twoNotesCount}");
            Console.WriteLine($"{AppScreen.cur}1 X {oneNotesCount} = {1 * oneNotesCount}");
            Console.WriteLine($"Total amount: {Utility.FormatAmount(amount)}\n\n");

            int opt = Validator.Convert<int>("1 to confirm");
            return opt.Equals(1);
        }

        public void InsertTransaction(long _UserBankAccountId, TransactionType _tranType, decimal _tranAmount, string _desc)
        {
            // create a new transaction object
            var transaction = new Transaction()
            {
                TransactionId = Utility.GetTransactionId(),
                UserBankAccountId = _UserBankAccountId,
                TransactionDate = DateTime.Now,
                TransactionType = _tranType,
                TransactionAmount = _tranAmount,
                Description = _desc
            };

            // add transaction object to the list
            _listOfTransactions.Add(transaction);
        }

        public void ViewTransactinon()
        {
            var fileredTransactionList = _listOfTransactions.Where(t => t.UserBankAccountId == selectedAccount.Id).ToList();
            // check if there's a transaction
            if(fileredTransactionList.Count <= 0)
            {
                Utility.PrintMessage("You have no transaction yet.", true);
            }
            else
            {
                var table = new ConsoleTable("Id", "Transaction Date","Type","Descriptions","Amount " + AppScreen.cur);
                foreach(var tran in fileredTransactionList)
                {
                    table.AddRow(tran.TransactionId, tran.TransactionDate, tran.TransactionType, tran.Description, tran.TransactionAmount);

                }
                table.Options.EnableCount = false;
                table.Write();
                Utility.PrintMessage($"You have {fileredTransactionList.Count} transaction(s)", true);
            }
        }

        private void ProcessInternalTransfer(InternalTransfer internalTransfer)
        {
            if(internalTransfer.TransferAmount <= 0)
            {
                Utility.PrintMessage("Amount needs to be more than zero. Try again.", false);
                return;
            }
            // check sender's account balance
            if(internalTransfer.TransferAmount > selectedAccount.AccountBalance)
            {
                Utility.PrintMessage($"Transfer failed. You do not have enough balance" +
                    $" to transfer {Utility.FormatAmount(internalTransfer.TransferAmount)}", false);
                return ;
            }
            // check the minimum kept amount
            if((selectedAccount.AccountBalance - internalTransfer.TransferAmount) < minimumKeptAmount)
            {
                Utility.PrintMessage($"Transfer failed. Your account needs to have minimum" +
                    $" {Utility.FormatAmount(minimumKeptAmount)}", false);
                return;
            }

            // check receiver's account is valid
            var selectBankAccountReceiver = (from userAcc in userAccountList
                                             where userAcc.AccountNumber == internalTransfer.RecipientBankAccountNumber
                                             select userAcc).FirstOrDefault();
            if(selectBankAccountReceiver == null)
            {
                Utility.PrintMessage("Transfer failed. Receiver bank account number is invalid.", false);
                return;
            }
            // check receiver's name
            if(selectBankAccountReceiver.FullName != internalTransfer.RecipientBankAccountName)
            {
                Utility.PrintMessage("Transfer failed. Recipient's bank account name does not match", false);
                return;
            }

            // add transaction to transaction record- sender
            InsertTransaction(selectedAccount.Id, TransactionType.Transfer, -internalTransfer.TransferAmount, "Transfered " +
                $"to {selectBankAccountReceiver.AccountNumber} ({selectBankAccountReceiver.FullName})");
            // update sender's account balance
            selectedAccount.AccountBalance -= internalTransfer.TransferAmount;

            // add transaction record-receiver
            InsertTransaction(selectBankAccountReceiver.Id, TransactionType.Transfer, internalTransfer.TransferAmount, "Transfered from" +
                $" {selectedAccount.AccountNumber} ({selectedAccount.FullName})");
            // update receiver's account balance
            selectBankAccountReceiver.AccountBalance += internalTransfer.TransferAmount;
            // print success message
            Utility.PrintMessage($"You have successfully transfered {Utility.FormatAmount(internalTransfer.TransferAmount)} to " +
                $"{internalTransfer.RecipientBankAccountName}", true);
        }
    }
}