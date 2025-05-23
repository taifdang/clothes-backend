﻿using clothes_backend.Interfaces;
using clothes_backend.Models;
using clothes_backend.Utils.Enum;

namespace clothes_backend.Service
{
    public static class SortTypeStategy
    {
        public static ISortStrategy<Products> getSortType(SortType type)
        {
            return type switch
            {
                SortType.Default => new SortByDefault(),
                SortType.Ascending => new SortByAsc(),
                SortType.Descending => new SortByDesc(),
                SortType.Percent => new SortByPercent(),
                _ => throw new ArgumentException()
            };
        }
    }
}
