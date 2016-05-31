﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using CodeJam.Expressions;

using JetBrains.Annotations;

namespace CodeJam.Mapping
{
	/// <summary>
	/// Maps an object of <i>TFrom</i> type to an object of <i>TTo</i> type.
	/// </summary>
	/// <typeparam name="TFrom">Type to map from.</typeparam>
	/// <typeparam name="TTo">Type to map to.</typeparam>
	[PublicAPI]
	public class Mapper<TFrom,TTo>
	{
		MappingSchema                                  _mappingSchema;
		List<Tuple<LambdaExpression,LambdaExpression>> _memberMappers;

		/// <summary>
		/// Sets mapping schema.
		/// </summary>
		/// <param name="schema">Mapping schema to set.</param>
		/// <returns>Returns this mapper.</returns>
		[Pure]
		public Mapper<TFrom,TTo> SetMappingSchema(MappingSchema schema)
		{
			_mappingSchema = schema;
			return this;
		}

		/// <summary>
		/// Returns a mapper expression to map an object of <i>TFrom</i> type to an object of <i>TTo</i> type.
		/// </summary>
		/// <returns>Mapping expression.</returns>
		[Pure]
		public Expression<Func<TFrom,TTo>> GetMapperExpression()
			=> new ExpressionMapper<TFrom,TTo>
			{
				MappingSchema = _mappingSchema ?? MappingSchema.Default,
				MemberMappers = _memberMappers?.Select(mm => Tuple.Create(mm.Item1.GetMembersInfo(), mm.Item2)).ToArray(),
			}
			.GetExpression();

		/// <summary>
		/// Returns a mapper to map an object of <i>TFrom</i> type to an object of <i>TTo</i> type.
		/// </summary>
		/// <returns>Mapping expression.</returns>
		[Pure]
		public Func<TFrom,TTo> GetMapper() => GetMapperExpression().Compile();

		/// <summary>
		/// Adds member mapper.
		/// </summary>
		/// <typeparam name="T">Type of the member to map.</typeparam>
		/// <param name="toMember">Expression that returns a member to map.</param>
		/// <param name="setter">Expression to set the member.</param>
		/// <returns>Returns this mapper.</returns>
		public Mapper<TFrom,TTo> MapMember<T>(Expression<Func<TTo,T>> toMember, Expression<Func<TFrom,T>> setter)
		{
			if (_memberMappers == null)
				_memberMappers = new List<Tuple<LambdaExpression,LambdaExpression>>();

			_memberMappers.Add(Tuple.Create((LambdaExpression)toMember, (LambdaExpression)setter));

			return this;
		}
	}
}