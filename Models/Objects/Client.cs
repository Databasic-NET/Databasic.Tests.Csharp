using Databasic;
using System.Collections.Generic;
using System;

namespace Models.Objects {
	[Table("Clients")]
	public class Client: Object {
		public string Name { get; set; }
		public string Fullname { get; set; }
		public decimal Discount { get; set; }
		public string Street { get; set; }
		public string City { get; set; }
		public string Zip { get; set; }
		public string Region { get; set; }
		public string Country { get; set; }

        public int? DealersCount = null;

        public Dictionary<int, Order> GetOrders () {
			return Statement.Prepare($@"
				SELECT {Columns<Order>()}
				FROM {Table<Order>()} o
				WHERE o.IdClient = @id
			").FetchAll(new {
				id = this.Id
			}).ToDictionary<int, Order>(o => o.Id.Value);
		}

		public Dictionary<int, Dealer> GetDealers () {
			return Statement.Prepare($@"
				SELECT {Columns<Dealer>()}
				FROM {Table<Dealer>()} d
				WHERE d.Id IN(
					SELECT o.IdDealer
					FROM {Table<Order>()} o
					WHERE o.IdClient = @id AND o.IdDealer IS NOT NULL
					GROUP BY o.IdDealer
				)
			").FetchAll(new {
				id = this.Id
			}).ToDictionary<int, Dealer>(d => d.Id.Value);
		}

		public static List<Client> GetWithDealersCounts () {
			return Statement.Prepare($@"
				SELECT {Columns<Client>()}, src.DealersCount
				FROM (
					SELECT src.IdClient, COUNT(src.IdDealer) AS DealersCount
					FROM (
						SELECT o.IdDealer, o.IdClient
						FROM {Table<Order>()} o
						WHERE o.IdDealer IS NOT NULL AND o.IdClient IS NOT NULL
						GROUP BY o.IdDealer, o.IdClient
					) src
					GROUP BY src.IdClient
				) src
				JOIN {Table<Client>()} AS c ON src.IdClient = c.Id
			").FetchAll().ToList<Client>();
		}

		public long GetAllClientsCount () {
			return Object.GetCount<Client>();
		}

		public long GetDealersCount () {
			return Object.GetCount<Dealer>($@"Id IN(
				SELECT o.IdDealer
				FROM {Table<Order>()} o
				WHERE o.IdClient = @id AND o.IdDealer IS NOT NULL
				GROUP BY o.IdDealer
			)", new { id = this.Id.Value });
		}
	}
}
