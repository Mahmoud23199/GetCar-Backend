using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetCar.BL.DTO.BookingDtos
{
    public class InvoiceResponseDto
    {
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal Amount { get; set; }
        public DateTime IssuedDate { get; set; }
        public bool IsPaid { get; set; }
    }
    public class PaymentResponseDto
    {
        public int PaymentId { get; set; }
        public string PaymentNumber { get; set; }
        public string Status { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
    }
    public class ProcessPaymentDto
    {
        public int InvoiceId { get; set; }
        public decimal Amount { get; set; }
    }
}
