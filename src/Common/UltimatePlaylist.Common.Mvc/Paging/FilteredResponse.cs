#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UltimatePlaylist.Common.Extensions;
using UltimatePlaylist.Common.Filters.Enums;
using UltimatePlaylist.Common.Mvc.Attributes;

#endregion

namespace UltimatePlaylist.Common.Mvc.Paging
{
    public class FilteredResponse<TItems>
        where TItems : class
    {
        #region Constructor(s)

        public FilteredResponse(IList<TItems> items)
        {
            var type = typeof(TItems);
            List = items;
            FilterColumns = GetAllPropertiesWithFilteringOptions(type);
        }

        protected FilteredResponse()
        {
        }

        #endregion

        #region Public Properties

        public IList<TItems> List { get; set; }

        public List<FilterOptions> FilterColumns { get; set; }

        #endregion

        #region Private Methods

        private List<FilterOptions> GetAllPropertiesWithFilteringOptions(Type type)
        {
            var allProps = new List<FilterOptions>();

            if (type == typeof(string) || type == typeof(DateTime))
            {
                return null;
            }

            if (type.AssemblyQualifiedName.Contains("System.Collections.Generic"))
            {
                foreach (var property in type.GetProperties())
                {
                    var children = GetAllPropertiesWithFilteringOptions(property.PropertyType);
                    if (children != null && children.Any())
                    {
                        allProps.AddRange(children);
                    }
                    else
                    {
                        var descriptors = TypeDescriptor.GetProperties(property.PropertyType);
                        SetProperties(descriptors, allProps);
                    }
                }
            }
            else
            {
                var descriptors = TypeDescriptor.GetProperties(type);
                SetProperties(descriptors, allProps);
            }

            return allProps;
        }

        private void SetProperties(PropertyDescriptorCollection descriptors, List<FilterOptions> allProps)
        {
            foreach (PropertyDescriptor prop in descriptors)
            {
                var children = GetAllPropertiesWithFilteringOptions(prop.PropertyType);
                if (children != null && children.Any())
                {
                    allProps.AddRange(children);
                }
                else
                {
                    var options = GetFilteringOptions(prop);
                    if (options != null)
                    {
                        allProps.Add(options);
                    }
                }
            }
        }

        private FilterOptions GetFilteringOptions(PropertyDescriptor prop)
        {
            var attr = prop.Attributes.OfType<FilterColumnAttribute>()?.FirstOrDefault();

            if (attr == null)
            {
                return null;
            }

            var filterOptions = new FilterOptions(attr.Name, prop.Name.FirstToLower());

            if (attr.OverridenBy != null)
            {
                if (attr.OverridenBy.Value.Value == typeof(int) || attr.OverridenBy.Value.Value == typeof(long) || attr.OverridenBy.Value.Value == typeof(decimal) || attr.OverridenBy.Value.Value == typeof(double))
                {
                    filterOptions.Kind = FilterKind.Int;
                    filterOptions.Name = attr.OverridenBy.Value.Key.FirstToLower();
                }

                return filterOptions;
            }

            if (attr.Without != null)
            {
                filterOptions.Kind = FilterKind.Enum;
                var type = prop.PropertyType.GetGenericArguments()[0];
                filterOptions.PossibleValues = Enum.GetNames(type);
                attr.Without.ForEach(w =>
                    {
                        filterOptions.PossibleValues = filterOptions.PossibleValues.Where(f => !w.Contains(f.ToString()));
                    });
                return filterOptions;
            }

            if (prop.PropertyType.FullName.Contains("System.Collections.Generic"))
            {
                var property = prop.PropertyType.GetGenericArguments()[0];
                filterOptions = GetFilteringOptionForPropertyDescriptor(property, filterOptions, attr);
            }
            else
            {
                filterOptions = GetFilteringOptionForPropertyDescriptor(prop.PropertyType, filterOptions, attr);
            }

            return filterOptions;
        }

        private FilterOptions GetFilteringOptionForPropertyDescriptor(Type type, FilterOptions filterOptions, FilterColumnAttribute attr)
        {
            if (type.IsEnum)
            {
                filterOptions.Kind = FilterKind.Enum;
                filterOptions.PossibleValues = Enum.GetNames(type);
            }
            else if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                filterOptions.Kind = FilterKind.Date;
            }
            else if (type == typeof(int) || type == typeof(int?) || type == typeof(long) || type == typeof(long?) || type == typeof(decimal) || type == typeof(decimal?) || type == typeof(double) || type == typeof(double?))
            {
                filterOptions.Kind = FilterKind.Int;
            }
            else if (type == typeof(bool))
            {
                filterOptions.Kind = FilterKind.Enum;

                if (attr.NamedValues != null && attr.NamedValues.Any())
                {
                    filterOptions.PossibleValues = attr.NamedValues;
                }
                else
                {
                    filterOptions.PossibleValues = new[] { true.ToString(), false.ToString() };
                }
            }
            else
            {
                filterOptions.Kind = FilterKind.String;
            }

            return filterOptions;
        }

        #endregion
    }
}