using System;
using CSharpMortgageCalc.Models;

namespace CSharpMortgageCalc.Helpers
{
    public class LoanHelper
    {
        public Loan GetPayments(Loan loan)
        {
            // calculate monthly payment
            loan.Payment = CalcPayment(loan.Amount, loan.Rate, loan.Term);

            // declare payment variables
            var balance = loan.Amount;
            var totalInterest = 0.0m;
            var monthlyInterest = 0.0m;
            var monthlyPrincipal = 0.0m;
            var monthlyRate = CalcMonthlyRate(loan.Rate);

            // calculate payments for each month for amortization schedule
            for (int month = 1; month <= loan.Term; ++month)
            {
                monthlyInterest = CalcMonthlyInterest(balance, monthlyRate);
                totalInterest += monthlyInterest;   
                monthlyPrincipal = loan.Payment - monthlyInterest;
                balance -= monthlyPrincipal;

                LoanPayment loanPayment = new();
                loanPayment.Month = month;
                loanPayment.Payment = loan.Payment;
                loanPayment.MonthlyPrincipal = monthlyPrincipal;
                loanPayment.MonthlyInterest = monthlyInterest;
                loanPayment.TotalInterest = totalInterest;
                loanPayment.Balance = balance;

                loan.Payments.Add(loanPayment);
            }
             
            loan.TotalInterest = totalInterest;
            loan.TotalCost = loan.Amount + totalInterest;
      
            return  loan;
        }
        public decimal GetTotalInterest(Loan loan)
        {
            return loan.Amount * (loan.Rate / 100) * loan.Term;
        }
        public decimal GetTotalPayments(Loan loan)
        {
            return GetTotalInterest(loan) + loan.Amount;
        }

        private decimal CalcPayment(decimal amount, decimal rate, int term)
        {
            var Monthlyrate = CalcMonthlyRate(rate);

            var monthlyRateD = Convert.ToDouble(Monthlyrate);
            var amountD = Convert.ToDouble(amount);

            var paymentD = (amountD * monthlyRateD) / (1-Math.Pow(1+ monthlyRateD, -term));
            
            return Convert.ToDecimal(paymentD);
        }
        private decimal CalcMonthlyRate(decimal rate)
        {
            return rate / 1200;
        } 
        private decimal CalcMonthlyInterest(decimal balance, decimal monthlyRate)
        {
            return balance * monthlyRate;   
        }
    }
}
