using System;
using System.Collections.Generic;
using System.Text;

namespace DataTransferObjects
{
    public enum EDtoType
    {
        ChatMessage
    }

    public class Dto
    {
        public EDtoType Type { get; set; }
        public string Data { get; set; }
    }

    public static class DtoTypeMap
    {
        private static Dictionary<EDtoType, Type> s_TypeMap = new Dictionary<EDtoType, Type>()
        {
            { EDtoType.ChatMessage, typeof(ChatMessage) }
        };

        public static Dictionary<EDtoType, Type> TypeMap { get { return s_TypeMap; } }
    }
}
