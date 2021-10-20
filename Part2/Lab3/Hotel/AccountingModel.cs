using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelAccounting
{
    //создайте класс AccountingModel здесь
    public class AccountingModel : ModelBase
    {
        private double price;
        private int nightsCount;
        private double discount;
        private double total;
        public double Price
        {
            get { return price; }
            set
            {
                price = value >= 0 ? value : throw new ArgumentException();
                Total = price * nightsCount * (1 - discount / 100);
                Notify(nameof(Price));
            }
        }

        public int NightsCount
        {
            get { return nightsCount; }
            set
            {
                nightsCount = value > 0 ? value : throw new ArgumentException();
                Total = price * nightsCount * (1 - discount / 100);
                Notify(nameof(NightsCount));
            }
        }

        public double Discount
        {
            get { return discount; }
            set
            {
                discount = value;
                Total = price * nightsCount * (1 - discount / 100);
                Notify(nameof(Discount));
            }
        }

        public double Total
        {
            get { return total; }
            set
            {
                if ((1-discount/100)* (price * nightsCount) != value)
               {
                    discount = (1 - value / (price * nightsCount)) * 100;
                    Notify(nameof(Discount));
                }
                total = value >= 0 ? value : throw new ArgumentException();
                Notify(nameof(Total));
            }
        }
    }
}
