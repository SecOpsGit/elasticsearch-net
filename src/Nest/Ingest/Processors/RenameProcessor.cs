﻿using System;
using System.Linq.Expressions;
using Newtonsoft.Json;

namespace Nest
{
	[JsonObject(MemberSerialization.OptIn)]
	[JsonConverter(typeof(ProcessorJsonConverter<RenameProcessor>))]
	public interface IRenameProcessor : IProcessor
	{
		[JsonProperty("field")]
		Field Field { get; set; }

		[JsonProperty("target_field")]
		Field TargetField { get; set; }
	}

	public class RenameProcessor : ProcessorBase, IRenameProcessor
	{
		public Field Field { get; set; }
		public Field TargetField { get; set; }
		protected override string Name => "rename";
	}

	public class RenameProcessorDescriptor<T>
		: ProcessorDescriptorBase<RenameProcessorDescriptor<T>, IRenameProcessor>, IRenameProcessor
		where T : class
	{
		protected override string Name => "rename";
		Field IRenameProcessor.Field { get; set; }
		Field IRenameProcessor.TargetField { get; set; }

		public RenameProcessorDescriptor<T> Field(Field field) => Assign(a => a.Field = field);

		public RenameProcessorDescriptor<T> Field(Expression<Func<T, object>> objectPath) =>
			Assign(a => a.Field = objectPath);

		public RenameProcessorDescriptor<T> TargetField(Field field) => Assign(a => a.TargetField = field);

		public RenameProcessorDescriptor<T> TargetField(Expression<Func<T, object>> objectPath) =>
			Assign(a => a.TargetField = objectPath);
	}
}
