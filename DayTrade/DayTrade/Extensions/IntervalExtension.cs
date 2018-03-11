using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayTrade
{
    public class Interval
    {
        private const char Minutes = 'm';
        private const char Hours = 'h';
        private const char Days = 'd';

        int period;
        char unit;

        public Interval(string interval)
        {
            ExtractIntervalPeriodAndUnit(interval);
        }

        private Interval(int period, char unit)
        {
            this.period = period;
            this.unit = unit;
        }

        public int InMinutes()
        {
            return ConvertIntervalPeriod(unit, period, Minutes);
        }
        public char Unit
        {
            get
            {
                return unit;
            }
        }
        public static Interval operator +(Interval a, Interval b)
        {
            var newIntervalUnit = DetermineIntervalUnit(a.Unit, b.Unit);
            var oldPeriodConverted = ConvertIntervalPeriod(a.unit, a.period, newIntervalUnit);
            var newPeriodConverted = ConvertIntervalPeriod(b.unit, b.period, newIntervalUnit);
            return new Interval(oldPeriodConverted + newPeriodConverted, newIntervalUnit);
        }

        public override string ToString()
        {
            return $"{period}{unit}";
        }

        private static char DetermineIntervalUnit(char oldUnit, char newUnit)
        {
            if (oldUnit == Minutes || newUnit == Minutes) return Minutes;
            if (oldUnit == Hours || newUnit == Hours) return Hours;
            return Days;
        }

        private void ExtractIntervalPeriodAndUnit(string interval)
        {
            period = int.Parse(interval.Substring(0, interval.Length - 1));
            unit = interval[interval.Length - 1]; //last char
        }

        private static int ConvertIntervalPeriod(char unit, int period, char newIntervalUnit)
        {
            switch (newIntervalUnit)
            {
                case Minutes:
                    switch (unit)
                    {
                        case Minutes:
                            return period;
                        case Hours:
                            return 60 * period;
                        case Days:
                            return 24 * 60 * period;
                    }
                    break;
                case Hours:
                    switch (unit)
                    {
                        case Hours:
                            return period;
                        case Days:
                            return 24 * period;
                    }
                    break;
                case Days:
                    if (unit == Days)
                        return period;
                    break;
            }
            throw new InvalidOperationException($"Invalid interval convertion between /'{unit}/' and /'{newIntervalUnit}/'");
        }

    }
}
