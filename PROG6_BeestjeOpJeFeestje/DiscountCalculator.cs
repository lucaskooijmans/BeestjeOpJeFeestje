using Domain.Models;
using PROG6_BeestjeOpJeFeestje.ViewModels;

namespace BeestjeOpJeFeestje.Domain
{
    public class DiscountCalculator
    {
        private readonly List<Discount> _discounts;
        private int _totalDiscount;
        private int _characterDiscount;
        private int _randomNumber;

        public DiscountCalculator()
        {
            _discounts = new List<Discount>();
            GetRandomNumber();
        }

        public List<Discount> CalculateTotalDiscount(BookingSession booking)
        {
            foreach (var animal in booking.Animals)
            {
                CalculateCharacterDiscount(animal.Name);
                var duckDiscount = DuckDiscount(animal.Name, _randomNumber);
                if (duckDiscount != null) _discounts.Add(duckDiscount);
            }
            if (_characterDiscount > 0)
            {
                _discounts.Add(new Discount("Character discount: ", _characterDiscount));
            }

            var dateDiscount = DateDiscount(booking.Date);
            if (dateDiscount != null) _discounts.Add(dateDiscount);

            var typeDiscount = TypeDiscount(booking.Animals.ToList());
            if (typeDiscount != null) _discounts.Add(typeDiscount);
            return _discounts;
        }

        public decimal CalculateTotalPrice(BookingSession booking)
        {
            var totalPrice = booking.Animals.Sum(animal => animal.Price);

            totalPrice = totalPrice * (100 - _totalDiscount) / 100;
            return totalPrice;
        }

        private void CalculateCharacterDiscount(string name)
        {
            name = name.ToLower();
            for (var c = 'a'; c <= 'z'; c++)
                if (name.Contains(c) && _totalDiscount < 60)
                {
                    _totalDiscount += 2;
                    _characterDiscount += 2;

                    _characterDiscount = CalculateMaxDiscount(_totalDiscount, _characterDiscount);

                }
                else
                {
                    return;
                }
        }

        public Discount? DuckDiscount(string name, int random)
        {
            if (!name.Equals("Eend") || _totalDiscount >= 60 || random != 1) return null;
            _totalDiscount += 50;
            var discount = 50;

            discount = CalculateMaxDiscount(_totalDiscount, discount);


            return new Discount("Duck: ", discount);
        }

        private void GetRandomNumber()
        {
            _randomNumber = new Random().Next(6);
        }

        public Discount? DateDiscount(DateTime date)
        {
            if ((date.DayOfWeek != DayOfWeek.Monday && date.DayOfWeek != DayOfWeek.Tuesday) || _totalDiscount >= 60) return null;
            _totalDiscount += 15;
            var discount = 15;

            discount = CalculateMaxDiscount(_totalDiscount, discount);

            return new Discount("Day of the week discount: ", discount);
        }

        public Discount? TypeDiscount(List<Animal> animals)
        {
            if (animals.Count < 3 || _totalDiscount >= 60) return null;
            var jungleAmount = 0;
            var desertAmount = 0;
            var farmAmount = 0;
            var snowAmount = 0;
            foreach (var animal in animals)
            {
                switch (animal.Type)
                {
                    case "Boerderij":
                        farmAmount++;
                        break;
                    case "Jungle":
                        jungleAmount++;
                        break;
                    case "Sneeuw":
                        snowAmount++;
                        break;
                    case "Woestijn":
                        desertAmount++;
                        break;
                }
            }

            if (jungleAmount < 3 && desertAmount < 3 && farmAmount < 3 && snowAmount < 3) return null;
            _totalDiscount += 10;
            var discount = 10;
            discount = CalculateMaxDiscount(_totalDiscount, discount);

            return new Discount("Type discount:", discount);
        }

        public int CalculateMaxDiscount(int total, int discount)
        {
            if (total < 60) return discount;
            var temp = total - 60;
            discount -= temp;
            _totalDiscount = 60;
            return discount;
        }

        public int GetTotalDiscount()
        {
            return _totalDiscount;
        }
    }
}
