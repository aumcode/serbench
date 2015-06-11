using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NFX.DataAccess;
using NFX.DataAccess.CRUD;

namespace Serbench.Data
{
  /// <summary>
  /// Represents a data store that SerbenchApp uses to save the data
  /// </summary>
  public interface ITestDataStore : IDataStore
  {

    /// <summary>
    /// Saves test data into the store
    /// </summary>
    void SaveTestData(Row data);

  }
}
