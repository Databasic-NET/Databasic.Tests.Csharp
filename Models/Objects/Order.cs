using System;
using Databasic;
namespace Models.Objects {
	public enum OrderStatus {
		NEW,
		PREPARING,
		DELIVERING,
		DONE
	}
	[Table("Orders")]
	public class Order: Object {
		public int? IdDealer { get; set; }
		public int? IdClient { get; set; }
		public double PriceInclVat { get; set; }
		public double PriceExclVat { get; set; }
		public System.DateTime DateSubmit { get; set; }
		public System.DateTime DateDispatch { get; set; }
		public short Paid { get; set; }
		public OrderStatus Status { get; set; }

		public Dealer GetDealer () {
			if (!this.IdDealer.HasValue) return null;
			return Object.GetById<Dealer>(this.IdDealer.Value);
		}

		public Client GetClient () {
			if (!this.IdClient.HasValue) return null;
			return Object.GetById<Client>(this.IdClient.Value);
		}
	}
}
