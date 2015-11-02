using System;
using System.Linq.Expressions;
using Newtonsoft.Json;

namespace Nest
{
	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	[JsonConverter(typeof(ReadAsTypeJsonConverter<NestedAggregation>))]
	public interface INestedAggregation : IBucketAggregation
	{
		[JsonProperty("path")] 
		FieldName Path { get; set;}
	}

	public class NestedAggregation : BucketAggregationBase, INestedAggregation
	{
		public FieldName Path { get; set; }

		internal NestedAggregation() { }

		public NestedAggregation(string name) : base(name) { }

		internal override void WrapInContainer(AggregationContainer c) => c.Nested = this;
	}

	public class NestedAggregationDescriptor<T> 
		: BucketAggregationDescriptorBase<NestedAggregationDescriptor<T>, INestedAggregation, T>
			, INestedAggregation 
		where T : class
	{
		FieldName INestedAggregation.Path { get; set; }

		public NestedAggregationDescriptor<T> Path(string path) => Assign(a => a.Path = path);

		public NestedAggregationDescriptor<T> Path(Expression<Func<T, object>> path) => Assign(a => a.Path = path);
	}
}