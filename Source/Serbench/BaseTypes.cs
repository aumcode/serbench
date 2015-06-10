using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using NFX;
using NFX.Environment;

namespace Serbench
{
  /// <summary>
  /// Provides abstract base for all test artifacts - such as serializers and tests
  /// </summary>
  public abstract class TestArtifact : INamed, IOrdered
  {
    protected TestArtifact(TestingSystem context, IConfigSectionNode conf)
    {
      Context = context;

      ConfigAttribute.Apply(this, conf);

      if (m_Name.IsNullOrWhiteSpace()) 
        m_Name = Guid.NewGuid().ToString();
    }

    [Config]
    private string m_Name;

    [Config]
    private int m_Order;


    /// <summary>
    /// The context of test execution
    /// </summary>
    public readonly TestingSystem Context;


    /// <summary>
    /// Returns the name of the test instance (do not confuse with test type)
    /// </summary>
    public string Name { get { return m_Name; }}

    /// <summary>
    /// Returns the relative order of test instance execution
    /// </summary>
    public int Order { get { return m_Order; }}

  }
}
