using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace FinanzenLib.Models
{
    public class BookingModel
	{
        // Properties
        public int? ID { get; set; } // Das Fragezeichen bedeutet nullable
        public string BookingDate { get; private set; }
        public string Purpose { get; set; }
        public decimal Amount { get; set; } = 0;
        public string CategoryName { get; set; }
        public int CategoryID { get; set; }
        public string Notice { get; set; }
        public string Kind { get; set; }
        public bool IsIncome { get; set; }
        public int UserID { get; set; }                      
        public int RegularBookingID { get; set; } = -1;      
        public int BankAccountID { get; set; } = 0;          
        public int DocumentID { get; set; } = 0;             
        public bool IsOffsetBooking { get; set; } = false;   
        public bool IsSplitBooking { get; set; } = false;    
        public int? OffsetBookingID { get; set; } = 0;        
        public int OffsetBankAccountID { get; set; } = 0;
        public string BankAccountName { get; set; }

        public DateTime Date
        {
            get => Convert.ToDateTime(BookingDate);
            set => BookingDate = value.ToString("yyyy-MM-dd HH:mm:ss");
        }


        // Constructors
        public BookingModel()
        { }

        public BookingModel(BookingModel b)
        {
            //ID = b.ID;
            //BookingDate = b.BookingDate;
            Purpose = b.Purpose;
            Amount = b.Amount;
            CategoryName = b.CategoryName;
            CategoryID = b.CategoryID;
            Notice = b.Notice;
            Kind = b.Kind;
            IsIncome = b.IsIncome;
            UserID = b.UserID;
            RegularBookingID = b.RegularBookingID;
            BankAccountID = b.BankAccountID;
            DocumentID = b.DocumentID;
            IsOffsetBooking = b.IsOffsetBooking;
            IsSplitBooking = b.IsSplitBooking;
            OffsetBookingID = b.OffsetBookingID;
            OffsetBankAccountID = b.OffsetBankAccountID;
            Date = b.Date;
        }
    }
}
