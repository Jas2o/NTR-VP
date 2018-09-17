using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTR_ViewerPlus {
    class NTRPacket {

        public int Magic, Sequence, Type, Command, DataLen;
        public int[] Arg;

        public NTRPacket() {
            Magic = 0x12345678;
            Sequence = 1000;
            Type = 0;
            Command = 0;
            Arg = new int[16];
            DataLen = 0;
        }

        public NTRPacket(int seq, int type, int cmd, int len = 0, params int[] arg) {
            Magic = 0x12345678;
            Sequence = seq;
            Type = type;
            Command = cmd;
            Arg = new int[16];
            for (int i = 0; i < arg.Length; i++)
                Arg[i] = arg[i];
            DataLen = len;
        }

        public NTRPacket(byte[] bytes) {
            Magic = BitConverter.ToInt32(bytes, 0);
            Sequence = BitConverter.ToInt32(bytes, 4);
            Type = BitConverter.ToInt32(bytes, 8);
            Command = BitConverter.ToInt32(bytes, 12);
            Arg = new int[16];
            for (int i = 0; i < Arg.Length; i++)
                Arg[i] = BitConverter.ToInt32(bytes, 16 + (4 * i));
            DataLen = BitConverter.ToInt32(bytes, 80);
        }

        public static NTRPacket NewRemotePlay(int seq, byte priorityFactor, byte mode, int jpegQuality = 90, int qosValue = 101) {
            return new NTRPacket(seq, 0, 901, 0, (mode << 8 | priorityFactor), jpegQuality, qosValue);
        }

        public static NTRPacket NewConnect(int seq) {
            return new NTRPacket(seq, 0, 5);
        }

        public static NTRPacket NewHeartbeat(int seq) {
            return new NTRPacket(seq, 0, 0);
        }

        public static NTRPacket NewWriteMemory(int seq, int arg1, int arg2, int arg3, int datalen) {
            return new NTRPacket(seq, 1, 10, datalen, arg1, arg2, arg3);
        }

        public byte[] GetBytes() {
            byte[] ret = new byte[84];

            BitConverter.GetBytes(Magic).CopyTo(ret, 0);
            BitConverter.GetBytes(Sequence).CopyTo(ret, 4);
            BitConverter.GetBytes(Type).CopyTo(ret, 8);
            BitConverter.GetBytes(Command).CopyTo(ret, 12);
            for(int i = 0; i < Arg.Length; i++)
                BitConverter.GetBytes(Arg[i]).CopyTo(ret, 16 + (i * 4));
            BitConverter.GetBytes(DataLen).CopyTo(ret, 80);

            return ret;
        }

    }
}
