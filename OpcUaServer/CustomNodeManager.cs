using Opc.Ua;
using Opc.Ua.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpcUaServer
{
    public class CustomNodeManager:CustomNodeManager2 //создаем менеджер узлов, наследуем базовый класс, который предоставляет
                                                       //методы работы с узлами
                                                       //управление пространтсвом имен (?)
                                                       //потокобезопасность (использует lock при изменение узлов) (?)

    {
        public CustomNodeManager(IServerInternal server,
                                 ApplicationConfiguration config) : base(server, config, "http://MyOpcUaServer/namespace/") { }
        //вызываем конструктор родительском класса


        //создает иерархию уздов сервера. вызывается автоматически при старте
        //externalRefernces - словарь для хранения на узлы из других пространств имен СТАНДАРТНЫХ OPC UA
        //NodeID - ключ - идентификатор узла от которого идет ссылка
        //IReference — интерфейс, описывающий связь между узлами (например, "Папка содержит Переменную"
        //Значение: IList<IReference> — список ссылок, связанных с этим узлом.
        public override void CreateAddressSpace(IDictionary<NodeId, IList<IReference>> externalReferences)
        {
            Console.WriteLine($"Количество внешних ссылок: {externalReferences.Count}"); //для отладки

            lock (Lock)
            {
                var ammiak2 = new FolderState(null) //класс для создания папки. null - у папки нет родителя
                {
                    //идентификатор и название
                    NodeId = new NodeId("ammiak2", NamespaceIndex), //уникальный id узла
                    BrowseName = new QualifiedName("ammiak2", NamespaceIndex), //для поиска узлов в дереве
                    DisplayName = new LocalizedText("Цех аммиак 2"), //для клиентов
                    TypeDefinitionId = ObjectTypeIds.FolderType, //тип узла - у нас папка
                    EventNotifier = EventNotifiers.None //папка не генерирует события
                };



                var sensorFolder = new FolderState(null)
                {
                    
                    NodeId = new NodeId("sensorFolder", NamespaceIndex),
                    BrowseName = new QualifiedName("sensorFolder", NamespaceIndex),
                    DisplayName = new LocalizedText("Датчики"),
                    TypeDefinitionId = ObjectTypeIds.FolderType,
                    EventNotifier = EventNotifiers.None
                };


                var controlFolder = new FolderState(null)
                {

                    NodeId = new NodeId("controlFolder", NamespaceIndex),
                    BrowseName = new QualifiedName("controlFolder", NamespaceIndex),
                    DisplayName = new LocalizedText("Регулиющее оборудование"),
                    TypeDefinitionId = ObjectTypeIds.FolderType,
                    EventNotifier = EventNotifiers.None
                };

                var temperatureSensor_1000 = new AnalogItemState(null)
                {

                    //идентификатор и название
                    NodeId = new NodeId("temperatureSensor", NamespaceIndex),
                    BrowseName = new QualifiedName("temperature_1", NamespaceIndex),
                    DisplayName = new LocalizedText("Датчик температуры"),

                    //тип данных и значение
                    DataType = DataTypeIds.Double,
                    Value = 0,
                    StatusCode = StatusCodes.Good,

                    //настройка для переменной
                    TypeDefinitionId = VariableTypeIds.AnalogItemType,
                };


                AddPredefinedNode(Server.DefaultSystemContext, ammiak2); //добавляем узел в пространство узлов.
                    AddPredefinedNode(Server.DefaultSystemContext, sensorFolder);
                        AddPredefinedNode(Server.DefaultSystemContext, temperatureSensor_1000);
                    AddPredefinedNode(Server.DefaultSystemContext, controlFolder);


                ammiak2.AddReference(ReferenceTypeIds.Organizes, true, ObjectIds.ObjectsFolder);
                ammiak2.AddChild(sensorFolder);
                ammiak2.AddChild(controlFolder);
                sensorFolder.AddChild(temperatureSensor_1000);







            }
        }
    }
}
