using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.InteropServices;
using Bedrock.Framework.Protocols;
using SoupBinTCP.NET.Messages;

namespace SoupBinTCP.NET
{
    public class SoupBinTcpNetProtocol : IMessageReader<IMessage>, IMessageWriter<IMessage>
    {
        public bool TryParseMessage(in ReadOnlySequence<byte> input, ref SequencePosition consumed, ref SequencePosition examined, out IMessage message)
        {
            var reader = new SequenceReader<byte>(input);

            if (input.Length < 2)
            {
                message = default;
                return false;
            }

            Span<byte> lengthBytesSpan = stackalloc byte[2];
            input
                .Slice(reader.Position, 2)
                .CopyTo(lengthBytesSpan);

            var length = BitConverter.ToUInt16(lengthBytesSpan);
            reader.Advance(2);

            if (input.Length < length + 2)
            {
                message = default;
                return false;
            }

            if (!reader.TryRead(out var messageType))
            {
                message = default;
                return false;
            }

            // We've already read out the message type, so don't count it in remaining length
            var payload = input.Slice(reader.Position, length - 1);

            if (!ParseMessage(messageType, payload, out var parsed))
            {
                message = default;
                return false;
            }
            
            consumed = payload.End;
            examined = consumed;
            message = parsed;
            return true;
        }

        public void WriteMessage(IMessage message, IBufferWriter<byte> output)
        {
            var payload = message.GetPayloadBytes();
            var length = Convert.ToUInt16(payload.Length + 1);
            
            var lengthBuffer = output.GetSpan(2);
            if (BitConverter.IsLittleEndian)
            {
                length = BinaryPrimitives.ReverseEndianness(length);
            }
            MemoryMarshal.Write(lengthBuffer, ref length);
            output.Advance(2);

            var messageType = (byte) message.MessageType;
            var typeBuffer = output.GetSpan(1);
            MemoryMarshal.Write(typeBuffer, ref messageType);
            output.Advance(1);
            
            output.Write(payload);
        }

        private bool ParseMessage(byte messageType, ReadOnlySequence<byte> payload, out IMessage message)
        {
            switch ((MessageType) messageType)
            {
                case MessageType.ClientHeartbeat:
                    message = new ClientHeartbeat(payload);
                    return true;
                case MessageType.Debug:
                    message = new Debug(payload);
                    return true;
                case MessageType.EndOfSession:
                    message = new EndOfSession(payload);
                    return true;
                case MessageType.LoginAccepted:
                    message = new LoginAccepted(payload);
                    return true;
                case MessageType.LoginRejected:
                    message = new LoginRejected(payload);
                    return true;
                case MessageType.LoginRequest:
                    message = new LoginRequest(payload);
                    return true;
                case MessageType.UnsequencedData:
                    message = new UnsequencedData(payload);
                    return true;
                case MessageType.SequencedData:
                    message = new SequencedData(payload);
                    return true;
                case MessageType.ServerHeartbeat:
                    message= new ServerHeartbeat(payload);
                    return true;
                case MessageType.LogoutRequest:
                    message = new LogoutRequest(payload);
                    return true;
                default:
                    message = default;
                    return false;
            }
        }
    }
}