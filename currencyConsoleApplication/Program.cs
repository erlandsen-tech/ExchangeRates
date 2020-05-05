using BLL;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace currencyConsoleApplication
{
    public class Program
    {
        private static int Main(string[] args)
        {
        var calc = new Calculation();
            var rootCommand = new RootCommand
            {
                new Option<string>(
                    "--from",
                    getDefaultValue: () => "NOK",
                    description: "From currency. Should be of valid currency symbol. Check --symbols for list."),
                new Option<string>(
                    "--to",
                    getDefaultValue: () => "SEK",
                    description: "To currency. Should be of valid currency symbol. Check --symbols for list."),
                new Option<string>(
                    "--date",
                    getDefaultValue: () => "latest",
                    description: "Date in format yyyy-dd-mm for historical data"),
                new Option<decimal>(
                    "--amount",
                    getDefaultValue: () => 1,
                    "Amount of currency to convert in dot decimal format. Amount is given in the 'to' currency.")
            };
            rootCommand.Description = "Currencyconverter. Takes a valid currency symbol as --to and --from and converts.";
            rootCommand.Handler = CommandHandler.Create<string, string, string, decimal>(async (from, to, date, amount) =>
            {
                var result = await calc.FromToAmount(from.ToUpper(), to.ToUpper(), amount, date);
                Console.WriteLine(result);
            });
            rootCommand.AddCommand(getSymbols());
            rootCommand.AddCommand(getAllRates());
            // Parse the incoming args and invoke the handler
            return rootCommand.InvokeAsync(args).Result;
        }

        private static Command getSymbols()
        {
            var cmd = new Command("--symbols", "Gets all valid currency symbols.");
            cmd.Handler = CommandHandler.Create(() =>
            {
                Calculation calc = new Calculation();
                var symbols = calc.GetSymbols();
                Console.WriteLine("Valid symbols will follow.");
                foreach(var item in symbols.Result)
                {
                    Console.WriteLine(item);
                }
            });
            return cmd;
        }

        private static Command getAllRates()
        {
            var cmd = new Command("--all", "Gets all currencies with base Euro.");
            cmd.Handler = CommandHandler.Create(() =>
            {
                NetworkHandler handler = new NetworkHandler();
                var response = handler.GetRates();
                Console.WriteLine("All stored currencies with base Euro will follow.");
                foreach(var item in response.Result.Rates)
                {
                    Console.WriteLine(item);
                }
            });
            return cmd;
        }
    }
}