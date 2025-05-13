using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class NetUseUtil
    {

        /// <summary>
        ///
        /// </summary>
        [Flags]
        public enum ResourceScope : uint
        {
            RESOURCE_CONNECTED = 0x00000001,
            RESOURCE_GLOBALNET = 0x00000002,
            RESOURCE_REMEMBERED = 0x00000003

        };
        /// <summary>
        ///
        /// </summary>
        [Flags]
        public enum ResourceType : uint
        {
            RESOURCETYPE_ANY = 0x00000000,
            RESOURCETYPE_DISK = 0x00000001,
            RESOURCETYPE_PRINT = 0x00000002,
            RESOURCETYPE_UNKNOWN = 0xFFFFFFFF
        };
        /// <summary>
        ///
        /// </summary>
        [Flags]
        public enum ResourceUsage : uint
        {
            RESOURCEUSAGE_CONNECTABLE = 0x00000001,
            RESOURCEUSAGE_CONTAINER = 0x00000002,
            RESOURCEUSAGE_ATTACHED = 0x00000010,
            RESOURCEUSAGE_RESERVED = 0x80000000,
            RESOURCEUSAGE_ALL = (RESOURCEUSAGE_CONNECTABLE | RESOURCEUSAGE_CONTAINER | RESOURCEUSAGE_ATTACHED),
        };
        /// <summary>
        ///
        /// </summary>
        [Flags]
        public enum ResourceDisplayType : uint
        {
            RESOURCEDISPLAYTYPE_GENERIC = 0x00000000, // The method used to display the object does not matter.
            RESOURCEDISPLAYTYPE_DOMAIN = 0x00000001, // The object should be displayed as a domain.
            RESOURCEDISPLAYTYPE_SERVER = 0x00000002, // The object should be displayed as a server.
            RESOURCEDISPLAYTYPE_SHARE = 0x00000003, // The object should be displayed as a share.
            RESOURCEDISPLAYTYPE_FILE = 0x00000004,
            RESOURCEDISPLAYTYPE_GROUP = 0x00000005,
            RESOURCEDISPLAYTYPE_TREE = 0x0000000A,
            RESOURCEDISPLAYTYPE_NDSCONTAINER

        };

        [StructLayout(LayoutKind.Sequential)]
        public struct NETRESOURCE
        {
            public uint dwScope;
            public uint dwType;
            public uint dwDisplayType;
            public uint dwUsage;
            public string lpLocalName;
            public string lpRemoteName;
            public string lpComment;
            public string lpProvider;
        }

        [DllImport("mpr.dll")]
        public static extern uint WNetAddConnection3(
            [In]IntPtr hwndOwner,
            [In]ref NETRESOURCE lpNetResource,
            [In]string lpPassword,
            [In]string lpUsername,
            [In]int dwFlags
            );

        [DllImport("mpr.dll")]
        public static extern uint WNetAddConnection2A(
            [In]ref NETRESOURCE lpNetResource,
            [In]string lpPassword,
            [In]string lpUsername,
            [In]int dwFlags
            );


        [DllImport("mpr.dll")]
        public static extern uint WNetCancelConnection2A(
            [In]string lpName,
            [In]int dwFlags,
            [In]bool fForce
            );


        /// <summary>
        /// UNC接続
        /// </summary>
        /// <param name="remoteName">UNC</param>
        /// <param name="user">ユーザ名（アカウント名）</param>
        /// <param name="pass">パスワード</param>
        public static int ConnectDisk(string remoteName, string user, string pass)
        {
            int share = -1;

            try
            {
                NETRESOURCE netres = new NETRESOURCE();
                netres.dwType = (uint)ResourceType.RESOURCETYPE_DISK;
                netres.dwScope = (uint)ResourceScope.RESOURCE_GLOBALNET;
                netres.dwUsage = (uint)ResourceUsage.RESOURCEUSAGE_CONNECTABLE;
                netres.dwDisplayType = (uint)ResourceDisplayType.RESOURCEDISPLAYTYPE_SHARE;
                netres.lpRemoteName = remoteName;
                netres.lpComment = null;
                netres.lpLocalName = "";
                netres.lpProvider = null;

                share = (int)WNetAddConnection3(
                    IntPtr.Zero,
                    ref netres,
                    pass,
                    user,
                    1
                    );

            }
            catch
            {
                share = -1;
            }
            return share;
        }

        /// <summary>
        /// UNCをドライブにマップ
        /// </summary>
        /// <param name="lDisk">ドライブレター</param>
        /// <param name="remoteName">UNC</param>
        /// <param name="user">ユーザ名（アカウント名）</param>
        /// <param name="pass">パスワード</param>
        public static int MapNetDisk(string lDisk, string remoteName, string user, string pass)
        {
            int share = -1;

            try
            {
                NETRESOURCE netres = new NETRESOURCE();
                netres.dwType = (uint)ResourceType.RESOURCETYPE_DISK;
                netres.dwScope = (uint)ResourceScope.RESOURCE_GLOBALNET;
                netres.dwUsage = (uint)ResourceUsage.RESOURCEUSAGE_CONNECTABLE;
                netres.dwDisplayType = (uint)ResourceDisplayType.RESOURCEDISPLAYTYPE_SHARE;
                netres.lpRemoteName = remoteName;
                netres.lpComment = null;
                netres.lpLocalName = lDisk;
                netres.lpProvider = null;

                share = (int)WNetAddConnection3(
                    IntPtr.Zero,
                    ref netres,
                    pass,
                    user,
                    1
                    );

            }
            catch
            {
                share = -1;
            }

            return share;
        }

        /// <summary>
        /// UNCの切断
        /// </summary>
        /// <param name="remoteName">UNC</param>
        public static int DisConnectDisk(string remoteName)
        {
            int share = -1;
            int iFlags = 0;
            try
            {
                share = (int)WNetCancelConnection2A(remoteName, iFlags, true);
            }
            catch
            {
                share = -1;
            }

            return share;
        }

        /// <summary>
        /// ドライブのアンマップと切断
        /// </summary>
        /// <param name="lDisk">ドライブレター</param>
        /// <param name="remoteName">UNC</param>
        public static int DisConnectMapDisk(string lDisk, string remoteName)
        {
            int share = -1;
            int iFlags = 0;
            try
            {
                share = (int)WNetCancelConnection2A(lDisk, iFlags, true);
                if (share == 0)
                {
                    share = (int)WNetCancelConnection2A(remoteName, iFlags, true);
                }
            }
            catch
            {
                share = -1;
            }
            return share;
        }


    }
}
