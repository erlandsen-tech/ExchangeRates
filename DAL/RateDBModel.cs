using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL
{
    public class RateDBModel
    {
        [Key]
        public int RateId { get; set; }
        public string Symbol { get; set; }
        public Decimal Rate { get; set; }
    }

    public class ResponseDBModel
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public ICollection<RateDBModel> Rates {get; set;}
    }
}
