using Model;
using DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class DbLogic
    {
        public bool Insert(SuccessResponse response)
        {
            CurrencyDBContext context = new CurrencyDBContext();
            var DbOp = new DbOperations(context);
            var convertedResponse = convertResponseToDB(response);
            return DbOp.InsertResponse(convertedResponse);
        }
        private static ResponseDBModel convertResponseToDB(SuccessResponse response)
        {
            var rates = new List<RateDBModel>();
            var responseDBM = new ResponseDBModel();
            foreach(var rate in response.Rates)
            {
                var rateDBModel = new RateDBModel();
                rateDBModel.Symbol = rate.Key;
                rateDBModel.Rate = rate.Value;
                rates.Add(rateDBModel);
            }
            responseDBM.DateTime = DateTime.Now;
            responseDBM.Rates = rates;
            return responseDBM;
        }
    }
}
