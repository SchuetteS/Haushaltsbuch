using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanzenLib.Models
{
    public class BankAccountModel
    {
        public int ID { get; set; }                 // ID INTEGER[pk]
        public DateTime StartDate { get; set; }     // StartDate DATETIME
        public string Name { get; set; }            // Name TEXT
        public decimal StartBalance { get; set; }   // StartBalance CURRENCY
        public int UserID { get; set; }             // UserID VARCHAR(3)
        public bool IsDefault { get; set; }         // IsDefault LOGICAL
        public string Notice { get; set; }          // Notice TEXT
        public bool IsActive { get; set; }          // IsActive LOGICAL

        public BankAccountModel()
        { }

        public BankAccountModel(BankAccountModel b)
        {
            ID = b.ID;
            StartDate = b.StartDate;
            Name = b.Name;
            StartBalance = b.StartBalance;
            UserID = b.UserID;
            IsDefault = b.IsDefault;
            Notice = b.Notice;
            IsActive = b.IsActive;
        }
    }
}
