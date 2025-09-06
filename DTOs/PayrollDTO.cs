using System.ComponentModel.DataAnnotations;
using ASCO.Models;

public class PayrollDTO
{
        public int Id { get; set; }
        public int CrewMemberId { get; set; }


        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }

        public decimal BaseWage { get; set; }
        public decimal Overtime { get; set; }
        public decimal Bonuses { get; set; }
        public decimal Deductions { get; set; }

        public string Currency { get; set; } = "USD"; //for now, later we can make it dynamic.

        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; } = "Bank Transfer"; // or "Cash"

        //public virtual User CrewMember { get; set; } = null!;
}