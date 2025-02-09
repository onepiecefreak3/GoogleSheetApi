﻿using System.Reflection;
using GoogleSheetsApiV4.Contract;
using GoogleSheetsApiV4.Contract.Aspects;
using GoogleSheetsApiV4.Contract.DataClasses;

namespace GoogleSheetsApiV4
{
    internal class PropertyAspectProvider : IPropertyAspectProvider
    {
        public static readonly PropertyAspectProvider Instance = new();

        public int? GetPropertyColumn(PropertyInfo property)
        {
            var attribute = property.GetCustomAttribute<ColumnAttribute>();
            return attribute?.Column.Value;
        }

        public CellType GetPropertyCellType(PropertyInfo property)
        {
            var cellTypeAttribute = property.GetCustomAttribute<CellTypeAttribute>();
            if (cellTypeAttribute != null)
                return cellTypeAttribute.CellType;

            switch (Type.GetTypeCode(property.PropertyType))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    return CellType.Number;

                case TypeCode.Boolean:
                    return CellType.Boolean;

                default:
                    return CellType.String;
            }
        }
    }
}
