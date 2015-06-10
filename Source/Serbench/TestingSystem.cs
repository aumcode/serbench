using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NFX;
using NFX.ServiceModel;
using NFX.Environment;
using NFX.Log;

namespace Serbench
{
    /// <summary>
    /// Provides a base fo test execution environment
    /// </summary>
    public class TestingSystem : Service
    {
      #region CONSTS

        public const string CONFIG_TEST_SECTION = "test";
        public const string CONFIG_SER_SECTION = "serializer";

      #endregion

      #region .ctor/.dctor
        public TestingSystem() : base() {}

        protected override void Destructor()
        {
          base.Destructor();
        }

      #endregion

      #region Fields

        private OrderedRegistry<Test> m_Tests = new OrderedRegistry<Test>();
        private OrderedRegistry<Serializer> m_Serializers = new OrderedRegistry<Serializer>();


      #endregion


      #region Svc Protected

        protected override void DoConfigure(IConfigSectionNode node)
        {
          base.DoConfigure(node);

          foreach(var tnode in node.Children.Where(cn => cn.IsSameName(CONFIG_TEST_SECTION)))
          {
            var item = FactoryUtils.Make<Test>(tnode, args: new object[]{this, tnode});
            m_Tests.Register( item );
            log(MessageType.Info, "conf tests", "Added test {0}.'{1}'[{2}]".Args(item.GetType().FullName, item.Name, item.Order));
          }

          foreach(var snode in node.Children.Where(cn => cn.IsSameName(CONFIG_SER_SECTION)))
          {
            var item = FactoryUtils.Make<Serializer>(snode, args: new object[]{this, snode});
            m_Serializers.Register( item  );
            log(MessageType.Info, "conf sers", "Added serializer {0}.'{1}'[{2}]".Args(item.GetType().FullName, item.Name, item.Order));
          }
        }

        protected override void DoStart()
        {
          base.DoStart();
        }

        protected override void DoWaitForCompleteStop()
        {
          base.DoWaitForCompleteStop();
        }


      #endregion

      #region .pvt

       private void log(MessageType tp, string from, string text, Exception err = null)
       {
         App.Log.Write(
           new Message
           {
             Type = tp,
              Topic = "TestingSystem",
               From = "{0}.{1}".Args(GetType().FullName, from),
                Text = text,
                 Exception = err
           }

         );
       }


      #endregion


    }
}
