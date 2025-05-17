using System.Collections.Generic;
using System.Runtime.Serialization;
using CoreWCF.OpenApi.Attributes;
using SpaceBattle.Lib;

namespace Endpoint
{
    /// <summary>
    /// Represents a message containing command information.
    /// </summary>
    [DataContract(Name = "Message")]
    public class Message
    {
        /// <summary>
        /// Type of command.
        /// </summary>
        [DataMember(Name = "Type", Order = 1, IsRequired = true)]
        [OpenApiProperty(Description = "type of command")]
        public string Type { get; set; }
        /// <summary>
        /// Game ID.
        /// </summary>
        [DataMember(Name = "GameID", Order = 2, IsRequired = true)]
        [OpenApiProperty(Description = "id game")]
        public string GameID { get; set; }
        /// <summary>
        /// Game Item ID.
        /// </summary>
        [DataMember(Name = "GameItemID", Order = 3, IsRequired = true)]
        [OpenApiProperty(Description = "id object")]
        public string GameItemID { get; set; }
        /// <summary>
        /// Special properties of the object
        /// </summary>
        [DataMember(Name = "Properties", Order = 4)]
        [OpenApiProperty(Description = "special properties of the object")]
        public IDictionary<string, object> Properties { get; set; }
    }
}
