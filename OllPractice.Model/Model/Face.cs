using System;
using System.Collections.Generic;
using System.Linq;

namespace OllPractice.Model.Model
{
    public class Face
    {
        private ulong _squares;

        public Face(RubikColor color)
        {
            _squares = 0;
            for (var index = 0; index < 8; index++)
            {
                _squares = _squares << 8 | (byte)color;
            }
        }

        public Face(IEnumerable<RubikColor> colors)
        {
            _squares = 0;
            foreach (var rubikColor in colors)
            {
                _squares = _squares << 8 | (byte) rubikColor;
            }
        }

        public void Rotate(Rotation rotation)
        {
            var shift = 16;
            switch (rotation)
            {
                case Rotation.ONE:
                    break;
                case Rotation.TWO:
                    shift = 32;
                    break;
                case Rotation.PRIME:
                    _squares = (_squares >> shift) | (_squares << (64 - shift));
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rotation), rotation, null);
            }
            _squares = (_squares << shift) | (_squares >> (64 - shift));
        }

        public byte[] RotateSide(int startIndex, byte[] newSquares = default)
        {
            var bytes = BitConverter.GetBytes(_squares);
            var oldSquares = new byte[3];
            if (startIndex == 6)
            {
                oldSquares[0] = bytes[6];
                oldSquares[1] = bytes[7];
                oldSquares[2] = bytes[0];
            }
            else
            {
                oldSquares = bytes.ToList().GetRange(startIndex, 3).ToArray();
            }
            if (newSquares != default && oldSquares != newSquares)
            {
                bytes[startIndex] = newSquares[0];
                bytes[startIndex+1] = newSquares[1];
                bytes[startIndex == 6 ? 0: startIndex+2] = newSquares[2];
                _squares = BitConverter.ToUInt64(bytes,0);
            }
            return oldSquares;
        }

        public RubikColor GetColorAt(int index)
        {
            var bytes = BitConverter.GetBytes(_squares);
            return (RubikColor) bytes[index];
        }

        public string PrintSide()
        {
            var bytes = BitConverter.GetBytes(_squares);
            var result = "";
            result += (RubikColor)bytes[0];
            result += (RubikColor)bytes[1];
            result += (RubikColor)bytes[2];
            result += Environment.NewLine;
            result += (RubikColor)bytes[7];
            result += " ";
            result += (RubikColor)bytes[3];
            result += Environment.NewLine;
            result += (RubikColor)bytes[6];
            result += (RubikColor)bytes[5];
            result += (RubikColor)bytes[4];
            result += Environment.NewLine;
            return result;
        }
    }
}
