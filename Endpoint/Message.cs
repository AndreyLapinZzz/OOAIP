using System.Collections.Generic;
using System.Runtime.Serialization;
using CoreWCF.OpenApi.Attributes;
using SpaceBattle.Lib;

namespace Endpoint
{
    [DataContract(Name = "Message")]
    public class Message //: IMessage
    {
        //1)Не указан тип команды 2)Нет такого типа команды
        [DataMember(Name = "Type", Order = 1, IsRequired = true)]
        [OpenApiProperty(Description = "тип команды")]
        public string Type { get; set; }
        //3)Не указан айди игры 4)Некорректный айди
        [DataMember(Name = "GameID", Order = 2, IsRequired = true)]
        [OpenApiProperty(Description = "id игры")]
        public string GameID { get; set; }
        //5)Не указан айди объекта 6)Некорректный айди
        [DataMember(Name = "GameItemID", Order = 3, IsRequired = true)]
        [OpenApiProperty(Description = "id объекта")]
        public string GameItemID { get; set; }
        //7)Отсутствуют особенные свойства 8)Некорректные особенные свойства
        [DataMember(Name = "Properties", Order = 4)]
        [OpenApiProperty(Description = "особенные свойства объекта")]
        public IDictionary<string, object> Properties { get; set; }
    }
}
