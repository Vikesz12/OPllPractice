using System;
using Model;
using Xunit;

namespace OllPracticeTests
{
    public class CubeTurnTests
    {
        [Fact]
        public void TestCubeCreation()
        {
            //Arrange
            var cube = new Cube();

            //Act

            //Assert
            Assert.Equal($"WWW{Environment.NewLine}W W{Environment.NewLine}WWW{Environment.NewLine}" +
                         $"GGG{Environment.NewLine}G G{Environment.NewLine}GGG{Environment.NewLine}" +
                         $"OOO{Environment.NewLine}O O{Environment.NewLine}OOO{Environment.NewLine}" +
                         $"RRR{Environment.NewLine}R R{Environment.NewLine}RRR{Environment.NewLine}" +
                         $"BBB{Environment.NewLine}B B{Environment.NewLine}BBB{Environment.NewLine}" +
                         $"YYY{Environment.NewLine}Y Y{Environment.NewLine}YYY{Environment.NewLine}", cube.PrintCubeState());
        }

        [Fact]
        public void TestCubeTurnU()
        {
            //Arrange
            var cube = new Cube();

            //Act
            cube.U();

            //Assert
            Assert.Equal($"WWW{Environment.NewLine}W W{Environment.NewLine}WWW{Environment.NewLine}" +
                         $"RRR{Environment.NewLine}G G{Environment.NewLine}GGG{Environment.NewLine}" +
                         $"GGG{Environment.NewLine}O O{Environment.NewLine}OOO{Environment.NewLine}" +
                         $"BBB{Environment.NewLine}R R{Environment.NewLine}RRR{Environment.NewLine}" +
                         $"OOO{Environment.NewLine}B B{Environment.NewLine}BBB{Environment.NewLine}" +
                         $"YYY{Environment.NewLine}Y Y{Environment.NewLine}YYY{Environment.NewLine}", cube.PrintCubeState());
        }

        [Fact]
        public void TestCubeTurnUPrime()
        {
            //Arrange
            var cube = new Cube();

            //Act
            cube.UPrime();

            //Assert
            Assert.Equal($"WWW{Environment.NewLine}W W{Environment.NewLine}WWW{Environment.NewLine}" +
                         $"OOO{Environment.NewLine}G G{Environment.NewLine}GGG{Environment.NewLine}" +
                         $"BBB{Environment.NewLine}O O{Environment.NewLine}OOO{Environment.NewLine}" +
                         $"GGG{Environment.NewLine}R R{Environment.NewLine}RRR{Environment.NewLine}" +
                         $"RRR{Environment.NewLine}B B{Environment.NewLine}BBB{Environment.NewLine}" +
                         $"YYY{Environment.NewLine}Y Y{Environment.NewLine}YYY{Environment.NewLine}", cube.PrintCubeState());
        }
        [Fact]
        public void TestCubeTurnR()
        {
            //Arrange
            var cube = new Cube();

            //Act
            cube.R();

            //Assert
            Assert.Equal($"WWG{Environment.NewLine}W G{Environment.NewLine}WWG{Environment.NewLine}" +
                         $"GGY{Environment.NewLine}G Y{Environment.NewLine}GGY{Environment.NewLine}" +
                         $"OOO{Environment.NewLine}O O{Environment.NewLine}OOO{Environment.NewLine}" +
                         $"RRR{Environment.NewLine}R R{Environment.NewLine}RRR{Environment.NewLine}" +
                         $"WBB{Environment.NewLine}W B{Environment.NewLine}WBB{Environment.NewLine}" +
                         $"BYY{Environment.NewLine}B Y{Environment.NewLine}BYY{Environment.NewLine}", cube.PrintCubeState());
        }
        [Fact]
        public void TestCubeTurnRPrime()
        {
            //Arrange
            var cube = new Cube();

            //Act
            cube.RPrime();

            //Assert
            Assert.Equal($"WWB{Environment.NewLine}W B{Environment.NewLine}WWB{Environment.NewLine}" +
                         $"GGW{Environment.NewLine}G W{Environment.NewLine}GGW{Environment.NewLine}" +
                         $"OOO{Environment.NewLine}O O{Environment.NewLine}OOO{Environment.NewLine}" +
                         $"RRR{Environment.NewLine}R R{Environment.NewLine}RRR{Environment.NewLine}" +
                         $"YBB{Environment.NewLine}Y B{Environment.NewLine}YBB{Environment.NewLine}" +
                         $"GYY{Environment.NewLine}G Y{Environment.NewLine}GYY{Environment.NewLine}", cube.PrintCubeState());
        }

