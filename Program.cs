using Models;
using Models.Objects;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

class Program {

	static void Main () {
		
		Databasic.Events.Error += (System.Exception ex, Databasic.SqlErrorsCollection sqlErrors) => {
			Desharp.Debug.Dump(ex);
			Desharp.Debug.Dump(sqlErrors);
		};
        
        List<Thread> threads = new List<Thread>();
        Thread t;
        for (int i = 0; i < 8; i += 1) {
            t = new Thread(Program.threadStart(i));
            t.IsBackground = true;
            threads.Add(t);
            t.Start();
        }

        System.Console.ReadLine();
	}
	private static ThreadStart threadStart (int i) {
		string timerKey = "Thread " + i.ToString();
		List<double> times = new List<double>();
		return new ThreadStart(delegate {
			Desharp.Debug.Timer(timerKey, true);
			for (int j = 0; j < 100; j += 1) {
				Program.test();
				times.Add(Desharp.Debug.Timer(timerKey, true));
			}
			Desharp.Debug.Dump(times.Average());
		});
	}
	
	public static void test () {
		var d1 = Object.GetById<Dealer>(500);
		var d1c = d1.GetClients();
		var d1o = d1.GetOrders();
		var d1cc = d1.GetClientsCount();

        var c1 = Object.GetByKey<Client>(new { Id = 2 });
		var c1d = c1.GetDealers();
		var c1o = c1.GetOrders();
		var c1dc = c1.GetDealersCount();

		var o1 = Object.GetById<Order>(740994);
		Dealer o1d = o1.GetDealer();
		Client o1c = o1.GetClient();

		List<Dealer> dealersWithClientsCounts = Dealer.GetWithClientsCounts();
		List<Client> clientsWithDealersCounts = Client.GetWithDealersCounts();

		var top10newOrds = Object.GetDictionary<int, Order>(
			o => o.Id.Value, "Status = 'NEW'", null,  "DateSubmit DESC", 0, 10
		);
		var next10newOrds = Object.GetList<Order>(
			"Status = 'NEW'", null,  "DateSubmit DESC", 10, 10
		);
		//Desharp.Debug.Dump(next10newOrds);

		Databasic.Connection.Close();
	}
}