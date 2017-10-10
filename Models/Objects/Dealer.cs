using Databasic;
using System.Collections.Generic;
using System;

namespace Models.Objects {
	[Table("Dealers")]
	public class Dealer: Object {
		public string Name { get; set; }
		public string Surname { get; set; }
		public string Description { get; set; }
		public double TurnOverExclVat { get; set; }
		public double TurnOverInclVat { get; set; }

        public int? ClientsCount;

		public Dictionary<int, Order> GetOrders () {
			return Statement.Prepare($@"
				SELECT {Columns<Order>()}
				FROM {Table<Order>()} o
				WHERE o.IdDealer = @id
			").FetchAll(new {
				id = this.Id
			}).ToDictionary<int, Order>(o => o.Id.Value);
		}

		public static List<Dealer> GetWithClientsCounts () {
			return Statement.Prepare($@"
				SELECT {Columns<Dealer>()}, src.ClientsCount
				FROM (
				   SELECT src.IdDealer, COUNT(src.IdClient) AS ClientsCount
				   FROM (
					   SELECT o.IdDealer, o.IdClient
					   FROM {Table<Order>()} AS o
					   WHERE o.IdClient IS NOT NULL AND o.IdDealer IS NOT NULL
					   GROUP BY o.IdDealer, o.IdClient
				   ) src
				   GROUP BY src.IdDealer
			   ) src
			   JOIN {Table<Dealer>()} AS d ON src.IdDealer = d.Id
			").FetchAll().ToList<Dealer>();
		}

		public Dictionary<int, Client> GetClients () {
			return Statement.Prepare($@"
				SELECT {Columns<Client>()}
				FROM {Table<Client>()} c
				WHERE c.Id IN(
					SELECT o.IdClient
					FROM {Table<Order>()} o
					WHERE o.IdDealer = @id AND o.IdClient IS NOT NULL
					GROUP BY o.IdClient
				)
			").FetchAll(new {
				id = this.Id
			}).ToDictionary<int, Client>(c => c.Id.Value);
		}

		public long GetAllDealersCount () {
			return Object.GetCount<Dealer>();
		}

		public long GetClientsCount () {
			return Object.GetCount<Client>($@"Id IN(
				SELECT o.IdClient
				FROM {Table<Order>()} o
				WHERE o.IdDealer = @id AND o.IdClient IS NOT NULL
				GROUP BY o.IdClient
			)", new { id = this.Id.Value });
		}
	}
}