        [Fact]

        public void TestCubeTurnRURU()
        {
            //Arrange
            var cube = new Cube();

            //Act
            cube.R();
            cube.U();
            cube.RPrime();
            cube.UPrime();

            //Assert
            Assert.Equal($"WWO{Environment.NewLine}W G{Environment.NewLine}WWG{Environment.NewLine}" +
                         $"GGY{Environment.NewLine}G W{Environment.NewLine}GGG{Environment.NewLine}" +
                         $"BOO{Environment.NewLine}O O{Environment.NewLine}OOO{Environment.NewLine}" +
                         $"RRW{Environment.NewLine}B R{Environment.NewLine}WRR{Environment.NewLine}" +
                         $"BRR{Environment.NewLine}B B{Environment.NewLine}BBB{Environment.NewLine}" +
                         $"YYY{Environment.NewLine}Y Y{Environment.NewLine}RYY{Environment.NewLine}", cube.PrintCubeState());
        }
        [Fact]
        public void TestCubeTurnL()
        {
            //Arrange
            var cube = new Cube();

            //Act
            cube.L();

            //Assert
            Assert.Equal($"BWW{Environment.NewLine}B W{Environment.NewLine}BWW{Environment.NewLine}" +
                         $"WGG{Environment.NewLine}W G{Environment.NewLine}WGG{Environment.NewLine}" +
                         $"OOO{Environment.NewLine}O O{Environment.NewLine}OOO{Environment.NewLine}" +
                         $"RRR{Environment.NewLine}R R{Environment.NewLine}RRR{Environment.NewLine}" +
                         $"BBY{Environment.NewLine}B Y{Environment.NewLine}BBY{Environment.NewLine}" +
                         $"YYG{Environment.NewLine}Y G{Environment.NewLine}YYG{Environment.NewLine}", cube.PrintCubeState());
        }

        [Fact]
        public void TestCubeTurnLPrime()
        {
            //Arrange
            var cube = new Cube();

            //Act
            cube.LPrime();

            //Assert
            Assert.Equal($"GWW{Environment.NewLine}G W{Environment.NewLine}GWW{Environment.NewLine}" +
                         $"YGG{Environment.NewLine}Y G{Environment.NewLine}YGG{Environment.NewLine}" +
                         $"OOO{Environment.NewLine}O O{Environment.NewLine}OOO{Environment.NewLine}" +
                         $"RRR{Environment.NewLine}R R{Environment.NewLine}RRR{Environment.NewLine}" +
                         $"BBW{Environment.NewLine}B W{Environment.NewLine}BBW{Environment.NewLine}" +
                         $"YYB{Environment.NewLine}Y B{Environment.NewLine}YYB{Environment.NewLine}", cube.PrintCubeState());
        }

        [Fact]
        public void TestCubeTurnF()
        {
            //Arrange
            var cube = new Cube();

            //Act
            cube.F();

            //Assert
            Assert.Equal($"WWW{Environment.NewLine}W W{Environment.NewLine}OOO{Environment.NewLine}" +
                         $"GGG{Environment.NewLine}G G{Environment.NewLine}GGG{Environment.NewLine}" +
                         $"OOY{Environment.NewLine}O Y{Environment.NewLine}OOY{Environment.NewLine}" +
                         $"WRR{Environment.NewLine}W R{Environment.NewLine}WRR{Environment.NewLine}" +
                         $"BBB{Environment.NewLine}B B{Environment.NewLine}BBB{Environment.NewLine}" +
                         $"YYY{Environment.NewLine}Y Y{Environment.NewLine}RRR{Environment.NewLine}", cube.PrintCubeState());
        }

