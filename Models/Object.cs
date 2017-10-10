using Databasic;
using System;
using System.Runtime.CompilerServices;

namespace Models {
	[Connection("mssql-databasic-demo-mlm")]
	public class Object: Databasic.ActiveRecord.Entity {
		[PrimaryKey, AutoIncrement]
		public int? Id { get; set; }

        [CompilerGenerated]
		private static System.Collections.Concurrent.ConcurrentDictionary<string, object> _getByIdCache
			= new System.Collections.Concurrent.ConcurrentDictionary<string, object>();

		/*public static new TValue GetById <TValue> (Int64 id, Int32? connectionIndex = null) {
			TValue result;
			Type resultType = typeof(TValue);
			if (!connectionIndex.HasValue) {
				connectionIndex = Tools.GetConnectionIndexByClassAttr(resultType, true);
			}
			string cacheKey = $"{connectionIndex}_{resultType.FullName}_{id}";
			if (Object._getByIdCache.ContainsKey(cacheKey)) {
				result = (TValue)Object._getByIdCache[cacheKey];
			} else {
				result = Databasic.ActiveRecord.Entity.GetById<TValue>(id, connectionIndex);
				Object._getByIdCache[cacheKey] = result;
			}
			return result;
		}*/
	}
}