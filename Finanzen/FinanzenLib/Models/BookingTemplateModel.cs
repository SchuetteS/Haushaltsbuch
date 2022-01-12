using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanzenLib.Models
{
    class BookingTemplateModel
    {
        public int ID { get; set; }                     // ID INTEGER[pk]
        public string Title { get; set; }               // Title TEXT
        public string Purpose { get; set; }             // Purpose TEXT
        public decimal Amount { get; set; }             // Amount CURRENCY
        public string Notice { get; set; }              // Notice TEXT
        public string Kind { get; set; }                // Kind TEXT
        public string CategoryName { get; set; }        // CategoryName TEXT(255)
        public int CategoryID { get; set; }             // CategoryID VARCHAR(10)
        public int UserID { get; set; }                 // UserID VARCHAR(3)
        public int BankAccountID { get; set; }          // BankAccountID VARCHAR(10)
        public bool IsOffsetBooking { get; set; }       // IsOffsetBooking LOGICAL
        public int OffsetBankAccountID { get; set; }    // OffsetBankAccountID VARCHAR(10)
    }
}
