using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DAL
{
    public class DbOperations
    {

        private readonly CurrencyDBContext _context;

        public DbOperations(CurrencyDBContext context)
        {
            _context = context;
        }

        public bool InsertResponse(ResponseDBModel response)
        {
            try
            {
                _context.Responses.Add(response);
                _context.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                throw new Exception($"Unable to insert into database.{ex.Message}");
            }
        }
    }
}
