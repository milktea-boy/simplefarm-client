using System;

namespace Constant
{
    public class Constants
    {
        // 服务器是否连接成功
        public static bool socketConnected = false;

        public static string IP = "127.0.0.1";
        public static int PORT = 10000;


        //用于存放报文中有效信息体长度变量的字节数
        public const int MSGLENTH = 4;
    }

}