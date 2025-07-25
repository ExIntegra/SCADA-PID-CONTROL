using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Opc.Ua;
using Opc.Ua.Server;

namespace OpcUaServer
{
    public class CutsomOpcUaServer: StandardServer //наследуем от базового класса для запуска/останова сервера
                                                   //обработка подключений клиентов
                                                   //шифровка

    {
        private CustomNodeManager _nodeManager;
    }
}
