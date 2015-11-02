using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Nest
{
	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	[JsonConverter(typeof(ReadAsTypeJsonConverter<FiltersAggregation>))]
	public interface IFiltersAggregation : IBucketAggregation
	{
		[JsonProperty("filters")]
		Union<INamedFiltersContainer, List<IQueryContainer>> Filters { get; set; }
	}

	public class FiltersAggregation : BucketAggregationBase, IFiltersAggregation
	{
		public Union<INamedFiltersContainer, List<IQueryContainer>> Filters { get; set; }

		internal FiltersAggregation() { }

		public FiltersAggregation(string name) : base(name) { }

		internal override void WrapInContainer(AggregationContainer c) => c.Filters = this;
	}

	public class FiltersAggregationDescriptor<T> 
		: BucketAggregationDescriptorBase<FiltersAggregationDescriptor<T>, IFiltersAggregation, T>
		, IFiltersAggregation
		where T : class
	{
		Union<INamedFiltersContainer, List<IQueryContainer>> IFiltersAggregation.Filters { get; set; }

		public FiltersAggregationDescriptor<T> NamedFilters(Func<NamedFiltersContainerDescriptor<T>, NamedFiltersContainerBase> selector) =>
			Assign(a => a.Filters = selector?.Invoke(new NamedFiltersContainerDescriptor<T>()));

		public FiltersAggregationDescriptor<T> AnonymousFilters(params Func<QueryContainerDescriptor<T>, IQueryContainer>[] selectors) =>
			Assign(a => a.Filters = selectors.Select(s=>s?.Invoke(new QueryContainerDescriptor<T>())).ToListOrNullIfEmpty());

		public FiltersAggregationDescriptor<T> AnonymousFilters(IEnumerable<Func<QueryContainerDescriptor<T>, IQueryContainer>> selectors) =>
			Assign(a => a.Filters = selectors.Select(s=>s?.Invoke(new QueryContainerDescriptor<T>())).ToListOrNullIfEmpty());

	}
}