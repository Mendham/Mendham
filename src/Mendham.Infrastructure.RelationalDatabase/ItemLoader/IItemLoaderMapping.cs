﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Infrastructure.RelationalDatabase
{
    public interface IItemLoaderMapping<T>
    {
        /// <summary>
        /// Message to be returned when a items are invalid
        /// </summary>
        string InvalidSetErrorMessage { get; }

        /// <summary>
        /// Sql statement to create temporary table that holds values
        /// </summary>
        string CreateTableSql { get; }

        /// <summary>
        /// Name of temporary table that holds set values
        /// </summary>
        string TableName { get; }

        /// <summary>
        /// Parameterized statement to insert item into temporary table
        /// </summary>
        string InsertItemSql { get; }

        /// <summary>
        /// Sql statement to drop temporary table that holds values. The statement should return a 1
        /// if the drop was successful or a 0 if it was not
        /// </summary>
        string DropTableSql { get; }

        /// <summary>
        /// Test to make sure an item in a set is valid (for example not the default value)
        /// </summary>
        /// <param name="item"></param>
        /// <returns>True if valid, false if not</returns>
        bool ItemIsValidPredicate(T item);

        /// <summary>
        /// Generates value to be passed in param field for insert statement for a given item
        /// </summary>
        /// <param name="item">Item in set</param>
        /// <returns>Anonymous type object that contains parmas to be used in insert statement</returns>
        dynamic GetParamForInsert(T item);
    }
}
