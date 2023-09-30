using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Data.SqlClient;

namespace HOAChairmanAssistant.DataLayer
{
    public class DataMigrator
    {
        public static void MoveDataFromOracleToMSSQL(string oracleConnectionString, string mssqlConnectionString, string oracleTable, string mssqlTable)
        {
            // Establish Oracle connection
            using (var oracleConnection = new OracleConnection(oracleConnectionString))
            {
                oracleConnection.Open();

                // Query Oracle database
                var oracleCommand = new OracleCommand($"SELECT * FROM {oracleTable}", oracleConnection);
                var oracleDataAdapter = new OracleDataAdapter(oracleCommand);
                var dataTable = new DataTable();
                oracleDataAdapter.Fill(dataTable);

                // Establish MS SQL Server connection
                using (var mssqlConnection = new SqlConnection(mssqlConnectionString))
                {
                    mssqlConnection.Open();

                    // Clear existing data in MS SQL Server table
                    var mssqlClearCommand = new SqlCommand($"DELETE FROM {mssqlTable}", mssqlConnection);
                    mssqlClearCommand.ExecuteNonQuery();

                    // Bulk copy data to MS SQL Server table
                    using (var bulkCopy = new SqlBulkCopy(mssqlConnection))
                    {
                        bulkCopy.DestinationTableName = mssqlTable;

                        // Get the destination table schema to retrieve column names
                        var destinationColumns = GetDestinationTableColumns(mssqlConnection, mssqlTable);

                        foreach (DataColumn sourceColumn in dataTable.Columns)
                        {
                            string sourceColumnName = sourceColumn.ColumnName;

                            // Find the matching destination column name (case-insensitive)
                            string destinationColumnName = destinationColumns.FirstOrDefault(c => string.Equals(c, sourceColumnName, StringComparison.OrdinalIgnoreCase));

                            if (destinationColumnName != null)
                            {
                                bulkCopy.ColumnMappings.Add(sourceColumnName, destinationColumnName);
                            }
                        }

                        bulkCopy.WriteToServer(dataTable);
                    }
                }
            }

            Console.WriteLine("Data successfully moved to MS SQL Server.");
        }

        private static IEnumerable<string> GetDestinationTableColumns(SqlConnection connection, string tableName)
        {
            using (var command = new SqlCommand($"SELECT TOP 0 * FROM {tableName}", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    var schemaTable = reader.GetSchemaTable();
                    var columnNames = schemaTable.Rows.Cast<DataRow>()
                        .Select(row => row["ColumnName"].ToString());
                    return columnNames;
                }
            }
        }

        public static void MoveDataFromMSSQLToOracle(string mssqlConnectionString, string oracleConnectionString, string mssqlTable, string oracleTable)
        {
            using (var mssqlConnection = new SqlConnection(mssqlConnectionString))
            {
                mssqlConnection.Open();

                // Query SQL Server database
                var mssqlCommand = new SqlCommand($"SELECT * FROM {mssqlTable}", mssqlConnection);
                var mssqlDataAdapter = new SqlDataAdapter(mssqlCommand);
                var dataTable = new DataTable();
                mssqlDataAdapter.Fill(dataTable);

                using (var oracleConnection = new OracleConnection(oracleConnectionString))
                {
                    oracleConnection.Open();

                    // Delete existing data in the Oracle table
                    using (var oracleDeleteCommand = new OracleCommand($"DELETE FROM {oracleTable}", oracleConnection))
                    {
                        oracleDeleteCommand.ExecuteNonQuery();
                    }

                    // Exclude the identity column from the column list
                    var columnsToInsert = dataTable.Columns.Cast<DataColumn>()
                        .Where(c => !c.ColumnName.Equals("IDENTITY_COLUMN", StringComparison.OrdinalIgnoreCase));

                    // Iterate through the rows and perform individual inserts
                    foreach (DataRow row in dataTable.Rows)
                    {
                        using (var oracleCommand = oracleConnection.CreateCommand())
                        {
                            // Create the INSERT statement for Oracle
                            oracleCommand.CommandText = $"INSERT INTO {oracleTable} ({string.Join(", ", columnsToInsert.Select(c => c.ColumnName))}) VALUES ({string.Join(", ", columnsToInsert.Select(c => $":{c.ColumnName}"))})";

                            // Bind parameters for each column in the Oracle INSERT statement
                            foreach (DataColumn column in columnsToInsert)
                            {
                                oracleCommand.Parameters.Add(new OracleParameter($":{column.ColumnName}", OracleDbType.Varchar2)).Value = row[column];
                            }

                            oracleCommand.ExecuteNonQuery();
                        }
                    }
                }
            }

            Console.WriteLine("Data successfully moved to Oracle.");
        }


        private static IEnumerable<string> GetDestinationTableColumns(OracleConnection connection, string tableName)
        {
            using (var command = new OracleCommand($"SELECT * FROM {tableName} WHERE ROWNUM <= 1", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    var schemaTable = reader.GetSchemaTable();
                    var columnNames = schemaTable.Rows.Cast<DataRow>()
                        .Select(row => row["ColumnName"].ToString());
                    return columnNames;
                }
            }
        }

    }
}
