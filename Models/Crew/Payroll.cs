using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ASCO.Models;


namespace ASCO.Models
{
    [Table("CrewPayrolls")]
    public class Payroll
    {

        [Key]
        public int Id { get; set; }
        public int CrewMemberId { get; set; }


        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }

        public decimal BaseWage { get; set; }
        public decimal Overtime { get; set; }
        public decimal Bonuses { get; set; }
        public decimal Deductions { get; set; }

        public decimal NetPay => BaseWage + Overtime + Bonuses - Deductions;
        public string Currency { get; set; } = "USD"; //for now, later we can make it dynamic.

        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; } = "Bank Transfer"; // or "Cash"

        public virtual User CrewMember { get; set; } = null!;
    }
}