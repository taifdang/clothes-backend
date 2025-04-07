﻿namespace clothes_backend.Inteface
{
    public interface ISortStrategy<T>
    {
        IEnumerable<T> Sort(IEnumerable<T> values);
    }
}
