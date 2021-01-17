using Enthro.Domain.Enumerations;
using System;

namespace Enthro.Domain.Dto
{
    public class IndicatorDto
    {
        public Int32 ID { get; set; }

        public Int32 Month { get; set; }

        public IndicatorType Type { get; set; }

        public Gender Gender { get; set; }

        public Double L { get; set; }

        public Double M { get; set; }

        public Double S { get; set; }

        public Double SD3N { get; set; }

        public Double SD2N { get; set; }

        public Double SD1N { get; set; }

        public Double SD0 { get; set; }

        public Double SD1P { get; set; }

        public Double SD2P { get; set; }

        public Double SD3P { get; set; }
    }
}
