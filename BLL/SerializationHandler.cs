using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    class SerializationHandler
    {
        public SuccessResponse DeSerializeJson(string json)
        {
            try
            {
                var convertedResponse = JsonConvert.DeserializeObject<Response>(json);
                if (convertedResponse.Success)
                {
                    return JsonConvert.DeserializeObject<SuccessResponse>(json);
                }
                else
                {
                    var error = JsonConvert.DeserializeObject<ErrorResponse>(json);
                    throw new Exception($"Response from fixer did not indicate success.\n" +
                                        $"{error.Error.Info}");
                }
            }
            catch(Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