        [Fact]
        public void TestCubeTurnFPrime()
        {
            //Arrange
            var cube = new Cube();

            //Act
            cube.FPrime();

            //Assert
            Assert.Equal($"WWW{Environment.NewLine}W W{Environment.NewLine}RRR{Environment.NewLine}" +
                         $"GGG{Environment.NewLine}G G{Environment.NewLine}GGG{Environment.NewLine}" +
                         $"OOW{Environment.NewLine}O W{Environment.NewLine}OOW{Environment.NewLine}" +
                         $"YRR{Environment.NewLine}Y R{Environment.NewLine}YRR{Environment.NewLine}" +
                         $"BBB{Environment.NewLine}B B{Environment.NewLine}BBB{Environment.NewLine}" +
                         $"YYY{Environment.NewLine}Y Y{Environment.NewLine}OOO{Environment.NewLine}", cube.PrintCubeState());
        }

        [Fact]
        public void TestCubeTurnD()
        {
            //Arrange
            var cube = new Cube();

            //Act
            cube.D();

            //Assert
            Assert.Equal($"WWW{Environment.NewLine}W W{Environment.NewLine}WWW{Environment.NewLine}" +
                         $"GGG{Environment.NewLine}G G{Environment.NewLine}OOO{Environment.NewLine}" +
                         $"OOO{Environment.NewLine}O O{Environment.NewLine}BBB{Environment.NewLine}" +
                         $"RRR{Environment.NewLine}R R{Environment.NewLine}GGG{Environment.NewLine}" +
                         $"BBB{Environment.NewLine}B B{Environment.NewLine}RRR{Environment.NewLine}" +
                         $"YYY{Environment.NewLine}Y Y{Environment.NewLine}YYY{Environment.NewLine}", cube.PrintCubeState());
        }

        [Fact]
        public void TestCubeTurnDPrime()
        {
            //Arrange
            var cube = new Cube();

            //Act
            cube.DPrime();

            //Assert
            Assert.Equal($"WWW{Environment.NewLine}W W{Environment.NewLine}WWW{Environment.NewLine}" +
                         $"GGG{Environment.NewLine}G G{Environment.NewLine}RRR{Environment.NewLine}" +
                         $"OOO{Environment.NewLine}O O{Environment.NewLine}GGG{Environment.NewLine}" +
                         $"RRR{Environment.NewLine}R R{Environment.NewLine}BBB{Environment.NewLine}" +
                         $"BBB{Environment.NewLine}B B{Environment.NewLine}OOO{Environment.NewLine}" +
                         $"YYY{Environment.NewLine}Y Y{Environment.NewLine}YYY{Environment.NewLine}", cube.PrintCubeState());
        }

        [Fact]
        public void TestCubeTurnB()
        {
            //Arrange
            var cube = new Cube();

            //Act
            cube.B();

            //Assert
            Assert.Equal($"RRR{Environment.NewLine}W W{Environment.NewLine}WWW{Environment.NewLine}" +
                         $"GGG{Environment.NewLine}G G{Environment.NewLine}GGG{Environment.NewLine}" +
                         $"WOO{Environment.NewLine}W O{Environment.NewLine}WOO{Environment.NewLine}" +
                         $"RRY{Environment.NewLine}R Y{Environment.NewLine}RRY{Environment.NewLine}" +
                         $"BBB{Environment.NewLine}B B{Environment.NewLine}BBB{Environment.NewLine}" +
                         $"OOO{Environment.NewLine}Y Y{Environment.NewLine}YYY{Environment.NewLine}", cube.PrintCubeState());
        }

        [Fact]
        public void TestCubeTurnBPrime()
        {
            //Arrange
            var cube = new Cube();

            //Act
            cube.BPrime();

            //Assert
            Assert.Equal($"OOO{Environment.NewLine}W W{Environment.NewLine}WWW{Environment.NewLine}" +
                         $"GGG{Environment.NewLine}G G{Environment.NewLine}GGG{Environment.NewLine}" +
                         $"YOO{Environment.NewLine}Y O{Environment.NewLine}YOO{Environment.NewLine}" +
                         $"RRW{Environment.NewLine}R W{Environment.NewLine}RRW{Environment.NewLine}" +
                         $"BBB{Environment.NewLine}B B{Environment.NewLine}BBB{Environment.NewLine}" +
                         $"RRR{Environment.NewLine}Y Y{Environment.NewLine}YYY{Environment.NewLine}", cube.PrintCubeState());
        }
    }
}
