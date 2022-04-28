#region Usings

using System.Linq.Expressions;
using AutoMapper;

#endregion

namespace UltimatePlaylist.Common.Mvc.Extensions
{
    public static class CreateMapExtensions
    {
        public static IMappingExpression<TSource, TDestination> ForMemberMapFrom<TSource, TDestination, TDestinationMember, TSourceMember>(
            this IMappingExpression<TSource, TDestination> mappingExpression,
            Expression<Func<TDestination, TDestinationMember>> destinationMember,
            Expression<Func<TSource, TSourceMember>> sourceMember)
        {
            return mappingExpression.ForMember(destinationMember, opt => opt.MapFrom(sourceMember));
        }

        public static IMappingExpression<TSource, TDestination> ForMemberSetValue<TSource, TDestination, TDestinationMember, TValue>(
            this IMappingExpression<TSource, TDestination> mappingExpression,
            Expression<Func<TDestination, TDestinationMember>> destinationMember,
            TValue value)
        {
            return mappingExpression.ForMember(destinationMember, opt => opt.MapFrom(_ => value));
        }
    }
}
