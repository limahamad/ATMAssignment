using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace ATM_Assignment
{
    [Serializable]
    public class Person
    {
        string firstName;
        string lastName;

        public Person(string firstName = "lima", string lastName = "hamad")
        {
            this.firstName = firstName;
            this.lastName = lastName;
        }

        public string FirstName
        {
            get
            {
                return firstName;
            }
            set
            {
                this.firstName = value;
            }
        }

        public string LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                this.lastName = value;
            }
        }
    }

    [Serializable]
    public class Bank
    {
        int bankCapacity;
        int numberOfCustomers;
        BankAccount[] accountsList;

        public Bank(int bankCapacity = 1000)
        {
            this.bankCapacity = bankCapacity;
            this.numberOfCustomers = 0;
            accountsList = new BankAccount[bankCapacity];
        }

        public void AddNewAccount(BankAccount tempAcount)
        {
            if (numberOfCustomers >= bankCapacity)
            {
                Console.WriteLine("No more customers can be added");
                return;
            }
            if (tempAcount.CardNumber.Length != 8 || tempAcount.PinCode.Length != 4)
            {
                Console.WriteLine("banck account information are not correct");
                return;
            }
            accountsList[numberOfCustomers++] = tempAcount;
            Save();
        }

        public BankAccount getAccount(string cardNum, string pinCode)
        {
            this.Load();
            for (int i = 0; i < numberOfCustomers; i++)
            {
                BankAccount p = this.accountsList[i];
                if (p.PinCode == pinCode && p.CardNumber == cardNum)
                {
                    return p;
                }
            }
            return null;
        }

        public bool IsBankUser(string cardNum, string pinCode)
        {
            if (this.getAccount(cardNum, pinCode) != null)
            {
                return true;
            }
            return false;
        }

        public int CheckBalance(string cardNum, string pinCode)
        {
            if (IsBankUser(cardNum, pinCode))
            {
                BankAccount temp = this.getAccount(cardNum, pinCode);
                return temp.AccountBalance;
            }
            else
            {
                Console.WriteLine("wrong login, bank account not found");
                return -1;
            }

        }

        public void Withdraw(BankAccount tmpAccount, int ammount)
        {
            if (IsBankUser(tmpAccount.CardNumber, tmpAccount.PinCode))
            {
                if (tmpAccount.AccountBalance >= ammount)
                {
                    for (int i = 0; i < numberOfCustomers; i++)
                    {
                        if (accountsList[i].Equals(tmpAccount))
                        {
                            accountsList[i].AccountBalance = (accountsList[i].AccountBalance - ammount);
                            Save();
                            return;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Cant withdraw this ammount");
                }
            }
            else
            {
                Console.WriteLine("wrong login, bank account not found");
            }
        }


        public void Deposit(BankAccount tmpAccount, int ammount)
        {
            if (IsBankUser(tmpAccount.CardNumber, tmpAccount.PinCode))
            {
                for (int i = 0; i < numberOfCustomers; i++)
                {
                    if (accountsList[i].Equals(tmpAccount))
                    {
                        accountsList[i].AccountBalance = (accountsList[i].AccountBalance + ammount);
                        Save();
                        return;
                    }
                }
            }
            else
            {
                Console.WriteLine("wrong login, bank account not found");
            }

        }

        // public bool checkLogin(string cardNum, string pinCode)
        // {

        //     Console.WriteLine("please enter your pinCode");
        //     //string pinCodeInput = Console.ReadLine();
        //     string pinCodeInput = pinCode;
        //     Console.WriteLine("please enter your cardNum");
        //     //string cardNumInput = Console.ReadLine();
        //     string cardNumInput = cardNum;

        //     if (IsBankUser(cardNum, pinCode) && pinCode == pinCodeInput && cardNumInput == cardNum)
        //     {
        //         return true;
        //     }
        //     else
        //     {
        //         // Console.WriteLine("not a bank user");
        //         return false;
        //     }

        // }

        public void Load()
        {
            this.numberOfCustomers = 0;
            FileStream fs2 = new FileStream("data.txt", FileMode.Open, FileAccess.Read);
            BinaryFormatter f2 = new BinaryFormatter();
            int cnt = 0;
            while (fs2.Position < fs2.Length && cnt < bankCapacity)
            {
                BankAccount p = (BankAccount)f2.Deserialize(fs2);
                this.accountsList[cnt++] = p;
                this.numberOfCustomers += 1;
            }
            fs2.Close();
        }

        public void Save()
        {
            FileStream fs = new FileStream("data.txt", FileMode.Create, FileAccess.Write);
            BinaryFormatter f = new BinaryFormatter();
            for (int i = 0; i < this.numberOfCustomers; i++)
            {
                f.Serialize(fs, this.accountsList[i]);
            }
            fs.Close();

        }

        public int NumberOfCustomers
        {
            get
            {
                return numberOfCustomers;
            }
            set
            {
                numberOfCustomers = value;
            }
        }
    }


    [Serializable]
    public class BankAccount
    {
        string email;
        string cardNumber;
        string pinCode;
        int accountBalance;
        Person p1;

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (this.GetType() != obj.GetType())
            {
                return false;
            }
            BankAccount b = (BankAccount)obj;
            return (this.cardNumber == b.cardNumber && this.pinCode == b.pinCode);
        }

        public BankAccount(Person p1, string email = " gmail", string cardNum = "320023", string pinCode = "2002", int accountBalance = 0)
        {
            this.p1 = p1;
            this.email = email;
            this.cardNumber = cardNum;
            this.pinCode = pinCode;
            this.accountBalance = accountBalance;
        }

        public string PinCode
        {
            get
            {
                return pinCode;
            }
            set
            {
                pinCode = value;
            }
        }


        public string CardNumber
        {
            get
            {
                return cardNumber;
            }
            set
            {
                cardNumber = value;
            }
        }

        public int AccountBalance
        {
            get
            {
                return accountBalance;
            }
            set
            {
                accountBalance = value;
            }
        }

        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
            }
        }

    }

    class Program
    {
        static void Main(string[] args)
        {


        }
    }
}
