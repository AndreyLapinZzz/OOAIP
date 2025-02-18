using System.Collections.Generic;
using System.Runtime.Serialization;
using CoreWCF.OpenApi.Attributes;
using SpaceBattle.Lib;

namespace Endpoint
{
    [DataContract(Name = "Message")]
    public class Message //: IMessage
    {
        //1)�� ������ ��� ������� 2)��� ������ ���� �������
        [DataMember(Name = "Type", Order = 1, IsRequired = true)]
        [OpenApiProperty(Description = "��� �������")]
        public string Type { get; set; }
        //3)�� ������ ���� ���� 4)������������ ����
        [DataMember(Name = "GameID", Order = 2, IsRequired = true)]
        [OpenApiProperty(Description = "id ����")]
        public string GameID { get; set; }
        //5)�� ������ ���� ������� 6)������������ ����
        [DataMember(Name = "GameItemID", Order = 3, IsRequired = true)]
        [OpenApiProperty(Description = "id �������")]
        public string GameItemID { get; set; }
        //7)����������� ��������� �������� 8)������������ ��������� ��������
        [DataMember(Name = "Properties", Order = 4)]
        [OpenApiProperty(Description = "��������� �������� �������")]
        public IDictionary<string, object> Properties { get; set; }
    }
}
