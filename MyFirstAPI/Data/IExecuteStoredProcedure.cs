using System.Data;

    namespace MyFirstAPI.Data
    {
        public interface IExecuteStoredProcedure
        {

            public Task<DataSet> CallStoredProcedure (String StoredProcedure, Dictionary<String, Object> Parameters);
        }
    }
