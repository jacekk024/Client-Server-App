using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.ServerCommands
{
    class ConfCommand : IServiceModule
    {

        private AddListnerD addListener;
        private AddServiceModuleD addService;
        private RemoveServiceModuleD removeServiceModule;
        private RemoveListnerD removeListener;


        ConfCommand()
        {
        
        
        }

        public string AnswerCommand(string command)
        {

            throw new NotImplementedException();
        }

        public string AddServiceModule(string name, IServiceModule service)
        {
            return "XYZ";

        }

        public string AddCommunicator(ICommunicator communicator)
        {
            return "XYZ";
        }    

        public string AddListener(IListner listener)
        {
            return "XYZ";
        }

        public string RemoveServiceModule(string name, IServiceModule service)
        {
            return "XYZ";
        }

        public string RemoveCommunicator(ICommunicator communicator)
        {
            return "XYZ";
        }

        public string RemoveListener(IListner listener)
        {
            return "XYZ";
        }

        delegate void AddListnerD(IListner listner);
        delegate void AddCommunicatorD(ICommunicator communicator);
        delegate void AddServiceModuleD(IServiceModule serviceModule);
        delegate void RemoveListnerD(IListner listner);
        delegate void RemoveCommunicatorD(ICommunicator communicator);
        delegate void RemoveServiceModuleD(IServiceModule serviceModule);
    }
}
